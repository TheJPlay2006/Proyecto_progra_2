using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using SistemaDeTickets.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            return detalle.Cantidad > 0 && detalle.Evento != null && detalle.Evento.TiquetesDisponibles >= detalle.Cantidad;
        }

        public bool ProcesarPago(DetalleCompra detalle, string numeroTarjeta, string cvv)
        {
            return ValidadorTarjeta.ValidarNumero(numeroTarjeta) && ValidadorTarjeta.ValidarCVV(cvv);
        }

        /// <summary>
        /// Confirma la compra y actualiza el stock del evento de forma persistente
        /// </summary>
        public Compra ConfirmarCompra(DetalleCompra detalle)
        {
            // RECARGAR EVENTOS FRESCOS DEL ARCHIVO ANTES DE OPERAR
            var eventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();
            var eventoActual = eventos.FirstOrDefault(e => e.Id == detalle.Evento.Id);

            // Verificación robusta de evento existente
            if (eventoActual == null)
            {
                throw new InvalidOperationException($"El evento con ID {detalle.Evento.Id} no fue encontrado. Puede haber sido eliminado.");
            }

            if (eventoActual.TiquetesDisponibles < detalle.Cantidad)
            {
                throw new InvalidOperationException($"No hay suficiente stock disponible. Solo quedan {eventoActual.TiquetesDisponibles} tickets.");
            }

            // Descontar stock del evento usando escritura atómica
            ActualizarStockEventoPersistente(detalle.Evento.Id, detalle.Cantidad);

            // Crear y guardar la compra
            Compra compra = new Compra
            {
                UsuarioId = ServicioAutenticacion.CurrentUser?.Id ?? 0,
                EventoId = detalle.Evento.Id,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = (decimal)detalle.Evento.Precio,
                PrecioTotal = detalle.Total,
                FechaCompra = DateTime.Now,
                Estado = EstadoCompra.Completada,
                MetodoPago = MetodoPago.TarjetaCredito
            };

            _repositorioCompras.Agregar(compra);
            return compra;
        }

        /// <summary>
        /// Actualiza el stock de un evento de forma persistente y atómica
        /// </summary>
        /// <param name="eventoId">ID del evento</param>
        /// <param name="cantidadDescontar">Cantidad a descontar del stock</param>
        private void ActualizarStockEventoPersistente(int eventoId, int cantidadDescontar)
        {
            // Cargar eventos frescos del archivo
            var eventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();

            // Buscar el evento específico
            var evento = eventos.FirstOrDefault(e => e.Id == eventoId);

            if (evento != null && evento.TiquetesDisponibles >= cantidadDescontar)
            {
                // Descontar stock
                evento.TiquetesDisponibles -= cantidadDescontar;

                // Guardar cambios usando escritura atómica - PERSISTENTE EN ARCHIVO
                GestorJSON.EscribirAtomico("Data/MisEventos.json", eventos);
            }
            else
            {
                throw new InvalidOperationException("Stock insuficiente para completar la operación.");
            }
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

            string rutaPdf = Utils.GeneradorPDF.GenerarRecibo(compra, null, null);
            return File.ReadAllBytes(rutaPdf);
        }
    }
}
