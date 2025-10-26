using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using System;
using System.Collections.Generic;

namespace SistemaDeTickets.Controlador
{
    public class ControladorCompra
    {
        private readonly RepositorioCompras _repositorioCompras;

        public ControladorCompra()
        {
            _repositorioCompras = new RepositorioCompras();
        }

        public DetalleCompra IniciarCompra(int usuarioId, int eventoId, int cantidad)
        {
            var detalle = new DetalleCompra
            {
                Evento = null, 
                Cantidad = cantidad
            };
            detalle.CalcularSubtotal();
            return detalle;
        }

        public bool ValidarCompra(DetalleCompra detalle)
        {
            return detalle.Cantidad > 0 && detalle.Evento != null && detalle.Evento.TiquesDisponibles >= detalle.Cantidad;
        }

        public bool ProcesarPago(DetalleCompra detalle, string numeroTarjeta, string cvv)
        {
            return ValidadorTarjeta.ValidarNumero(numeroTarjeta) && ValidadorTarjeta.ValidarCVV(cvv);
        }

        public Compra ConfirmarCompra(DetalleCompra detalle)
        {
            Compra compra = new Compra
            {
                UsuarioId = 0, // Setea el usuario real aquí
                EventoId = detalle.Evento.Id,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.Evento.Precio,
                PrecioTotal = detalle.Total,
                FechaCompra = DateTime.Now,
                Estado = EstadoCompra.Completada,
                MetodoPago = MetodoPago.TarjetaCredito
            };
            _repositorioCompras.Agregar(compra);
            return compra;
        }

        public void CancelarCompra(int compraId)
        {
            _repositorioCompras.Eliminar(compraId);
        }

        public List<Compra> ObtenerHistorial(int usuarioId)
        {
            return _repositorioCompras.ComprasPorUsuario(usuarioId);
        }

        public byte[] GenerarReciboPDF(int compraId)
        {
            var compra = _repositorioCompras.BuscarPorId(compraId);

            return GeneradorPDF.GenerarRecibo(compra, null, null);
        }
    }
}
