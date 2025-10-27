using System;
using System.IO;

namespace SistemaDeTickets.Utils
{
    public static class Logger
    {
        private static string archivoLog = "log_sistema_tickets.txt";

        public static void Log(string mensaje)
        {
            Escribir($"INFO: {mensaje}");
        }

        public static void Error(string mensaje, Exception ex)
        {
            Escribir($"ERROR: {mensaje} -- {ex.Message}");
        }

        public static void Info(string mensaje)
        {
            Escribir($"INFO: {mensaje}");
        }

        public static void Warning(string mensaje)
        {
            Escribir($"WARNING: {mensaje}");
        }

        private static void Escribir(string mensaje)
        {
            File.AppendAllText(archivoLog, $"{DateTime.Now}: {mensaje}{Environment.NewLine}");
        }
    }
}
