using SistemaDeTickets.Modelo;
using System.Collections.Generic;

namespace SistemaDeTickets.Controlador
{
    public class ControladorHistorial
    {
        private readonly RepositorioCompras _repositorioCompras;

        public ControladorHistorial()
        {
            _repositorioCompras = new RepositorioCompras();
        }

        public List<Compra> ObtenerComprasUsuario(int usuarioId)
        {
            return _repositorioCompras.ComprasPorUsuario(usuarioId);
        }

        public byte[] GenerarReciboPDF(int compraId)
        {
            var compra = _repositorioCompras.BuscarPorId(compraId);
            return Utils.GeneradorPDF.GenerarRecibo(compra, null, null);
        }

        public void DescargarRecibo(int compraId, string rutaDestino)
        {
            var pdf = GenerarReciboPDF(compraId);
            Utils.GeneradorPDF.GuardarPDF(pdf, rutaDestino);
        }
    }
}
