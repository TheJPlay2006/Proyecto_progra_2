using System;

namespace SistemaDeTickets.Modelo
{
    public class DetalleCompra
    {
        public Evento Evento { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        public decimal CalcularSubtotal()
        {
            Subtotal = Evento.Precio * Cantidad;
            Total = Subtotal;
            return Subtotal;
        }
    }
}
