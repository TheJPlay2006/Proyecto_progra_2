using SistemaDeTickets.Modelo;
using System.Collections.Generic;

namespace SistemaDeTickets.Controlador
{
    public class ControladorUsuario
    {
        private readonly RepositorioUsuarios _repositorio;

        public ControladorUsuario()
        {
            _repositorio = new RepositorioUsuarios();
        }

        public bool RegistrarUsuario(Usuario usuario)
        {
            if (!_repositorio.ValidarUnicidad(usuario.Email))
                return false;
            _repositorio.Agregar(usuario);
            return true;
        }

        public Usuario Login(string email, string password)
        {
            var usuario = _repositorio.BuscarPorEmail(email);
            if (usuario == null || !usuario.ValidarCredenciales(password))
                return null;
            return usuario;
        }

        public void Logout()
        {
            // Implementa la lógica de cierre de sesión (usualmente limpiar sesión actual)
        }

        public bool SolicitarRecuperacionPassword(string email)
        {
            // Reutiliza la lógica de ControladorAutenticacion si lo necesitas
            return true;
        }

        public bool ResetPassword(string token, string password)
        {
            // Reutiliza la lógica de ControladorAutenticacion si lo necesitas
            return true;
        }

        public Usuario ObtenerPerfil(int usuarioId)
        {
            return _repositorio.BuscarPorId(usuarioId);
        }
    }
}
