using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaDeTickets.Controlador.Patrones;
using SistemaDeTickets.Modelo;

namespace SistemaDeTickets.Vista
{
    /// <summary>
    /// UserControl para mostrar notificaciones en la UI.
    /// Implementa el patrón Observer para recibir actualizaciones del sistema.
    /// </summary>
    public partial class UserControlNotificacion : UserControl, IObservador
    {
        private Timer _timerAutoOcultar;

        public UserControlNotificacion()
        {
            InitializeComponent();
            InicializarTimer();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.timerAutoOcultar = new System.Windows.Forms.Timer(this.components);
        }

        private void InicializarTimer()
        {
            _timerAutoOcultar = new Timer();
            _timerAutoOcultar.Interval = 5000; // 5 segundos
            _timerAutoOcultar.Tick += TimerAutoOcultar_Tick;
        }

        /// <summary>
        /// Implementación del patrón Observer.
        /// Actualiza la notificación según el tipo y datos recibidos.
        /// </summary>
        /// <param name="tipo">Tipo de notificación</param>
        /// <param name="datos">Datos adicionales</param>
        public void Actualizar(TipoNotificacion tipo, object datos)
        {
            string mensaje = "";

            switch (tipo)
            {
                case TipoNotificacion.NuevoEvento:
                    var evento = datos as Modelo.Evento;
                    mensaje = $"¡Nuevo evento disponible: {evento?.Nombre}!";
                    break;
                case TipoNotificacion.BajoInventario:
                    var eventoId = (int)datos;
                    mensaje = $"¡Atención! Quedan pocos tickets para el evento {eventoId}";
                    break;
                case TipoNotificacion.CompraExitosa:
                    mensaje = "¡Compra realizada exitosamente!";
                    break;
            }

            MostrarNotificacion(mensaje);
        }

        /// <summary>
        /// Muestra una notificación con el mensaje especificado.
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar</param>
        public void MostrarNotificacion(string mensaje)
        {
            lblMensaje.Text = mensaje;
            this.Visible = true;
            _timerAutoOcultar.Start();
        }

        private void TimerAutoOcultar_Tick(object sender, EventArgs e)
        {
            this.Visible = false;
            _timerAutoOcultar.Stop();
        }

        /// <summary>
        /// Oculta manualmente la notificación.
        /// </summary>
        public void OcultarNotificacion()
        {
            this.Visible = false;
            _timerAutoOcultar.Stop();
        }
    }
}