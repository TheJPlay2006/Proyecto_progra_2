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

namespace SistemaDeTickets.Vista
{
    /// <summary>
    /// Formulario principal de inicio de la aplicación.
    /// Permite navegación a login, registro y visualización de eventos.
    /// Incluye notificaciones en tiempo real mediante patrón Observer.
    /// </summary>
    public partial class Inicio : Form
    {
        private UserControlNotificacion _notificacionControl;
        private GestorEventos _gestorEventos;

        public Inicio()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Configurar apariencia moderna
            this.Text = "Sistema de Tickets - Inicio";
            this.BackColor = Color.FromArgb(247, 247, 251); // Color de fondo claro

            // Configurar título principal
            if (this.Controls.ContainsKey("lblTitulo"))
            {
                var lblTitulo = this.Controls["lblTitulo"] as Label;
                if (lblTitulo != null)
                {
                    lblTitulo.Font = new Font("Segoe UI", 24, FontStyle.Bold);
                    lblTitulo.ForeColor = Color.FromArgb(30, 31, 59); // Color primario
                    lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
                }
            }

            // Configurar botones con estilo moderno (solo si existen en el designer)
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Name.Contains("IniciarSesion") || btn.Name.Contains("Login"))
                        ConfigurarBoton(btn, "Iniciar Sesión", Color.FromArgb(255, 56, 92));
                    else if (btn.Name.Contains("Registrarse") || btn.Name.Contains("Registro"))
                        ConfigurarBoton(btn, "Registrarse", Color.FromArgb(48, 63, 159));
                    else if (btn.Name.Contains("VerEvento") || btn.Name.Contains("Eventos"))
                        ConfigurarBoton(btn, "Ver Eventos", Color.FromArgb(76, 175, 80));
                }
            }

            InicializarNotificaciones();
        }

        private void ConfigurarBoton(Button btn, string texto, Color colorFondo)
        {
            if (btn != null)
            {
                btn.Text = texto;
                btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                btn.BackColor = colorFondo;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Height = 45;
                btn.Width = 200;
                btn.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Inicializa el control de notificaciones y lo registra como observador.
        /// Se asegura que todos los componentes estén inicializados antes de manipularlos.
        /// Patrón seguro: inicializar dependencias → crear controles → agregar al formulario → registrar observers
        /// </summary>
        private void InicializarNotificaciones()
        {
            try
            {
                // Paso 1: Inicializar gestor de eventos (dependencia principal)
                if (_gestorEventos == null)
                {
                    _gestorEventos = new GestorEventos();
                }

                // Paso 2: Crear e inicializar el control de notificaciones
                if (_notificacionControl == null)
                {
                    _notificacionControl = new UserControlNotificacion();
                    _notificacionControl.Location = new Point(10, 10);
                }

                // Paso 3: Agregar al formulario solo después de inicializar completamente
                if (!this.Controls.Contains(_notificacionControl))
                {
                    this.Controls.Add(_notificacionControl);
                }

                // Paso 4: Registrar como observador de eventos (después de que todo esté inicializado)
                if (_gestorEventos != null && _notificacionControl != null)
                {
                    _gestorEventos.AgregarObservador(_notificacionControl);
                }
            }
            catch (Exception ex)
            {
                // Log del error para debugging (sin bloquear la aplicación)
                System.Diagnostics.Debug.WriteLine($"Error en InicializarNotificaciones: {ex.Message}");
                // En producción, podría mostrar un mensaje al usuario
            }
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            // Navegar a login: ocultar ventana anterior y centrar nueva
            var loginForm = new VistaLogin();
            loginForm.StartPosition = FormStartPosition.CenterScreen;
            loginForm.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Evento de imagen (sin implementación específica)
        }

        private void btnVerEventos_Click(object sender, EventArgs e)
        {
        }

        private void btnIniciarSesion_Click_1(object sender, EventArgs e)
        {
            // Duplicado del método anterior - mantener consistencia
            btnIniciarSesion_Click(sender, e);
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            // Navegar a registro: ocultar ventana anterior y centrar nueva
            var registroForm = new VistaRegistro();
            registroForm.StartPosition = FormStartPosition.CenterScreen;
            registroForm.Show();
            this.Hide();
        }

        private void btnVerEvento_Click(object sender, EventArgs e)
        {
            var vistaEvento = new VistaEvento();
            vistaEvento.StartPosition = FormStartPosition.CenterScreen;
            vistaEvento.Show();
            this.Hide(); // Oculta la ventana actual sin terminar la aplicación
        }
    }
}
