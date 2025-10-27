using SistemaDeTickets.Modelo;

namespace SistemaDeTickets.Controlador.Patrones
{
    public interface IObservador
    {
        void Actualizar(TipoNotificacion tipo, object datos);
    }
}
