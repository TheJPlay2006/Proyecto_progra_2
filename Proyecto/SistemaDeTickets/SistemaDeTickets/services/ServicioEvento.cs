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
    //Aquí se leen los eventos guardados y también se pueden filtrar según diferentes criterios.

    public class ServicioEvento
    {
        // Ruta donde está guardado el archivo JSON con los eventos
        private string path = "Data/MisEventos.json";

        public List<Modelo.Evento> ObtenerEventos()
        {
            // Si el archivo no existe, devuelve una lista vacía (evita errores)
            if (!File.Exists(path))
                return new List<Modelo.Evento>();

            string json = File.ReadAllText(path);
            // Muestra información en consola (útil para revisar si el archivo se está leyendo bien)
            Console.WriteLine($"Archivo JSON encontrado: {File.Exists(path)}");
            Console.WriteLine($"Ruta actual: {Path.GetFullPath(path)}");
            Console.WriteLine($"Contenido JSON:\n{json}");
            // Convierte el texto JSON en una lista de objetos de tipo Evento
            return JsonConvert.DeserializeObject<List<Modelo.Evento>>(json);
        }

        public List<Modelo.Evento> FiltrarEventos(string nombre, string tipo, string recinto)
        {
            var eventos = ObtenerEventos();
            // busca los eventos cuyo nombre contenga ese texto 
            if (!string.IsNullOrEmpty(nombre))
                eventos = eventos.FindAll(e => e.Nombre.ToLower().Contains(nombre.ToLower()));
            // Si se especificó un tipo, filtra solo los eventos de ese tipo
            if (!string.IsNullOrEmpty(tipo))
                eventos = eventos.FindAll(e => e.Tipo.ToLower().Contains(tipo.ToLower()));
            // Si se especificó un recinto, filtra solo los eventos que se realicen en ese lugar
            if (!string.IsNullOrEmpty(recinto))
                eventos = eventos.FindAll(e => e.Recinto.ToLower().Contains(recinto.ToLower()));
            // Retorna la lista filtrada (o todos los eventos si no se aplicaron filtros)
            return eventos;
        }
    }
}
