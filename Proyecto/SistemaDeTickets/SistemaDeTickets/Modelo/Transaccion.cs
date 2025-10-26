using System;

namespace SistemaDeTickets.Modelo
{
    public class Transaccion
    {
        public int Id { get; set; }
        public Compra CompraAsociada { get; set; }
        public string Estado { get; set; }
        public DateTime Timestamp { get; set; }

        public void Iniciar()
        {
            Estado = "Iniciada";
            Timestamp = DateTime.Now;
        }

        public void Commit()
        {
            Estado = "Completada";
        }

        public void Rollback()
        {
            Estado = "Revertida";
        }
    }
}
