using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SistemaDeTickets.Controlador
{
    public class RepositorioUsuarios
    {
        private const string RutaArchivo = @"Data/Usuarios.json";
        private List<Usuario> _usuarios;

        public RepositorioUsuarios()
        {
            _usuarios = GestorJSON.LeerArchivo<List<Usuario>>(RutaArchivo) ?? new List<Usuario>();
        }

        public void Agregar(Usuario usuario)
        {
            usuario.Id = _usuarios.Count > 0 ? _usuarios.Max(u => u.Id) + 1 : 1;
            _usuarios.Add(usuario);
            GestorJSON.EscribirAtomico(RutaArchivo, _usuarios);
        }

        public Usuario BuscarPorEmail(string email)
        {
            return _usuarios.FirstOrDefault(u => u.Email == email);
        }

        public Usuario BuscarPorId(int id)
        {
            return _usuarios.FirstOrDefault(u => u.Id == id);
        }

        public Usuario BuscarPorToken(string token)
        {
            return _usuarios.FirstOrDefault(u => u.TokenRecuperacion == token);
        }

        public bool Actualizar(Usuario usuario)
        {
            var old = BuscarPorId(usuario.Id);
            if (old == null) return false;
            old.Nombre = usuario.Nombre;
            old.PasswordHash = usuario.PasswordHash;
            old.Rol = usuario.Rol;
            old.EventosSeguidos = usuario.EventosSeguidos;
            old.TokenRecuperacion = usuario.TokenRecuperacion;
            old.TokenExpiracion = usuario.TokenExpiracion;
            GestorJSON.EscribirAtomico(RutaArchivo, _usuarios);
            return true;
        }

        public bool ValidarUnicidad(string email)
        {
            return _usuarios.All(u => u.Email != email);
        }
    }
}
