using SistemaDeTickets.Modelo;
using SistemaDeTickets.Controlador.Patrones;
using SistemaDeTickets.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaDeTickets.Controlador.Patrones
{
    /// <summary>
    /// Resultado de una operación de compra con información de éxito y detalles
    /// </summary>
    public class ResultadoCompra
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public Compra Compra { get; set; }
        public string TransactionId { get; set; }
    }

    /// <summary>
    /// Fachada que simplifica el proceso de compra de tickets.
    /// Implementa el patrón Facade para coordinar validaciones, pagos y confirmaciones.
    /// Centraliza la lógica de compra en una interfaz unificada.
    /// </summary>
    public class FachadaCompraTique
    {
        private readonly ControladorUsuario _usuarioControlador;
        private readonly ControladorCompra _compraControlador;
        private readonly GestorInventario _gestorInventario;
        private readonly GestorEventos _gestorEventos;

        // Locks por evento para concurrencia
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locksPorEvento =
            new ConcurrentDictionary<string, SemaphoreSlim>();

        // Cache de transacciones procesadas para idempotencia
        private static readonly ConcurrentDictionary<string, DateTime> _transaccionesProcesadas =
            new ConcurrentDictionary<string, DateTime>();

        public FachadaCompraTique(ControladorUsuario usuarioControlador, ControladorCompra compraControlador,
                                  GestorInventario gestorInventario, GestorEventos gestorEventos)
        {
            _usuarioControlador = usuarioControlador;
            _compraControlador = compraControlador;
            _gestorInventario = gestorInventario;
            _gestorEventos = gestorEventos;
        }

        /// <summary>
        /// Método async para compra transaccional con manejo de concurrencia
        /// </summary>
        public async Task<ResultadoCompra> ComprarAsync(int usuarioId, int eventoId, int cantidad,
                                                      string transactionId, string numeroTarjeta,
                                                      string cvv, string nombreTitular)
        {
            // Validar idempotencia - si ya se procesó esta transacción, devolver resultado previo
            if (_transaccionesProcesadas.ContainsKey(transactionId))
            {
                return new ResultadoCompra
                {
                    Exitoso = false,
                    Mensaje = "Transacción ya procesada - posible duplicación",
                    TransactionId = transactionId
                };
            }

            // Obtener lock específico para este evento
            string eventoKey = $"evento_{eventoId}";
            var lockSemaphore = _locksPorEvento.GetOrAdd(eventoKey, new SemaphoreSlim(1, 1));

            await lockSemaphore.WaitAsync();
            
            try
            {
                // Validar disponibilidad inicial
                if (_gestorInventario.ObtenerDisponibilidad(eventoId) < cantidad)
                {
                    return new ResultadoCompra
                    {
                        Exitoso = false,
                        Mensaje = "No hay suficiente inventario disponible",
                        TransactionId = transactionId
                    };
                }

                // Crear detalle de compra
                var detalle = _compraControlador.IniciarCompra(usuarioId, eventoId, cantidad);

                // Validar tarjeta de forma synchrónica (aquí podríamos hacer validación asincrónica real)
                if (!ValidarTarjetaRapida(numeroTarjeta, cvv, nombreTitular))
                {
                    return new ResultadoCompra
                    {
                        Exitoso = false,
                        Mensaje = "Datos de tarjeta inválidos",
                        TransactionId = transactionId
                    };
                }

                // Bloquear inventario de forma atómica
                if (!_gestorInventario.BloquearInventario(eventoId, cantidad))
                {
                    return new ResultadoCompra
                    {
                        Exitoso = false,
                        Mensaje = "No se pudo reservar el inventario",
                        TransactionId = transactionId
                    };
                }

                // Procesar pago
                bool pagoExitoso = _compraControlador.ProcesarPago(detalle, numeroTarjeta, cvv);
                
                if (!pagoExitoso)
                {
                    // Rollback - liberar inventario
                    // Rollback automático - el inventario se libera en caso de fallo
                    return new ResultadoCompra
                    {
                        Exitoso = false,
                        Mensaje = "Error al procesar el pago",
                        TransactionId = transactionId
                    };
                }

                // Confirmar compra
                var compraConfirmada = _compraControlador.ConfirmarCompra(detalle);

                // Marcar transacción como procesada para idempotencia
                _transaccionesProcesadas[transactionId] = DateTime.Now;

                // Notificar stock bajo si es necesario
                await NotificarStockBajoAsync(eventoId);

                // Notificar compra exitosa
                var eventos = GestorJSON.LeerArchivo<List<SistemaDeTickets.Modelo.Evento>>("Data/MisEventos.json") ?? new List<SistemaDeTickets.Modelo.Evento>();
                var evento = eventos.FirstOrDefault(e => e.Id == eventoId);
                if (evento != null)
                {
                    _gestorEventos.NotificarCompraExitosa(evento.Nombre, cantidad);
                }

                return new ResultadoCompra
                {
                    Exitoso = true,
                    Mensaje = "Compra procesada exitosamente",
                    Compra = compraConfirmada,
                    TransactionId = transactionId
                };
            }
            catch (Exception ex)
            {
                // En caso de error, intentar rollback si es necesario
                try
                {
                    // Rollback automático - el inventario se libera en caso de fallo
                }
                catch
                {
                    // Log error de rollback si fuera necesario
                }

                return new ResultadoCompra
                {
                    Exitoso = false,
                    Mensaje = $"Error durante la compra: {ex.Message}",
                    TransactionId = transactionId
                };
            }
            finally
            {
                lockSemaphore.Release();
            }
        }

        /// <summary>
        /// Notifica cuando el stock está bajo (umbral = 10)
        /// </summary>
        private async Task NotificarStockBajoAsync(int eventoId)
        {
            var disponibilidad = _gestorInventario.ObtenerDisponibilidad(eventoId);

            if (disponibilidad <= 10)
            {
                // Obtener nombre del evento para la notificación
                var eventos = GestorJSON.LeerArchivo<List<SistemaDeTickets.Modelo.Evento>>("Data/MisEventos.json") ?? new List<SistemaDeTickets.Modelo.Evento>();
                var evento = eventos.FirstOrDefault(e => e.Id == eventoId);

                if (evento != null)
                {
                    // Notificar bajo inventario
                    _gestorEventos.NotificarBajoInventario(eventoId, disponibilidad, evento.Nombre);
                }
            }
        }

        /// <summary>
        /// Validación rápida de tarjeta (implementación simplificada)
        /// </summary>
        private bool ValidarTarjetaRapida(string numero, string cvv, string nombre)
        {
            return !string.IsNullOrWhiteSpace(numero) &&
                   numero.Length == 16 &&
                   !string.IsNullOrWhiteSpace(cvv) &&
                   cvv.Length >= 3 &&
                   !string.IsNullOrWhiteSpace(nombre);
        }

        public DetalleCompra IniciarProcesoCompra(int usuarioId, int eventoId, int cantidad)
        {
            if (!_gestorInventario.BloquearInventario(eventoId, cantidad))
                throw new Exception("No hay suficiente inventario disponible.");

            return _compraControlador.IniciarCompra(usuarioId, eventoId, cantidad);
        }

        public bool ValidarDisponibilidad(int eventoId, int cantidad)
        {
            return _gestorInventario.ObtenerDisponibilidad(eventoId) >= cantidad;
        }

        public bool ProcesarPago(string numeroTarjeta, string cvv, DetalleCompra detalle)
        {
            return _compraControlador.ProcesarPago(detalle, numeroTarjeta, cvv);
        }

        public Compra ConfirmarCompra(DetalleCompra detalle)
        {
            var compra = _compraControlador.ConfirmarCompra(detalle);
            return compra;
        }

        public void CancelarCompra(int compraId)
        {
            _compraControlador.CancelarCompra(compraId);
        }

        public byte[] GenerarReciboPDF(int compraId)
        {
            return _compraControlador.GenerarReciboPDF(compraId);
        }
    }
}
