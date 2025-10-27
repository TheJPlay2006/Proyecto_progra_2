using SistemaDeTickets.Modelo;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SistemaDeTickets.Controlador
{
    public class GestorConcurrencia
    {
        private static readonly ConcurrentDictionary<int, object> locks = new ConcurrentDictionary<int, object>();

        public object CrearLock(int eventoId)
        {
            return locks.GetOrAdd(eventoId, new object());
        }

        public void AdquirirLock(int eventoId)
        {
            lock (CrearLock(eventoId))
            {
            }
        }

        public void LiberarLock(int eventoId)
        {
        }

        public Task<bool> ProcesarCompraAsync(Compra compra)
        {
            return Task.Run(() =>
            {
                AdquirirLock(compra.EventoId);
                LiberarLock(compra.EventoId);
                return true;
            });
        }
    }
}
