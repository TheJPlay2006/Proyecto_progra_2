using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SistemaDeTickets.Services
{
    public static class ServicioAutenticacion
    {
        // Usuario actual (singleton thread-safe para persistencia de sesión)
        private static Usuario _currentUser = null;
        private static readonly object _lock = new object();

        public static Usuario CurrentUser
        {
            get
            {
                lock (_lock)
                {
                    return _currentUser;
                }
            }
            private set
            {
                lock (_lock)
                {
                    _currentUser = value;
                }
            }
        }

        // Constantes para PBKDF2
        private const int Iteraciones = 100000;
        private const int TamanoSalt = 16;
        private const int TamanoHash = 32;

        // Constantes para tokens de recuperación
        private const int TamanoToken = 32; // 32 bytes

        /// <summary>
        /// Genera token aleatorio seguro de 32 bytes en Base64
        /// </summary>
        private static string GenerarTokenAleatorio()
        {
            byte[] token = new byte[TamanoToken];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(token);
            }
            return Convert.ToBase64String(token);
        }

        /// <summary>
        /// Genera hash PBKDF2 con formato: PBKDF2$iteraciones$saltBase64$hashBase64
        /// </summary>
        public static string GenerarHash(string password)
        {
            byte[] salt = new byte[TamanoSalt];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iteraciones))
            {
                hash = pbkdf2.GetBytes(TamanoHash);
            }

            return $"PBKDF2${Iteraciones}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Verifica si el password coincide con el hash almacenado
        /// </summary>
        public static bool VerificarPassword(string password, string hashAlmacenado)
        {
            // Primero intentar comparación directa (para passwords en texto plano)
            if (password == hashAlmacenado)
            {
                Console.WriteLine("[DEBUG PASSWORD] Password coincide directamente (texto plano)");
                return true;
            }

            try
            {
                // Parsear el formato: PBKDF2$iteraciones$saltBase64$hashBase64
                var partes = hashAlmacenado.Split('$');
                if (partes.Length != 4 || partes[0] != "PBKDF2")
                {
                    Console.WriteLine($"[DEBUG PASSWORD] No es formato PBKDF2, partes: {partes.Length}, primer parte: '{partes[0]}'");
                    return false;
                }

                int iteraciones = int.Parse(partes[1]);
                byte[] salt = Convert.FromBase64String(partes[2]);
                byte[] hashOriginal = Convert.FromBase64String(partes[3]);

                // Generar nuevo hash con el salt almacenado
                byte[] hashNuevo;
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iteraciones))
                {
                    hashNuevo = pbkdf2.GetBytes(TamanoHash);
                }

                // Comparar arrays de bytes
                bool coincide = hashNuevo.SequenceEqual(hashOriginal);
                Console.WriteLine($"[DEBUG PASSWORD] Comparación PBKDF2: {coincide}");
                return coincide;
            }
            catch (Exception ex)
            {
                // Si hay error en parsing, podría ser un password en claro (legacy)
                Console.WriteLine($"[DEBUG PASSWORD] Error en parsing PBKDF2: {ex.Message}, intentando comparación directa");
                return password == hashAlmacenado;
            }
        }

        /// <summary>
        /// Migra passwords en claro a formato hash
        /// </summary>
        public static void MigrarPasswords()
        {
            var repo = new ServicioUsuario();
            var usuarios = repo.ObtenerTodos();

            // Crear copia de la lista para evitar InvalidOperationException
            var usuariosCopia = new List<Usuario>(usuarios);
            bool huboCambios = false;

            foreach (var usuario in usuariosCopia)
            {
                // Si el password no tiene formato PBKDF2, es un password en claro
                if (!usuario.PasswordHash.StartsWith("PBKDF2$"))
                {
                    // Migrar a hash seguro
                    string nuevoHash = GenerarHash(usuario.PasswordHash);
                    usuario.PasswordHash = nuevoHash;
                    huboCambios = true;
                }
            }

            // Guardar cambios si hubo migración
            if (huboCambios)
            {
                repo.GuardarTodos(usuariosCopia);
            }
        }

        // Inicia sesión (devuelve true si ok)
        public static bool Login(string email, string password)
        {
            var repo = new ServicioUsuario();
            var usuario = repo.ObtenerPorCorreo(email);

            if (usuario != null)
            {
                // Debug temporal
                Console.WriteLine($"[DEBUG LOGIN] Usuario encontrado: {usuario.Email}, Rol: {usuario.Rol}");
                Console.WriteLine($"[DEBUG LOGIN] Password ingresado: '{password}'");
                Console.WriteLine($"[DEBUG LOGIN] Password almacenado: '{usuario.PasswordHash}'");

                // Verificar password usando el método seguro
                bool passwordValido = VerificarPassword(password, usuario.PasswordHash);
                Console.WriteLine($"[DEBUG LOGIN] Password válido: {passwordValido}");

                if (passwordValido)
                {
                    CurrentUser = usuario;
                    Console.WriteLine($"[DEBUG LOGIN] Login exitoso para {usuario.Email}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"[DEBUG LOGIN] Password incorrecto para {usuario.Email}");
                }
            }
            else
            {
                Console.WriteLine($"[DEBUG LOGIN] Usuario no encontrado: {email}");
            }
            return false;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

        /// <summary>
        /// Genera token de recuperación para el email especificado
        /// </summary>
        public static string GenerarTokenRecuperacion(string email)
        {
            var repo = new ServicioUsuario();
            var usuario = repo.ObtenerPorCorreo(email);
            
            if (usuario == null)
                return null;

            // Generar token aleatorio
            string token = GenerarTokenAleatorio();

            // Crear objeto token
            var resetToken = new PasswordResetToken(usuario.Id, email, token);

            // Guardar token en archivo usando escritura atómica
            var tokensExistentes = GestorJSON.LeerArchivo<List<PasswordResetToken>>("Data/ResetTokens.json") ?? new List<PasswordResetToken>();
            tokensExistentes.Add(resetToken);

            // Limpiar tokens expirados
            tokensExistentes.RemoveAll(t => t.HaExpirado());

            GestorJSON.EscribirAtomico("Data/ResetTokens.json", tokensExistentes);

            return token;
        }

        /// <summary>
        /// Resetea la contraseña usando el token proporcionado
        /// </summary>
        public static bool ResetPassword(string token, string nuevaPassword)
        {
            try
            {
                // Cargar tokens existentes
                var tokensExistentes = GestorJSON.LeerArchivo<List<PasswordResetToken>>("Data/ResetTokens.json") ?? new List<PasswordResetToken>();

                // Buscar token válido
                var resetToken = tokensExistentes.FirstOrDefault(t => t.Token == token);
                
                if (resetToken == null || !resetToken.EsValido())
                    return false;

                // Obtener usuario
                var repo = new ServicioUsuario();
                var usuario = repo.ObtenerPorCorreo(resetToken.Email);
                
                if (usuario == null)
                    return false;

                // Generar nuevo hash para la contraseña
                string nuevoHash = GenerarHash(nuevaPassword);
                usuario.PasswordHash = nuevoHash;

                // Actualizar usuario
                var usuarios = repo.ObtenerTodos();
                var usuarioEnLista = usuarios.FirstOrDefault(u => u.Id == usuario.Id);
                if (usuarioEnLista != null)
                {
                    usuarioEnLista.PasswordHash = nuevoHash;
                    repo.GuardarTodos(usuarios);
                }

                // Marcar token como usado y eliminarlo
                resetToken.MarcarComoUsado();
                tokensExistentes.Remove(resetToken);

                // Guardar tokens actualizados
                GestorJSON.EscribirAtomico("Data/ResetTokens.json", tokensExistentes);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
    