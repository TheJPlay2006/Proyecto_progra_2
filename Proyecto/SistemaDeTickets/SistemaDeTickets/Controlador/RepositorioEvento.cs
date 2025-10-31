using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SistemaDeTickets.Controlador
{
    public class RepositorioEventos
    {
        private const string RutaArchivo = @"Data/MisEventos.json";
        private List<Modelo.Evento> _eventos;

        public RepositorioEventos()
        {
            _eventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>(RutaArchivo) ?? new List<Modelo.Evento>();
        }

        public void Agregar(Modelo.Evento evento)
        {
            evento.Id = _eventos.Count > 0 ? _eventos.Max(e => e.Id) + 1 : 1;
            _eventos.Add(evento);
            GestorJSON.EscribirAtomico(RutaArchivo, _eventos);
        }

        public Modelo.Evento BuscarPorId(int id)
        {
            return _eventos.FirstOrDefault(e => e.Id == id);
        }

        public List<Modelo.Evento> ObtenerTodos()
        {
            return _eventos;
        }

        public bool Actualizar(Modelo.Evento evento)
        {
            var old = BuscarPorId(evento.Id);
            if (old == null) return false;

            old.Nombre = evento.Nombre;
            old.Fecha = evento.Fecha;
            old.Recinto = evento.Recinto;
            old.Tipo = evento.Tipo;
            old.Descripcion = evento.Descripcion;
            old.TiquetesDisponibles = evento.TiquetesDisponibles;
            old.Precio = evento.Precio;
            old.Estado = evento.Estado;

            GestorJSON.EscribirAtomico(RutaArchivo, _eventos);
            return true;
        }

        public bool ActualizarInventario(int eventoId, int nuevaCantidad)
        {
            var evento = BuscarPorId(eventoId);
            if (evento == null) return false;

            evento.TiquetesDisponibles = nuevaCantidad;
            GestorJSON.EscribirAtomico(RutaArchivo, _eventos);
            return true;
        }
    }
}