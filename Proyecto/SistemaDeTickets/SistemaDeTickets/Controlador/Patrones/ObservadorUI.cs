using System.Windows.Forms;

namespace SistemaDeTickets.Controlador.Patrones
{
    public class ObservadorUI : IObservador
    {
        public void Actualizar(TipoNotificacion tipo, object datos)
        {
            switch (tipo)
            {
                case TipoNotificacion.NuevoEvento:
                    MostrarNotificacionNuevoEvento((Evento)datos);
                    break;
                case TipoNotificacion.BajoInventario:
                    var info = (dynamic)datos;
                    MostrarAlertaBajoInventario((int)info.EventoId, (int)info.CantidadRestante);
                    break;
                case TipoNotificacion.CompraExitosa:
                    break;
            }
        }

        public void MostrarNotificacionNuevoEvento(Evento evento)
        {
            MessageBox.Show($"Nuevo evento creado: {evento.Nombre} ({evento.Fecha:dd/MM/yyyy})", "Nuevo Evento", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MostrarAlertaBajoInventario(int eventoId, int cantidadRestante)
        {
            MessageBox.Show($"¡Alerta! Quedan solo {cantidadRestante} tiques para el evento #{eventoId}.", "Bajo Inventario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
