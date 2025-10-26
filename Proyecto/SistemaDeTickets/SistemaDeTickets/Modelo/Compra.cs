using System;

namespace SistemaDeTickets.Modelo
{
    public class Compra
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int EventoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
        public DateTime FechaCompra { get; set; }
        public EstadoCompra Estado { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public string UltimosDigitosTarjeta { get; set; }

        public decimal CalcularTotal()
        {
            PrecioTotal = PrecioUnitario * Cantidad;
            return PrecioTotal;
        }

        public bool Validar()
        {
            return Cantidad > 0 && PrecioUnitario > 0 && UsuarioId > 0 && EventoId > 0;
        }
    }
}
