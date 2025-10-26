using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeTickets.Modelo
{
    public class Compra
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int EventoId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaCompra { get; set; }
        public double Total { get; set; }
        

        public Compra() { }
        public Compra(int id, int usuarioId, int eventoId, int cantidad, DateTime fechaCompra,double total)
        {
            Id = id;
            UsuarioId = usuarioId;
            EventoId = eventoId;
            Cantidad = cantidad;
            FechaCompra = fechaCompra;
            Total = total;
        }
    }
}
