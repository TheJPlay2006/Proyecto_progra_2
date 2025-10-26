using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SistemaDeTickets.Controlador
{
    public class RepositorioCompras
    {
        private const string RutaArchivo = @"Data/Compras.json";
        private List<Compra> _compras;

        public RepositorioCompras()
        {
            _compras = GestorJSON.LeerArchivo<List<Compra>>(RutaArchivo) ?? new List<Compra>();
        }

        public void Agregar(Compra compra)
        {
            compra.Id = _compras.Count > 0 ? _compras.Max(c => c.Id) + 1 : 1;
            _compras.Add(compra);
            GestorJSON.EscribirAtomico(RutaArchivo, _compras);
        }

        public void Eliminar(int compraId)
        {
            _compras.RemoveAll(c => c.Id == compraId);
            GestorJSON.EscribirAtomico(RutaArchivo, _compras);
        }

        public List<Compra> ComprasPorUsuario(int usuarioId)
        {
            return _compras.Where(c => c.UsuarioId == usuarioId).ToList();
        }

        public Compra BuscarPorId(int compraId)
        {
            return _compras.FirstOrDefault(c => c.Id == compraId);
        }
    }
}
