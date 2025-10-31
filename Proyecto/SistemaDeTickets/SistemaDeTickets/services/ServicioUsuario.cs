using Newtonsoft.Json;
using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeTickets.Services
{
    internal class ServicioUsuario
    {
        private string path = "Data/Usuarios.json";

        public ServicioUsuario()
        {
            // Llamar a migración automática al crear el servicio
            ServicioAutenticacion.MigrarPasswords();
        }

        public List<Usuario> ObtenerTodos()
        {
            return GestorJSON.LeerArchivo<List<Usuario>>(path) ?? new List<Usuario>();
        }

        public Usuario ObtenerPorCorreo(string email)
        {
            return ObtenerTodos().FirstOrDefault(u => u.Email.Equals(email, System.StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Guarda todos los usuarios usando escritura atómica
        /// </summary>
        public void GuardarTodos(List<Usuario> usuarios)
        {
            GestorJSON.EscribirAtomico(path, usuarios);
        }
    }
}
