using SistemaDeTickets.Modelo;
using System.Collections.Generic;

namespace SistemaDeTickets.Controlador
{
    public class GestorInventario
    {
        private readonly object _bloqueo = new object();
        private readonly Dictionary<int, Inventario> _inventarios;

        public GestorInventario()
        {
            _inventarios = new Dictionary<int, Inventario>();
        }

        public bool BloquearInventario(int eventoId, int cantidad)
        {
            lock (_bloqueo)
            {
                if (_inventarios.ContainsKey(eventoId) && _inventarios[eventoId].VerificarDisponibilidad(cantidad))
                {
                    _inventarios[eventoId].ReservarTiques(cantidad);
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
        }

        public int ObtenerDisponibilidad(int eventoId)
        {
            return _inventarios.ContainsKey(eventoId) ? _inventarios[eventoId].TiquesDisponibles : 0;
        }
    }
}
