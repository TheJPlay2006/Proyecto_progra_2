using System;
using System.Collections.Generic;

namespace SistemaDeTickets.Modelo
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public RolUsuario Rol { get; set; }
        public List<int> EventosSeguidos { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string TokenRecuperacion { get; set; }
        public DateTime? TokenExpiracion { get; set; }

        public bool ValidarCredenciales(string passwordIngresado)
        {
            return BCrypt.Net.BCrypt.Verify(passwordIngresado, PasswordHash);
        }

        public void CambiarPassword(string nuevoPassword)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevoPassword);
        }

        public void SeguirEvento(int eventoId)
        {
            if (!EventosSeguidos.Contains(eventoId))
                EventosSeguidos.Add(eventoId);
        }

        public void DejarDeSeguirEvento(int eventoId)
        {
            if (EventosSeguidos.Contains(eventoId))
                EventosSeguidos.Remove(eventoId);
        }
    }
}
