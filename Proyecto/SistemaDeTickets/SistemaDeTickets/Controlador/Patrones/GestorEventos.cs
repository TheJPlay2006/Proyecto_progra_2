using SistemaDeTickets.Controlador;
using SistemaDeTickets.Modelo;
using System;

namespace SistemaDeTickets.Controlador.Patrones
{
    public class GestorEventos : SujetoObservable
    {

        public GestorEventos()
        {
            // Inicializar gestor de notificaciones cuando sea necesario
        }

        public new void NotificarNuevoEvento(SistemaDeTickets.Modelo.Evento evento)
        {
            // Notificar a través del patrón Observer
            NotificarObservadores(SistemaDeTickets.Modelo.TipoNotificacion.NuevoEvento, evento);

            // Notificar en el área de sistema de Windows
            // TODO: Implementar notificación visual cuando se resuelva el problema de referencia
            Console.WriteLine($"[NOTIFICACIÓN] ¡Nuevo evento disponible: {evento.Nombre}!");
        }

        public void NotificarBajoInventario(int eventoId, int cantidadRestante, string nombreEvento)
        {
            var datos = new { EventoId = eventoId, CantidadRestante = cantidadRestante };
            NotificarObservadores(SistemaDeTickets.Modelo.TipoNotificacion.BajoInventario, datos);

            // Notificar en el área de sistema de Windows
            // TODO: Implementar notificación visual cuando se resuelva el problema de referencia
            Console.WriteLine($"[NOTIFICACIÓN] ¡Quedan pocos tiques para {nombreEvento}! Solo {cantidadRestante} disponibles.");
        }

        public void NotificarCompraExitosa(string nombreEvento, int cantidad)
        {
            // Notificar en el área de sistema de Windows
            // TODO: Implementar notificación visual cuando se resuelva el problema de referencia
            Console.WriteLine($"[NOTIFICACIÓN] ¡Compra realizada exitosamente! {cantidad} ticket(s) para {nombreEvento}");
        }

        public void NotificarError(string mensaje)
        {
            // TODO: Implementar notificación visual cuando se resuelva el problema de referencia
            Console.WriteLine($"[ERROR] {mensaje}");
        }

        public void NotificarAdvertencia(string mensaje)
        {
            // TODO: Implementar notificación visual cuando se resuelva el problema de referencia
            Console.WriteLine($"[ADVERTENCIA] {mensaje}");
        }
    }
}
