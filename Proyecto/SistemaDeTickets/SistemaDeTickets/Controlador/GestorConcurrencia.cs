using SistemaDeTickets.Modelo;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaDeTickets.Controlador
{
    /// <summary>
    /// Gestor mejorado de concurrencia para operaciones de compra de tickets.
    /// Implementa bloqueos por evento para prevenir sobreventa y race conditions.
    /// </summary>
    public class GestorConcurrencia
    {
        // Diccionario de semáforos por evento para control de concurrencia
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> semaforosPorEvento =
            new ConcurrentDictionary<int, SemaphoreSlim>();

        // Diccionario de locks tradicionales como respaldo
        private static readonly ConcurrentDictionary<int, object> locks = new ConcurrentDictionary<int, object>();

        // Timeout para adquisiciones de lock (evitar deadlocks)
        private const int TIMEOUT_LOCK_SEGUNDOS = 30;

        /// <summary>
        /// Crea o obtiene un semáforo para el evento especificado
        /// </summary>
        private SemaphoreSlim ObtenerSemaforo(int eventoId)
        {
            return semaforosPorEvento.GetOrAdd(eventoId, new SemaphoreSlim(1, 1));
        }

        /// <summary>
        /// Crea o obtiene un lock tradicional para el evento especificado
        /// </summary>
        public object CrearLock(int eventoId)
        {
            return locks.GetOrAdd(eventoId, new object());
        }

        /// <summary>
        /// Adquiere un lock de forma síncrona con timeout
        /// </summary>
        public void AdquirirLock(int eventoId)
        {
            var semaforo = ObtenerSemaforo(eventoId);
            if (!semaforo.Wait(TIMEOUT_LOCK_SEGUNDOS * 1000))
            {
                throw new TimeoutException($"Timeout al adquirir lock para evento {eventoId}");
            }
        }

        /// <summary>
        /// Libera el lock del evento especificado
        /// </summary>
        public void LiberarLock(int eventoId)
        {
            if (semaforosPorEvento.TryGetValue(eventoId, out var semaforo))
            {
                semaforo.Release();
            }
        }

        /// <summary>
        /// Ejecuta una operación de forma atómica usando lock por evento
        /// </summary>
        public async Task<T> EjecutarAtomicoAsync<T>(int eventoId, Func<Task<T>> operacion)
        {
            var semaforo = ObtenerSemaforo(eventoId);

            await semaforo.WaitAsync();

            try
            {
                return await operacion();
            }
            finally
            {
                semaforo.Release();
            }
        }

        /// <summary>
        /// Ejecuta una operación de forma atómica usando lock por evento (síncrona)
        /// </summary>
        public T EjecutarAtomico<T>(int eventoId, Func<T> operacion)
        {
            var semaforo = ObtenerSemaforo(eventoId);

            semaforo.Wait();

            try
            {
                return operacion();
            }
            finally
            {
                semaforo.Release();
            }
        }

        /// <summary>
        /// Procesa una compra de forma segura con control de concurrencia
        /// </summary>
        public async Task<ResultadoCompraConcurrencia> ProcesarCompraAsync(Compra compra, Func<Task<bool>> operacionCompra)
        {
            var semaforo = ObtenerSemaforo(compra.EventoId);

            // Intentar adquirir el lock con timeout
            if (!await semaforo.WaitAsync(TIMEOUT_LOCK_SEGUNDOS * 1000))
            {
                return new ResultadoCompraConcurrencia
                {
                    Exitoso = false,
                    Mensaje = $"Sistema ocupado. Intente nuevamente en unos segundos.",
                    MotivoFallo = MotivoFalloCompra.TimeoutLock
                };
            }

            try
            {
                // Ejecutar la operación de compra
                bool compraExitosa = await operacionCompra();

                return new ResultadoCompraConcurrencia
                {
                    Exitoso = compraExitosa,
                    Mensaje = compraExitosa ? "Compra procesada exitosamente" : "Compra fallida",
                    MotivoFallo = compraExitosa ? MotivoFalloCompra.Ninguno : MotivoFalloCompra.StockInsuficiente
                };
            }
            catch (Exception ex)
            {
                return new ResultadoCompraConcurrencia
                {
                    Exitoso = false,
                    Mensaje = $"Error durante la compra: {ex.Message}",
                    MotivoFallo = MotivoFalloCompra.ErrorDesconocido
                };
            }
            finally
            {
                semaforo.Release();
            }
        }

        /// <summary>
        /// Verifica si hay operaciones pendientes para un evento
        /// </summary>
        public bool HayOperacionesPendientes(int eventoId)
        {
            if (semaforosPorEvento.TryGetValue(eventoId, out var semaforo))
            {
                return semaforo.CurrentCount == 0;
            }
            return false;
        }

        /// <summary>
        /// Obtiene el número de operaciones esperando para un evento
        /// </summary>
        public int ObtenerOperacionesEsperando(int eventoId)
        {
            if (semaforosPorEvento.TryGetValue(eventoId, out var semaforo))
            {
                return 1 - semaforo.CurrentCount;
            }
            return 0;
        }
    }

    /// <summary>
    /// Resultado de una operación de compra con control de concurrencia
    /// </summary>
    public class ResultadoCompraConcurrencia
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public MotivoFalloCompra MotivoFallo { get; set; }
    }

    /// <summary>
    /// Motivos posibles de fallo en una compra
    /// </summary>
    public enum MotivoFalloCompra
    {
        Ninguno,
        StockInsuficiente,
        TimeoutLock,
        ErrorDesconocido
    }
}
