using SistemaDeTickets.Modelo;
using System;
using System.Collections.Generic;

namespace SistemaDeTickets.Controlador.Patrones
{
    /// <summary>
    /// Fábrica para crear instancias de Usuario.
    /// Implementa el patrón Factory para centralizar la creación de usuarios con validaciones.
    /// Maneja la creación de usuarios regulares y administradores.
    /// </summary>
    public class FabricaUsuarios
    {
        public Usuario CrearUsuario(string nombre, string email, string password)
        {
            return new Usuario
            {
                Nombre = nombre,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Rol = RolUsuario.Usuario,
                EventosSeguidos = new List<int>(),
                FechaRegistro = DateTime.Now
            };
        }

        public Usuario CrearAdministrador(string nombre, string email, string password)
        {
            return new Usuario
            {
                Nombre = nombre,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Rol = RolUsuario.Administrador,
                EventosSeguidos = new List<int>(),
                FechaRegistro = DateTime.Now
            };
        }
    }
}
