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
    public class ServicioEvento
    {
        private string path = "Data/MisEventos.json";

        public List<Modelo.Evento> ObtenerEventos()
        {
            if (!File.Exists(path))
                return new List<Modelo.Evento>();

            string json = File.ReadAllText(path);
            Console.WriteLine($"Archivo JSON encontrado: {File.Exists(path)}");
            Console.WriteLine($"Ruta actual: {Path.GetFullPath(path)}");
            Console.WriteLine($"Contenido JSON:\n{json}");
            return JsonConvert.DeserializeObject<List<Modelo.Evento>>(json);
        }

        public List<Modelo.Evento> FiltrarEventos(string nombre, string tipo, string recinto)
        {
            var eventos = ObtenerEventos();
            if (!string.IsNullOrEmpty(nombre))
                eventos = eventos.FindAll(e => e.Nombre.ToLower().Contains(nombre.ToLower()));
            if (!string.IsNullOrEmpty(tipo))
                eventos = eventos.FindAll(e => e.Tipo.ToLower().Contains(tipo.ToLower()));
            if (!string.IsNullOrEmpty(recinto))
                eventos = eventos.FindAll(e => e.Recinto.ToLower().Contains(recinto.ToLower()));
            return eventos;
        }
    }
}
