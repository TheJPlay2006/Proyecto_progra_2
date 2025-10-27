namespace SistemaDeTickets.Controlador.Patrones
{
    public class GestorEventos : SujetoObservable
    {
        public void NotificarNuevoEvento(Modelo.Evento evento)
        {
            NotificarObservadores(Modelo.TipoNotificacion.NuevoEvento, evento);
        }

        public void NotificarBajoInventario(int eventoId, int cantidadRestante)
        {
            var datos = new { EventoId = eventoId, CantidadRestante = cantidadRestante };
            NotificarObservadores(Modelo.TipoNotificacion.BajoInventario, datos);
        }
    }
}
