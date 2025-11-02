using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaDeTickets.Controlador
{
    public class GestorInventario
    {
        private readonly object _bloqueo = new object();

        public GestorInventario()
        {
            // Constructor vacío - ahora lee directamente del JSON
        }

        public bool BloquearInventario(int eventoId, int cantidad)
        {
            lock (_bloqueo)
            {
                // SIEMPRE leer del JSON para obtener datos frescos
                var eventos = GestorJSON.LeerArchivo<List<SistemaDeTickets.Modelo.Evento>>("Data/MisEventos.json") ?? new List<SistemaDeTickets.Modelo.Evento>();
                var evento = eventos.FirstOrDefault(e => e.Id == eventoId);

                if (evento != null && evento.TiquetesDisponibles >= cantidad)
                {
                    // Descontar stock directamente en el JSON
                    evento.TiquetesDisponibles -= cantidad;
                    GestorJSON.EscribirAtomico("Data/MisEventos.json", eventos);

                    return true;
                }

                return false;
            }
        }

        public bool ActualizarInventarioAtomico(int eventoId, int cantidad)
        {
            return BloquearInventario(eventoId, cantidad);
        }

        public void LiberarBloqueo(int eventoId)
        {
            // Método obsoleto - el bloqueo se maneja automáticamente
        }

        public int ObtenerDisponibilidad(int eventoId)
        {
            // SIEMPRE leer del JSON para obtener datos frescos
            var eventos = GestorJSON.LeerArchivo<List<SistemaDeTickets.Modelo.Evento>>("Data/MisEventos.json") ?? new List<SistemaDeTickets.Modelo.Evento>();
            var evento = eventos.FirstOrDefault(e => e.Id == eventoId);

            int disponibilidad = evento?.TiquetesDisponibles ?? 0;
            return disponibilidad;
        }
    }
}
