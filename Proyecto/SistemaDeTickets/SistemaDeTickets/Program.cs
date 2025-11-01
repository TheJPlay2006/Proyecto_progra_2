using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaDeTickets.Services;
using SistemaDeTickets.Utils;

namespace SistemaDeTickets
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Inicialización básica de la aplicación
            InitializeApplication();

            // Crear administradores por defecto
            CrearAdministradoresPorDefecto();

            // Ejecutar migración automática de passwords al inicio de la aplicación
            try
            {
                ServicioAutenticacion.MigrarPasswords();
            }
            catch (Exception ex)
            {
                // Log error silenciosamente, no bloquear el inicio
                Console.WriteLine($"Error en migración automática: {ex.Message}");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // CREDENCIALES ADMIN DE PRUEBA:
            // Email: admin@jeticketscr.com
            // Contraseña: !Admin123Segura2025!

            Application.Run(new Vista.Inicio());
        }

        /// <summary>
        /// Inicialización básica - sin dependencias complejas
        /// </summary>
        private static void InitializeApplication()
        {
            // Aquí se pueden agregar inicializaciones futuras si es necesario
            Console.WriteLine("[INIT] Aplicación inicializada correctamente");
        }

        /// <summary>
        /// Crear administradores por defecto si no existen
        /// </summary>
        private static void CrearAdministradoresPorDefecto()
        {
            try
            {
                // Forzar creación directa en el archivo JSON
                string rutaArchivo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Usuarios.json");
                Console.WriteLine($"[INIT] Ruta del archivo JSON: {rutaArchivo}");

                // Leer usuarios existentes
                var usuarios = GestorJSON.LeerArchivo<List<Modelo.Usuario>>(rutaArchivo) ?? new List<Modelo.Usuario>();
                Console.WriteLine($"[INIT] Usuarios existentes antes: {usuarios.Count}");

                // Administradores requeridos - forzar creación
                var adminsRequeridos = new[]
                {
                    ("admin@jeticketscr.com", "!Admin123Segura2025!", "JE Tickets CR Admin"),
                    ("jairo@jeticketscr.com", "Jairo1234Clave2025!", "Jairo Admin"),
                    ("emesis@jeticketscr.com", "Emesis1234Seguro2025!", "Emesis Admin")
                };

                foreach (var (email, password, nombre) in adminsRequeridos)
                {
                    // Buscar si ya existe (ignorar case)
                    var adminExistente = usuarios.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                    if (adminExistente != null)
                    {
                        // Si existe, asegurar que tenga rol de admin y contraseña correcta
                        adminExistente.Rol = Modelo.RolUsuario.Admin;
                        adminExistente.PasswordHash = password;
                        Console.WriteLine($"[INIT] Administrador actualizado: {email}");
                    }
                    else
                    {
                        // Crear nuevo administrador
                        var nuevoAdmin = new Modelo.Usuario
                        {
                            Id = usuarios.Count > 0 ? usuarios.Max(u => u.Id) + 1 : 1,
                            Nombre = nombre,
                            Email = email,
                            PasswordHash = password, // Contraseña en texto plano para pruebas
                            Rol = Modelo.RolUsuario.Admin,
                            EventosSeguidos = new List<int>(),
                            FechaRegistro = DateTime.Now,
                            TokenRecuperacion = null,
                            TokenExpiracion = null
                        };

                        usuarios.Add(nuevoAdmin);
                        Console.WriteLine($"[INIT] Administrador creado: {email} (ID: {nuevoAdmin.Id})");
                    }
                }

                // Forzar guardado directo al archivo
                GestorJSON.EscribirAtomico(rutaArchivo, usuarios);
                Console.WriteLine($"[INIT] Archivo guardado en: {rutaArchivo}");

                // Verificar que se guardaron leyendo de nuevo
                var usuariosVerificados = GestorJSON.LeerArchivo<List<Modelo.Usuario>>(rutaArchivo) ?? new List<Modelo.Usuario>();
                var adminsVerificados = usuariosVerificados.Where(u => u.Rol == Modelo.RolUsuario.Admin).ToList();

                Console.WriteLine($"[INIT] Total usuarios después: {usuariosVerificados.Count}");
                Console.WriteLine($"[INIT] Administradores encontrados: {adminsVerificados.Count}");

                foreach (var admin in adminsVerificados)
                {
                    Console.WriteLine($"[INIT] Admin verificado: {admin.Email} (ID: {admin.Id}, Rol: {admin.Rol})");
                }

                Console.WriteLine("[INIT] Verificación de administradores completada");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[INIT ERROR] Error al crear administradores: {ex.Message}");
                Console.WriteLine($"[INIT ERROR] StackTrace: {ex.StackTrace}");
            }
        }
    }
}
