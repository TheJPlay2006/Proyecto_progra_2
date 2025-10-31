using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaDeTickets.Services;

namespace SistemaDeTickets
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Inicialización básica de la aplicación
            InitializeApplication();

            // Ejecutar migración automática de passwords al inicio de la aplicación
            try
            {
                ServicioAutenticacion.MigrarPasswords();
            }
            catch (Exception ex)
            {
                // Log error silenciosamente, no bloquear el inicio
                Console.WriteLine($"Error en migración automática: {ex.Message}");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // CREDENCIALES ADMIN DE PRUEBA:
            // Email: admin@tickets.com
            // Contraseña: Admin2025*

            Application.Run(new Vista.Inicio());
        }

        /// <summary>
        /// Inicialización básica - sin dependencias complejas
        /// </summary>
        private static void InitializeApplication()
        {
            // Aquí se pueden agregar inicializaciones futuras si es necesario
            Console.WriteLine("[INIT] Aplicación inicializada correctamente");
        }
    }
}
