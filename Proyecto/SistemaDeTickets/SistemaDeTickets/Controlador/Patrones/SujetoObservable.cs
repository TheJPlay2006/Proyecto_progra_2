using System;
using System.Collections.Generic;
using SistemaDeTickets.Modelo;

namespace SistemaDeTickets.Controlador.Patrones
{
    public abstract class SujetoObservable
    {
        private readonly List<IObservador> observadores = new List<IObservador>();

        public void AgregarObservador(IObservador observador)
        {
            if (!observadores.Contains(observador))
                observadores.Add(observador);
        }

        public void EliminarObservador(IObservador observador)
        {
            if (observadores.Contains(observador))
                observadores.Remove(observador);
        }

        protected void NotificarObservadores(TipoNotificacion tipo, object datos)
        {
            // Crear copia de la lista para evitar InvalidOperationException si se modifica durante notificación
            var observadoresCopia = new List<IObservador>(observadores);

            foreach (var obs in observadoresCopia)
            {
                try
                {
                    obs.Actualizar(tipo, datos);
                }
                catch (Exception ex)
                {
                    // Log error silenciosamente para evitar que un observador roto detenga a los demás
                    Console.WriteLine($"Error notificando observador: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Notifica a todos los observadores sobre un nuevo evento
        /// </summary>
        public void NotificarNuevoEvento(SistemaDeTickets.Modelo.Evento evento)
        {
            NotificarObservadores(TipoNotificacion.NuevoEvento, evento);
        }

        /// <summary>
        /// Notifica cuando el stock está bajo (umbral = 10)
        /// </summary>
        public void NotificarStockBajo(string eventoId, int cantidadRestante)
        {
            // Crear objeto anónimo con la información del evento
            var infoStock = new { EventoId = eventoId, CantidadRestante = cantidadRestante };
            NotificarObservadores(TipoNotificacion.BajoInventario, infoStock);
        }

        /// <summary>
        /// Notifica cuando hay stock bajo basándose en umbral (10 tickets)
        /// </summary>
        public void VerificarYNotificarStockBajo(int eventoId, int cantidadRestante)
        {
            const int UmbralStockBajo = 10;
            
            if (cantidadRestante <= UmbralStockBajo)
            {
                NotificarStockBajo(eventoId.ToString(), cantidadRestante);
            }
        }
    }
}
