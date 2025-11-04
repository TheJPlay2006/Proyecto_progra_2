using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SistemaDeTickets.Controlador
{
    // Desde aquí se pueden agregar, buscar, actualizar o leer los eventos del archivo JSON.
    public class RepositorioEventos
    {
        // Ruta del archivo donde se guardan todos los eventos
        private const string RutaArchivo = @"Data/MisEventos.json";
        // Lista que almacena todos los eventos cargados desde el archivo
        private List<Modelo.Evento> _eventos;

        public RepositorioEventos()
        {
            // Intenta leer el archivo JSON con ayuda del GestorJSON.
            // Si no hay datos (devuelve null), crea una lista vacía para evitar errores.
            _eventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>(RutaArchivo) ?? new List<Modelo.Evento>();
        }

        public void Agregar(Modelo.Evento evento)
        {
            evento.Id = _eventos.Count > 0 ? _eventos.Max(e => e.Id) + 1 : 1;
            _eventos.Add(evento);
            // Guarda los cambios en el archivo de forma segura usando escritura atómica
            GestorJSON.EscribirAtomico(RutaArchivo, _eventos);
        }

        public Modelo.Evento BuscarPorId(int id)
        {
            // SIEMPRE leer del JSON para obtener datos frescos
            _eventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>(RutaArchivo) ?? new List<Modelo.Evento>();
            return _eventos.FirstOrDefault(e => e.Id == id);
        }

        public List<Modelo.Evento> ObtenerTodos()
        {
            // SIEMPRE leer del JSON para obtener datos frescos
            _eventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>(RutaArchivo) ?? new List<Modelo.Evento>();
            return _eventos;
        }

        public bool Actualizar(Modelo.Evento evento)
        {
            var old = BuscarPorId(evento.Id);
            if (old == null) return false;
            // Copia los nuevos datos en el evento encontrado
            old.Nombre = evento.Nombre;
            old.Fecha = evento.Fecha;
            old.Recinto = evento.Recinto;
            old.Tipo = evento.Tipo;
            old.Descripcion = evento.Descripcion;
            old.TiquetesDisponibles = evento.TiquetesDisponibles;
            old.Precio = evento.Precio;
            old.Estado = evento.Estado;

            GestorJSON.EscribirAtomico(RutaArchivo, _eventos);
            return true;// Devuelve true si la actualización fue exitosa
        }

        public bool ActualizarInventario(int eventoId, int nuevaCantidad)
        {
            var evento = BuscarPorId(eventoId);
            if (evento == null) return false;
            // Actualiza la cantidad de tiquetes disponibles
            evento.TiquetesDisponibles = nuevaCantidad;
            GestorJSON.EscribirAtomico(RutaArchivo, _eventos);
            return true;
        }
    }
}