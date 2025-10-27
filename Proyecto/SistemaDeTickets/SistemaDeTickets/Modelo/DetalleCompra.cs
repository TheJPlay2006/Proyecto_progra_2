using System;

namespace SistemaDeTickets.Modelo
{
    public class DetalleCompra
    {
        public Modelo.Evento Evento { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        public decimal CalcularSubtotal()
        {
            Subtotal = (decimal)Evento.Precio * Cantidad;
            Total = Subtotal;
            return Subtotal;
        }
    }
}
