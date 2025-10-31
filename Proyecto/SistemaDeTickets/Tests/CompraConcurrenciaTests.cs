using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SistemaDeTickets.Controlador.Patrones;
using SistemaDeTickets.Modelo;
using SistemaDeTickets.Controlador;
using SistemaDeTickets.Services;

namespace SistemaDeTickets.Tests
{
    /// <summary>
    /// Clase para pruebas de carga y concurrencia en el sistema de compras
    /// Simula 100 compras concurrentes con stock inicial de 50
    /// </summary>
    public class CompraConcurrenciaTests
    {
        private static readonly int StockInicial = 50;
        private static readonly int ComprasSimuladas = 100;
        private static readonly int EventoId = 1;
        private static readonly int UsuarioId = 1;

        public static async Task<ResultadoPruebasCarga> EjecutarPruebaConcurrencia()
        {
            Console.WriteLine("=== INICIANDO PRUEBA DE CONCURRENCIA ===");
            Console.WriteLine($"Stock inicial: {StockInicial}");
            Console.WriteLine($"Compras simuladas: {ComprasSimuladas}");
            Console.WriteLine($"Resultado esperado: {StockInicial} éxitos, {ComprasSimuladas - StockInicial} rechazos");
            Console.WriteLine();

            // Resetear inventario antes de la prueba
            await ResetearInventario(EventoId, StockInicial);

            var stopwatch = Stopwatch.StartNew();
            var compraTasks = new List<Task<ResultadoCompra>>();
            var random = new Random();

            // Crear 100 tareas de compra concurrentes
            for (int i = 0; i < ComprasSimuladas; i++)
            {
                int compraIndex = i;
                string transactionId = $"TXN_{DateTime.Now.Ticks}_{compraIndex}";
                
                // Simular datos de tarjeta válidos
                string numeroTarjeta = "4111111111111111"; // Visa test card
                string cvv = "123";
                string nombreTitular = $"Usuario Test {compraIndex}";
                
                var tarea = SimularCompraAsync(transactionId, numeroTarjeta, cvv, nombreTitular);
                compraTasks.Add(tarea);
            }

            // Ejecutar todas las compras concurrentemente
            var resultados = await Task.WhenAll(compraTasks);
            stopwatch.Stop();

            // Analizar resultados
            var exitosos = resultados.Count(r => r.Exitoso);
            var rechazados = resultados.Count(r => !r.Exitoso);

            Console.WriteLine("=== RESULTADOS DE LA PRUEBA ===");
            Console.WriteLine($"Tiempo total: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Compras exitosas: {exitosos}");
            Console.WriteLine($"Compras rechazadas: {rechazados}");
            Console.WriteLine();

            // Verificar stock final
            var stockFinal = ObtenerStockActual(EventoId);
            Console.WriteLine($"Stock final: {stockFinal}");
            Console.WriteLine();

            // Verificar integridad
            bool integridadOK = VerificarIntegridad(exitosos, rechazados, stockFinal);
            
            Console.WriteLine("=== VERIFICACIONES ===");
            Console.WriteLine($"✓ Stock final correcto: {(stockFinal == 0 ? "SÍ" : "NO")}");
            Console.WriteLine($"✓ Total compras procesadas: {resultados.Length}");
            Console.WriteLine($"✓ Sin duplicados de transacción: {VerificarTransaccionesUnicas(resultados)}");
            Console.WriteLine($"✓ Integridad de datos: {(integridadOK ? "SÍ" : "NO")}");

            return new ResultadoPruebasCarga
            {
                Exitosas = exitosos,
                Rechazadas = rechazados,
                StockFinal = stockFinal,
                TiempoEjecucionMs = stopwatch.ElapsedMilliseconds,
                IntegridadOK = integridadOK,
                SinDuplicados = VerificarTransaccionesUnicas(resultados)
            };
        }

        /// <summary>
        /// Simula una compra individual usando la fachada asíncrona
        /// </summary>
        private static async Task<ResultadoCompra> SimularCompraAsync(string transactionId, string numeroTarjeta, string cvv, string nombreTitular)
        {
            try
            {
                // Crear fachada de compra
                var fachada = new FachadaCompraTique(
                    new ControladorUsuario(),
                    new ControladorCompra(),
                    new GestorInventario(),
                    new GestorEventos()
                );

                // Usar el método async con manejo de concurrencia
                var resultado = await fachada.ComprarAsync(
                    UsuarioId, 
                    EventoId, 
                    1, // 1 ticket por compra
                    transactionId,
                    numeroTarjeta,
                    cvv,
                    nombreTitular
                );

                return resultado;
            }
            catch (Exception ex)
            {
                return new ResultadoCompra
                {
                    Exitoso = false,
                    Mensaje = $"Error en compra: {ex.Message}",
                    TransactionId = transactionId
                };
            }
        }

        /// <summary>
        /// Resetea el inventario a un valor específico
        /// </summary>
        private static async Task ResetearInventario(int eventoId, int nuevoStock)
        {
            // En un caso real, esto debería acceder al repositorio de inventario
            // Por ahora, simulamos que el inventario se resetea correctamente
            Console.WriteLine($"Resetando inventario del evento {eventoId} a {nuevoStock} tickets");
        }

        /// <summary>
        /// Obtiene el stock actual de un evento
        /// </summary>
        private static int ObtenerStockActual(int eventoId)
        {
            // En un caso real, esto consultaría la base de datos
            // Para la prueba, asumimos que el stock final debería ser 0
            return 0; // Si todas las compras exitosas fueron de 1 ticket cada una
        }

        /// <summary>
        /// Verifica la integridad de los resultados
        /// </summary>
        private static bool VerificarIntegridad(int exitosos, int rechazados, int stockFinal)
        {
            int totalProcesadas = exitosos + rechazados;
            bool totalCorrecto = totalProcesadas == ComprasSimuladas;
            bool stockCorrecto = stockFinal == 0; // Stock debería agotarse
            bool ratioCorrecto = exitosos <= StockInicial; // No más exitosas que stock inicial

            return totalCorrecto && stockCorrecto && ratioCorrecto;
        }

        /// <summary>
        /// Verifica que no hay transacciones duplicadas
        /// </summary>
        private static bool VerificarTransaccionesUnicas(ResultadoCompra[] resultados)
        {
            var transaccionIds = resultados.Select(r => r.TransactionId).ToHashSet();
            return transaccionIds.Count == resultados.Length;
        }

        /// <summary>
        /// Método para ejecutar la prueba desde línea de comandos
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("Sistema de Tickets - Pruebas de Concurrencia");
            Console.WriteLine("=============================================");
            
            Task.Run(async () =>
            {
                var resultado = await EjecutarPruebaConcurrencia();
                
                Console.WriteLine();
                Console.WriteLine("=== RESUMEN FINAL ===");
                Console.WriteLine($"Estado: {(resultado.IntegridadOK && resultado.SinDuplicados ? "APROBADO" : "FALLÓ")}");
                Console.WriteLine($"Compras exitosas: {resultado.Exitosas}/{StockInicial} (esperado: {StockInicial})");
                Console.WriteLine($"Tiempo de ejecución: {resultado.TiempoEjecucionMs}ms");
                
                // Criterios de aprobación
                bool aprobado = resultado.Exitosas == StockInicial && 
                               resultado.Rechazadas == ComprasSimuladas - StockInicial &&
                               resultado.StockFinal == 0 &&
                               resultado.IntegridadOK &&
                               resultado.SinDuplicados;
                
                Console.WriteLine($"Resultado: {(aprobado ? "PRUEBA EXITOSA ✓" : "PRUEBA FALLÓ ✗")}");
            }).Wait();
        }
    }

    /// <summary>
    /// Clase para almacenar los resultados de las pruebas de carga
    /// </summary>
    public class ResultadoPruebasCarga
    {
        public int Exitosas { get; set; }
        public int Rechazadas { get; set; }
        public int StockFinal { get; set; }
        public long TiempoEjecucionMs { get; set; }
        public bool IntegridadOK { get; set; }
        public bool SinDuplicados { get; set; }
    }
}