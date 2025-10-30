using Newtonsoft.Json;
using SistemaDeTickets.Modelo;
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
        private string path = "Data/Usuario.json";

        public List<Usuario> ObtenerTodos()
        {
            if (!File.Exists(path)) return new List<Usuario>();
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();
        }

        public Usuario ObtenerPorCorreo(string email)
        {
            return ObtenerTodos().FirstOrDefault(u => u.Email.Equals(email, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
