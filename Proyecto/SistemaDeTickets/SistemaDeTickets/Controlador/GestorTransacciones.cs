using SistemaDeTickets.Modelo;

namespace SistemaDeTickets.Controlador
{
    public class GestorTransacciones
    {
        public Transaccion IniciarTransaccion(Compra compra)
        {
            var transaccion = new Transaccion
            {
                CompraAsociada = compra
            };
            transaccion.Iniciar();
            return transaccion;
        }

        public bool CommitTransaccion(Transaccion transaccion)
        {
            transaccion.Commit();
            return true;
        }

        public void RollbackTransaccion(Transaccion transaccion)
        {
            transaccion.Rollback();
        }
    }
}
