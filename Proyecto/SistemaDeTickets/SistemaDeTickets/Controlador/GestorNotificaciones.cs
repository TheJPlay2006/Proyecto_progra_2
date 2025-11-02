using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaDeTickets.Modelo;

namespace SistemaDeTickets.Controlador
{
    /// <summary>
    /// Gestor de notificaciones del sistema usando NotifyIcon para el área de notificación de Windows
    /// </summary>
    public class GestorNotificaciones : IDisposable
    {
        private NotifyIcon _notifyIcon;
        private readonly Timer _timerAutoOcultar;

        public GestorNotificaciones()
        {
            InicializarNotifyIcon();
            _timerAutoOcultar = new Timer();
            _timerAutoOcultar.Interval = 5000; // 5 segundos
            _timerAutoOcultar.Tick += TimerAutoOcultar_Tick;
        }

        private void InicializarNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Information, // Icono por defecto
                Visible = true,
                Text = "Sistema de Tickets"
            };

            // Manejar doble clic en el icono
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        }

        /// <summary>
        /// Muestra una notificación de nuevo evento disponible
        /// </summary>
        public void NotificarNuevoEvento(string nombreEvento)
        {
            _notifyIcon.ShowBalloonTip(
                5000,
                "¡Nuevo Evento Disponible!",
                $"¡Nuevo evento disponible: {nombreEvento}!",
                ToolTipIcon.Info
            );
            _timerAutoOcultar.Start();
        }

        /// <summary>
        /// Muestra una notificación cuando quedan pocos tickets
        /// </summary>
        public void NotificarPocosTiques(string nombreEvento, int restantes)
        {
            _notifyIcon.ShowBalloonTip(
                5000,
                "¡Pocos Tickets Disponibles!",
                $"¡Quedan pocos tiques para {nombreEvento}! Solo {restantes} disponibles.",
                ToolTipIcon.Warning
            );
            _timerAutoOcultar.Start();
        }

        /// <summary>
        /// Muestra una notificación de compra exitosa
        /// </summary>
        public void NotificarCompraExitosa(string nombreEvento, int cantidad)
        {
            _notifyIcon.ShowBalloonTip(
                3000,
                "Compra Exitosa",
                $"¡Compra realizada exitosamente! {cantidad} ticket(s) para {nombreEvento}",
                ToolTipIcon.Info
            );
            _timerAutoOcultar.Start();
        }

        /// <summary>
        /// Muestra una notificación de error
        /// </summary>
        public void NotificarError(string mensaje)
        {
            _notifyIcon.ShowBalloonTip(
                5000,
                "Error",
                mensaje,
                ToolTipIcon.Error
            );
            _timerAutoOcultar.Start();
        }

        /// <summary>
        /// Muestra una notificación de advertencia
        /// </summary>
        public void NotificarAdvertencia(string mensaje)
        {
            _notifyIcon.ShowBalloonTip(
                4000,
                "Advertencia",
                mensaje,
                ToolTipIcon.Warning
            );
            _timerAutoOcultar.Start();
        }

        private void TimerAutoOcultar_Tick(object sender, EventArgs e)
        {
            // El timer se reinicia automáticamente en cada notificación
            _timerAutoOcultar.Stop();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // Aquí se podría abrir la aplicación principal si está minimizada
            // Por ahora, solo mostramos un mensaje
            MessageBox.Show("Sistema de Tickets - Doble clic en el icono de notificación",
                          "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Dispose()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
            }
            _timerAutoOcultar?.Dispose();
        }
    }
}