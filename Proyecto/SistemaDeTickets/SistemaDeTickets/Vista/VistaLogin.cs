using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaDeTickets.Controlador;
using SistemaDeTickets.Modelo;
using SistemaDeTickets.Services;

namespace SistemaDeTickets.Vista
{
    /// <summary>
    /// Formulario de login del sistema.
    /// Maneja autenticación de usuarios y navegación a recuperación de contraseña.
    /// </summary>
    public partial class VistaLogin : Form
    {
        private ControladorAutenticacion _controladorAutenticacion;

        // Contexto de navegación: de dónde viene el usuario
        public enum ContextoNavegacion
        {
            DesdeInicio,      // Vino desde la pantalla de inicio
            DesdeRegistro,    // Vino desde registro
            DesdeCompraEvento // Vino porque intentó comprar en VistaEvento
        }

        public ContextoNavegacion ContextoOrigen { get; set; } = ContextoNavegacion.DesdeInicio;

        // Cache del evento seleccionado (para restaurar selección después del login)
        public Modelo.Evento EventoSeleccionadoCache { get; set; }

        public VistaLogin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema de Tickets - Iniciar Sesión";
            // Restaurar imagen de fondo original
            try
            {
                string rutaImagen = System.IO.Path.Combine(Application.StartupPath, "Images", "Fondo_Pop-Conciertos_Fondo-claro_F7F7FB_3840x2160.png");
                if (System.IO.File.Exists(rutaImagen))
                {
                    this.BackgroundImage = Image.FromFile(rutaImagen);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch
            {
                // Si no se puede cargar la imagen, mantener color sólido
            }

            _controladorAutenticacion = new ControladorAutenticacion();

            // Configurar apariencia moderna
            ConfigurarControlesLogin();

            // Configurar visibilidad inicial de contraseña
            txtPassword.UseSystemPasswordChar = true;
        }

        private void ConfigurarControlesLogin()
        {
            // Configurar título
            if (this.Controls.ContainsKey("lblTitulo") || this.Controls.ContainsKey("label1"))
            {
                var titulo = this.Controls.OfType<Label>().FirstOrDefault(l => l.Text.Contains("Login") || l.Text.Contains("Iniciar"));
                if (titulo != null)
                {
                    titulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
                    titulo.ForeColor = Color.FromArgb(30, 31, 59);
                    titulo.TextAlign = ContentAlignment.MiddleCenter;
                }
            }

            // Configurar botones con colores originales (blanco)
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Name.Contains("Login") || btn.Name.Contains("Iniciar"))
                        ConfigurarBoton(btn, "Iniciar Sesión", Color.White);
                    else if (btn.Name.Contains("Volver"))
                        ConfigurarBoton(btn, "Volver", Color.White);
                    else if (btn.Name.Contains("Salir"))
                        ConfigurarBoton(btn, "Salir", Color.White);
                }
                else if (ctrl is TextBox txt)
                {
                    txt.Font = new Font("Segoe UI", 10);
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        private void ConfigurarBoton(Button btn, string texto, Color colorFondo)
        {
            btn.Text = texto;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.BackColor = colorFondo;
            btn.ForeColor = Color.Black;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Height = 40;
            btn.Cursor = Cursors.Hand;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            Console.WriteLine($"[DEBUG VISTALOGIN] Email ingresado: '{email}'");
            Console.WriteLine($"[DEBUG VISTALOGIN] Password ingresado: '{password}'");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar credenciales usando el servicio de autenticación
            bool credencialesValidas = ServicioAutenticacion.Login(email, password);

            if (credencialesValidas)
            {
                var usuario = ServicioAutenticacion.CurrentUser;

                // 🔹 Verificar el rol del usuario usando enum
                if (usuario.Rol == RolUsuario.Admin)
                {
                    // Si es ADMIN → abrir formulario de cambios de eventos
                    this.Hide();
                    var cambios = new VistaCambiosEventos();
                    cambios.StartPosition = FormStartPosition.CenterScreen;
                    cambios.ShowDialog();
                    this.Close(); // Cerrar login después de gestión
                }
                else // RolUsuario.Usuario
                {
                    // Si es USUARIO → navegar según el contexto
                    this.Hide();
                    switch (ContextoOrigen)
                    {
                        case ContextoNavegacion.DesdeCompraEvento:
                            var ventanaEventos = new VistaEvento();
                            ventanaEventos.StartPosition = FormStartPosition.CenterScreen;

                            if (EventoSeleccionadoCache != null)
                            {
                                // Aquí podrías preseleccionar el evento, si lo implementas
                            }

                            ventanaEventos.ShowDialog();
                            break;

                        case ContextoNavegacion.DesdeRegistro:
                        case ContextoNavegacion.DesdeInicio:
                        default:
                            var ventanaEventosDefault = new VistaEvento();
                            ventanaEventosDefault.StartPosition = FormStartPosition.CenterScreen;
                            ventanaEventosDefault.ShowDialog();
                            break;
                    }
                    this.Close(); // Cerrar login después de eventos
                }
            }
            else
            {
                // Mostrar error de credenciales
                MessageBox.Show("Credenciales incorrectas. Verifique su email y contraseña.", "Error de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }


        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            // Botón "Registrarse": navegar a registro
            this.Hide();
            var registroForm = new VistaRegistro();
            registroForm.StartPosition = FormStartPosition.CenterScreen;
            registroForm.ShowDialog();
            this.Show(); // Mostrar login nuevamente cuando se cierre registro
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Navegación contextual según el origen
            this.Hide();
            switch (ContextoOrigen)
            {
                case ContextoNavegacion.DesdeCompraEvento:
                    // Si vino desde compra en VistaEvento, volver a VistaEvento
                    var ventanaEventos = new VistaEvento();
                    ventanaEventos.StartPosition = FormStartPosition.CenterScreen;
                    ventanaEventos.ShowDialog();
                    break;

                case ContextoNavegacion.DesdeRegistro:
                    // Si vino desde registro, volver a registro
                    var registroForm = new VistaRegistro();
                    registroForm.StartPosition = FormStartPosition.CenterScreen;
                    registroForm.ShowDialog();
                    break;

                case ContextoNavegacion.DesdeInicio:
                default:
                    // Por defecto, volver a inicio
                    var inicioForm = new Inicio();
                    inicioForm.StartPosition = FormStartPosition.CenterScreen;
                    inicioForm.ShowDialog();
                    break;
            }
            this.Show(); // Mostrar login nuevamente cuando se cierre la ventana anterior
        }

        // Método obsoleto - reemplazado por btnOcultarVerContra_Click

        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Salir directamente sin confirmación
            Application.Exit();
        }

        private void linkOlvidePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Cerrar/ocultar la ventana de login de inmediato
            this.Hide();

            // Abrir la ventana de recuperación de contraseña en modo modal
            var recuperacionForm = new VistaRecuperacionPassword();
            recuperacionForm.StartPosition = FormStartPosition.CenterScreen;
            recuperacionForm.ShowDialog();

            // Cerrar la ventana de login después de que se cierre la recuperación
            this.Close();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnOcultarVerContra_Click(object sender, EventArgs e)
        {
            // Alternar visibilidad de la contraseña
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;

            // Cambiar ícono del botón basado en el nuevo estado
            btnOcultarVerContra.Text = txtPassword.UseSystemPasswordChar ? "👁" : "🙈";
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblCorreo_Click(object sender, EventArgs e)
        {

        }
    }
}
