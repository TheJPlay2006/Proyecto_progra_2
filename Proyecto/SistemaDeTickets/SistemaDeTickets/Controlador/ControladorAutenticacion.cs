using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using System;
using System.Collections.Generic;

namespace SistemaDeTickets.Controlador
{
    public class ControladorAutenticacion
    {
        private readonly RepositorioUsuarios _repositorio;

        public ControladorAutenticacion()
        {
            _repositorio = new RepositorioUsuarios();
        }

        public Usuario Registrar(string nombre, string email, string password, RolUsuario rol)
        {
            if (!_repositorio.ValidarUnicidad(email))
                throw new Exception("Ya existe un usuario con ese correo.");

            var usuario = new Usuario
            {
                Nombre = nombre,
                Email = email,
                PasswordHash = HashContrasena.CrearHash(password),
                Rol = rol,
                EventosSeguidos = new List<int>(),
                FechaRegistro = DateTime.Now
            };
            _repositorio.Agregar(usuario);
            return usuario;
        }

        public Usuario IniciarSesion(string email, string password)
        {
            var usuario = _repositorio.BuscarPorEmail(email);
            if (usuario == null || !HashContrasena.VerificarHash(password, usuario.PasswordHash))
                throw new Exception("Correo o contraseña incorrectos.");
            return usuario;
        }

        public string GenerarTokenRecuperacion(string email)
        {
            var usuario = _repositorio.BuscarPorEmail(email);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");
            usuario.TokenRecuperacion = Guid.NewGuid().ToString();
            usuario.TokenExpiracion = DateTime.Now.AddMinutes(30);
            _repositorio.Actualizar(usuario);
            return usuario.TokenRecuperacion;
        }

        public bool ValidarTokenRecuperacion(string token)
        {
            var usuario = _repositorio.BuscarPorToken(token);
            return usuario != null && usuario.TokenExpiracion > DateTime.Now;
        }

        public bool ResetPassword(string token, string nuevoPassword)
        {
            var usuario = _repositorio.BuscarPorToken(token);
            if (usuario == null || usuario.TokenExpiracion < DateTime.Now)
                return false;

            usuario.CambiarPassword(nuevoPassword);
            usuario.TokenRecuperacion = null;
            usuario.TokenExpiracion = null;
            _repositorio.Actualizar(usuario);
            return true;
        }
    }
}
