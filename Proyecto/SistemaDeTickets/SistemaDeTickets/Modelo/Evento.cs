using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeTickets.Modelo
{
    /// <summary>
    /// Modelo que representa un evento disponible para venta de tickets.
    /// Incluye información del evento, precios y disponibilidad.
    /// </summary>
    public class Evento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Recinto { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public int TiquetesDisponibles { get; set; }
        public double Precio { get; set; }
        public EstadoEvento Estado { get; set; }

        public Evento() { }
        public Evento(int id, string nombre, DateTime fecha, string recinto, string tipo, string descripcion, int tiquetesDisponibles, double precio, EstadoEvento estado = EstadoEvento.Activo)
        {
            Id = id;
            Nombre = nombre;
            Fecha = fecha;
            Recinto = recinto;
            Tipo = tipo;
            Descripcion = descripcion;
            TiquetesDisponibles = tiquetesDisponibles;
            Precio = precio;
            Estado = estado;
        }
    }
}
