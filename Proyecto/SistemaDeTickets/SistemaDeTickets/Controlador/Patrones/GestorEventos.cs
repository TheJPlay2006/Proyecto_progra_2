namespace SistemaDeTickets.Controlador.Patrones
{
    public class GestorEventos : SujetoObservable
    {
        public new void NotificarNuevoEvento(SistemaDeTickets.Modelo.Evento evento)
        {
            NotificarObservadores(SistemaDeTickets.Modelo.TipoNotificacion.NuevoEvento, evento);
        }

        public void NotificarBajoInventario(int eventoId, int cantidadRestante)
        {
            var datos = new { EventoId = eventoId, CantidadRestante = cantidadRestante };
            NotificarObservadores(SistemaDeTickets.Modelo.TipoNotificacion.BajoInventario, datos);
        }
    }
}
