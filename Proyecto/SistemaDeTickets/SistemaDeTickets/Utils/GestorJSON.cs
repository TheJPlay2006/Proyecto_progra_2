using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace SistemaDeTickets.Utils
{
    public static class GestorJSON
    {
        // Locks por archivo para mejor concurrencia
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locksPorArchivo =
            new ConcurrentDictionary<string, SemaphoreSlim>();

        /// <summary>
        /// Obtiene un SemaphoreSlim específico para un archivo
        /// </summary>
        private static SemaphoreSlim ObtenerLockArchivo(string rutaArchivo)
        {
            return _locksPorArchivo.GetOrAdd(rutaArchivo, new SemaphoreSlim(1, 1));
        }

        public static T LeerArchivo<T>(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo)) return default;
            
            var lockArchivo = ObtenerLockArchivo(rutaArchivo);
            lockArchivo.Wait();
            
            try
            {
                string json = File.ReadAllText(rutaArchivo);
                return JsonConvert.DeserializeObject<T>(json);
            }
            finally
            {
                lockArchivo.Release();
            }
        }

        public static void EscribirArchivo<T>(string rutaArchivo, T datos)
        {
            var lockArchivo = ObtenerLockArchivo(rutaArchivo);
            lockArchivo.Wait();
            
            try
            {
                string json = JsonConvert.SerializeObject(datos, Formatting.Indented);
                File.WriteAllText(rutaArchivo, json);
            }
            finally
            {
                lockArchivo.Release();
            }
        }

        public static void EscribirAtomico<T>(string rutaArchivo, T datos)
        {
            string tempFile = rutaArchivo + ".tmp";
            
            // Escribir al archivo temporal
            EscribirArchivo(tempFile, datos);
            
            var lockArchivo = ObtenerLockArchivo(rutaArchivo);
            lockArchivo.Wait();
            
            try
            {
                if (File.Exists(rutaArchivo))
                    File.Delete(rutaArchivo);
                File.Move(tempFile, rutaArchivo);
            }
            finally
            {
                lockArchivo.Release();
            }
        }

        /// <summary>
        /// Método adicional para limpiar locks no utilizados (opcional para limpieza de memoria)
        /// </summary>
        public static void LimpiarLocksInactivos()
        {
            // Este método podría implementarse si fuera necesario limpiar memoria
            // En un caso real, probablemente no sería necesario ya que los locks
            // están en un ConcurrentDictionary y su cantidad es limitada por archivos únicos
        }
    }
}
