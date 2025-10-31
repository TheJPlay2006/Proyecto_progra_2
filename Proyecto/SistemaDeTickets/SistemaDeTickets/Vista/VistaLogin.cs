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
            this.BackColor = Color.FromArgb(247, 247, 251);

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

            SetPasswordVisibility(checkPassword.Checked);
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

                MessageBox.Show($"¡Bienvenido, {usuario.Nombre}!", "Inicio de Sesión Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔹 Verificar el rol del usuario usando enum
                if (usuario.Rol == RolUsuario.Admin)
                {
                    // Si es ADMIN → abrir formulario de gestión de eventos
                    var gestion = new VistaGestionEventos();
                    gestion.StartPosition = FormStartPosition.CenterScreen;
                    gestion.Show();
                }
                else // RolUsuario.Usuario
                {
                    // Si es USUARIO → navegar según el contexto
                    switch (ContextoOrigen)
                    {
                        case ContextoNavegacion.DesdeCompraEvento:
                            var ventanaEventos = new VistaEvento();
                            ventanaEventos.StartPosition = FormStartPosition.CenterScreen;

                            if (EventoSeleccionadoCache != null)
                            {
                                // Aquí podrías preseleccionar el evento, si lo implementas
                            }

                            ventanaEventos.Show();
                            break;

                        case ContextoNavegacion.DesdeRegistro:
                        case ContextoNavegacion.DesdeInicio:
                        default:
                            var ventanaEventosDefault = new VistaEvento();
                            ventanaEventosDefault.StartPosition = FormStartPosition.CenterScreen;
                            ventanaEventosDefault.Show();
                            break;
                    }
                }

                this.Hide(); // Oculta el login sin cerrarlo
            }
            else
            {
                // Mostrar error
                MessageBox.Show("Credenciales incorrectas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }


        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            // Botón "Registrarse": navegar a registro (estilo dispose() de Java)
            // Usar Hide() para mantener la aplicación viva
            var registroForm = new VistaRegistro();
            registroForm.StartPosition = FormStartPosition.CenterScreen;
            registroForm.Show();
            this.Hide(); // Oculta el login sin terminar la aplicación
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Navegación contextual según el origen
            switch (ContextoOrigen)
            {
                case ContextoNavegacion.DesdeCompraEvento:
                    // Si vino desde compra en VistaEvento, volver a VistaEvento
                    var ventanaEventos = new VistaEvento();
                    ventanaEventos.StartPosition = FormStartPosition.CenterScreen;
                    ventanaEventos.Show();
                    break;

                case ContextoNavegacion.DesdeRegistro:
                    // Si vino desde registro, volver a registro
                    var registroForm = new VistaRegistro();
                    registroForm.StartPosition = FormStartPosition.CenterScreen;
                    registroForm.Show();
                    break;

                case ContextoNavegacion.DesdeInicio:
                default:
                    // Por defecto, volver a inicio
                    var inicioForm = new Inicio();
                    inicioForm.StartPosition = FormStartPosition.CenterScreen;
                    inicioForm.Show();
                    break;
            }

            this.Hide(); // Oculta el login sin terminar la aplicación
        }

        private void SetPasswordVisibility(bool ocultar)
        {
            txtPassword.UseSystemPasswordChar = ocultar;
            checkPassword.Text = ocultar ? "Ocultar" : "Mostrar";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Solo usar Application.Exit() cuando el usuario realmente quiera cerrar todo
            Application.Exit();
        }

        private void linkOlvidePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Abrir formulario de recuperación de contraseña (modal, centrado)
            var recuperacionForm = new VistaRecuperacionPassword();
            recuperacionForm.StartPosition = FormStartPosition.CenterScreen;
            recuperacionForm.ShowDialog(); // Usar ShowDialog para ventana modal
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }

        private void checkPassword_CheckedChanged(object sender, EventArgs e)
        {
            SetPasswordVisibility(checkPassword.Checked);
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblCorreo_Click(object sender, EventArgs e)
        {

        }
    }
}
