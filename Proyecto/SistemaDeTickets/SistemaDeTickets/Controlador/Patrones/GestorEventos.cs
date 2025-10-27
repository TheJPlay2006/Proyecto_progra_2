namespace SistemaDeTickets.Controlador.Patrones
{
    public class GestorEventos : SujetoObservable
    {
        public void NotificarNuevoEvento(Evento evento)
        {
            NotificarObservadores(TipoNotificacion.NuevoEvento, evento);
        }

        public void NotificarBajoInventario(int eventoId, int cantidadRestante)
        {
            var datos = new { EventoId = eventoId, CantidadRestante = cantidadRestante };
            NotificarObservadores(TipoNotificacion.BajoInventario, datos);
        }
    }
}
