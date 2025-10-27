using SistemaDeTickets.Modelo;

namespace SistemaDeTickets.Controlador.Patrones
{
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
