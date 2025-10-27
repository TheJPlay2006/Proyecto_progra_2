using SistemaDeTickets.Modelo;
using SistemaDeTickets.Controlador.Patrones;
using System;

namespace SistemaDeTickets.Controlador.Patrones
{
    /// <summary>
    /// Fachada que simplifica el proceso de compra de tickets.
    /// Implementa el patrón Facade para coordinar validaciones, pagos y confirmaciones.
    /// Centraliza la lógica de compra en una interfaz unificada.
    /// </summary>
    public class FachadaCompraTique
    {
        private readonly ControladorUsuario _usuarioControlador;
        private readonly ControladorCompra _compraControlador;
        private readonly GestorInventario _gestorInventario;
        private readonly GestorEventos _gestorEventos;

        public FachadaCompraTique(ControladorUsuario usuarioControlador, ControladorCompra compraControlador,
                                  GestorInventario gestorInventario, GestorEventos gestorEventos)
        {
            _usuarioControlador = usuarioControlador;
            _compraControlador = compraControlador;
            _gestorInventario = gestorInventario;
            _gestorEventos = gestorEventos;
        }

        public DetalleCompra IniciarProcesoCompra(int usuarioId, int eventoId, int cantidad)
        {
            if (!_gestorInventario.BloquearInventario(eventoId, cantidad))
                throw new Exception("No hay suficiente inventario disponible.");

            return _compraControlador.IniciarCompra(usuarioId, eventoId, cantidad);
        }

        public bool ValidarDisponibilidad(int eventoId, int cantidad)
        {
            return _gestorInventario.ObtenerDisponibilidad(eventoId) >= cantidad;
        }

        public bool ProcesarPago(string numeroTarjeta, string cvv, DetalleCompra detalle)
        {
            return _compraControlador.ProcesarPago(detalle, numeroTarjeta, cvv);
        }

        public Compra ConfirmarCompra(DetalleCompra detalle)
        {
            var compra = _compraControlador.ConfirmarCompra(detalle);
            return compra;
        }

        public void CancelarCompra(int compraId)
        {
            _compraControlador.CancelarCompra(compraId);
        }

        public byte[] GenerarReciboPDF(int compraId)
        {
            return _compraControlador.GenerarReciboPDF(compraId);
        }
    }
}
