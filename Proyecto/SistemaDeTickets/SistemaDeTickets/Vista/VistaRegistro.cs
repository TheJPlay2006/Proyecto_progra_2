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
using SistemaDeTickets.Utils;

namespace SistemaDeTickets.Vista
{
    /// <summary>
    /// Formulario de registro de nuevos usuarios.
    /// Permite crear cuentas con validación de datos y hash de contraseñas.
    /// </summary>
    public partial class VistaRegistro : Form
    {
        private ControladorAutenticacion _controladorAutenticacion;

        public VistaRegistro()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema de Tickets - Registro de Usuario";
            this.BackColor = Color.FromArgb(247, 247, 251);

            _controladorAutenticacion = new ControladorAutenticacion();

            ConfigurarInterfazRegistro();

            // Configurar campos de contraseña con máscara por defecto
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmarPassword.UseSystemPasswordChar = true;

        }

        private void ConfigurarInterfazRegistro()
        {
            // Configurar título
            if (this.Controls.ContainsKey("lblTitulo"))
            {
                var lblTitulo = this.Controls["lblTitulo"] as Label;
                if (lblTitulo != null)
                {
                    lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                    lblTitulo.ForeColor = Color.FromArgb(30, 31, 59);
                    lblTitulo.Text = "REGISTRO DE USUARIO";
                    lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
                }
            }

            // Configurar botones
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Name.Contains("Registrar"))
                        ConfigurarBoton(btn, "Crear Cuenta", Color.FromArgb(76, 175, 80));
                    else if (btn.Name.Contains("Volver"))
                        ConfigurarBoton(btn, "Volver", Color.FromArgb(158, 158, 158));
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
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Height = 40;
            btn.Cursor = Cursors.Hand;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmarPassword = txtConfirmarPassword.Text;

            // Validaciones
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmarPassword))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidadorDatos.ValidarEmail(email))
            {
                MessageBox.Show("El formato del correo electrónico no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidadorDatos.ValidarPassword(password))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluir mayúsculas, minúsculas y números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar fortaleza mínima de contraseña
            var fortaleza = CalcularFortalezaPassword(password);
            if (fortaleza == PasswordStrength.Weak)
            {
                MessageBox.Show("La contraseña es demasiado débil. Debe ser al menos de fortaleza 'Media' para registrarse.", "Contraseña Débil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmarPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var usuario = _controladorAutenticacion.Registrar(nombre, email, password, Modelo.RolUsuario.Usuario);
                MessageBox.Show($"Usuario {usuario.Nombre} registrado exitosamente. Ahora puede iniciar sesión.", "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Navegar a login: ocultar ventana anterior y centrar nueva
                this.Hide();
                var loginForm = new VistaLogin();
                loginForm.ContextoOrigen = VistaLogin.ContextoNavegacion.DesdeRegistro; // Indicar que viene desde registro
                loginForm.StartPosition = FormStartPosition.CenterScreen;
                loginForm.ShowDialog();
                this.Show(); // Mostrar registro nuevamente cuando se cierre login
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Volver a la pantalla de inicio
            this.Hide();
            var inicioForm = new Inicio();
            inicioForm.StartPosition = FormStartPosition.CenterScreen;
            inicioForm.ShowDialog();
            this.Show(); // Mostrar registro nuevamente cuando se cierre inicio
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text.Length == 0)
            {
                progressBarContrasena.Value = 0;
                lblFortalezaContrasena.Text = "";
                progressBarContrasena.BackColor = SystemColors.Control;
                return;
            }
            EvaluarFortalezaPassword(txtPassword.Text, true);
        }

        private void txtConfirmarPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtConfirmarPassword.Text.Length == 0)
            {
                progressBarConfirmarContrasena.Value = 0;
                lblFortalezaConfirmarContrasena.Text = "";
                progressBarConfirmarContrasena.BackColor = SystemColors.Control;
                return;
            }
            EvaluarFortalezaPassword(txtConfirmarPassword.Text, false);
        }

        /// <summary>
        /// Evalúa la fortaleza de la contraseña y actualiza la barra de progreso
        /// </summary>
        private void EvaluarFortalezaPassword(string password, bool esPasswordPrincipal)
        {
            // Evaluar fortaleza
            var fortaleza = CalcularFortalezaPassword(password);

            // Actualizar barra de progreso correspondiente
            if (esPasswordPrincipal)
            {
                ActualizarProgressBar(progressBarContrasena, fortaleza);
                ActualizarLabelFortaleza(lblFortalezaContrasena, fortaleza);
            }
            else
            {
                ActualizarProgressBar(progressBarConfirmarContrasena, fortaleza);
                ActualizarLabelFortaleza(lblFortalezaConfirmarContrasena, fortaleza);
            }
        }

        /// <summary>
        /// Calcula la fortaleza de una contraseña
        /// </summary>
        private PasswordStrength CalcularFortalezaPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 4)
                return PasswordStrength.Weak;

            bool hasLower = password.Any(char.IsLower);
            bool hasUpper = password.Any(char.IsUpper);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            int score = 0;
            if (hasLower) score++;
            if (hasUpper) score++;
            if (hasDigit) score++;
            if (hasSpecial) score++;
            if (password.Length >= 8) score++;
            if (password.Length >= 12) score++;

            if (score <= 2) return PasswordStrength.Weak;
            if (score <= 4) return PasswordStrength.Medium;
            return PasswordStrength.Strong;
        }

        /// <summary>
        /// Actualiza la barra de progreso según la fortaleza
        /// </summary>
        private void ActualizarProgressBar(ProgressBar progressBar, PasswordStrength fortaleza)
        {
            switch (fortaleza)
            {
                case PasswordStrength.Weak:
                    progressBar.Value = 33;
                    progressBar.BackColor = Color.Red;
                    break;

                case PasswordStrength.Medium:
                    progressBar.Value = 66;
                    progressBar.BackColor = Color.Orange;
                    break;

                case PasswordStrength.Strong:
                    progressBar.Value = 100;
                    progressBar.BackColor = Color.Green;
                    break;
            }
        }

        /// <summary>
        /// Actualiza el label de fortaleza según la fortaleza
        /// </summary>
        private void ActualizarLabelFortaleza(Label label, PasswordStrength fortaleza)
        {
            switch (fortaleza)
            {
                case PasswordStrength.Weak:
                    label.Text = "Débil";
                    label.ForeColor = Color.Red;
                    break;

                case PasswordStrength.Medium:
                    label.Text = "Media";
                    label.ForeColor = Color.Orange;
                    break;

                case PasswordStrength.Strong:
                    label.Text = "Fuerte";
                    label.ForeColor = Color.Green;
                    break;
            }
        }

        /// <summary>
        /// Maneja el evento de mostrar/ocultar contraseña para ambos campos
        /// </summary>
        private void btnOcultarVerContra_Click(object sender, EventArgs e)
        {
            // Alternar visibilidad para ambos campos de contraseña
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            txtConfirmarPassword.UseSystemPasswordChar = !txtConfirmarPassword.UseSystemPasswordChar;

            // Cambiar ícono del botón basado en el nuevo estado
            // Si UseSystemPasswordChar = true (oculto), mostrar 👁 (quiero ver)
            // Si UseSystemPasswordChar = false (visible), mostrar 🙈 (quiero ocultar)
            btnOcultarVerContra.Text = txtPassword.UseSystemPasswordChar ? "👁" : "🙈";
        }

        /// <summary>
        /// Enum para representar la fortaleza de la contraseña
        /// </summary>
        private enum PasswordStrength
        {
            Weak,
            Medium,
            Strong
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }
    }
}
