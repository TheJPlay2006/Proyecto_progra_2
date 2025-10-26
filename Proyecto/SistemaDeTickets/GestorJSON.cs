using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace SistemaDeTickets.Utils
{
    public static class GestorJSON
    {
        private static readonly object _bloqueo = new object();

        public static T LeerArchivo<T>(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo)) return default;
            lock (_bloqueo)
            {
                string json = File.ReadAllText(rutaArchivo);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public static void EscribirArchivo<T>(string rutaArchivo, T datos)
        {
            lock (_bloqueo)
            {
                string json = JsonConvert.SerializeObject(datos, Formatting.Indented);
                File.WriteAllText(rutaArchivo, json);
            }
        }

        public static void EscribirAtomico<T>(string rutaArchivo, T datos)
        {
            string tempFile = rutaArchivo + ".tmp";
            EscribirArchivo(tempFile, datos);
            lock (_bloqueo)
            {
                if (File.Exists(rutaArchivo)) File.Delete(rutaArchivo);
                File.Move(tempFile, rutaArchivo);
            }
        }
    }
}
