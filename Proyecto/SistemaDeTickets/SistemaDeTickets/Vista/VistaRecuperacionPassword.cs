using System;
using System.Windows.Forms;
using SistemaDeTickets.Controlador;
using SistemaDeTickets.Utils;

namespace SistemaDeTickets.Vista
{
    /// <summary>
    /// Formulario para recuperación de contraseña: enviar token y resetear contraseña.
    /// </summary>
    public partial class VistaRecuperacionPassword : Form
    {
        private readonly ControladorAutenticacion _controladorAutenticacion;

        public VistaRecuperacionPassword()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            _controladorAutenticacion = new ControladorAutenticacion();

            // Estado inicial
            txtToken.Enabled = false;
            txtNuevoPassword.Enabled = false;
            btnRecuperar.Enabled = false;
            lblToken.Visible = false;

            // Eventos
            btnEnviarToken.Click += btnEnviarToken_Click;
            btnRecuperar.Click += btnRecuperar_Click;
            btnCancelar.Click += BtnCancelar_Click;
            btnCambiarContrasena.Click += btnCambiarContrasena_Click; // ← solicitado
            lblEmail.Click += lblEmail_Click;
            lblToken.Click += lblToken_Click;

            // Entrada segura
            if (txtNuevoPassword != null) txtNuevoPassword.UseSystemPasswordChar = true;

            // Enter dispara el cambio de contraseña
            AcceptButton = btnCambiarContrasena;
        }

        // Enviar token al correo
        private void btnEnviarToken_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Por favor, ingrese su correo electrónico.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidadorDatos.ValidarEmail(email))
            {
                MessageBox.Show("El formato del correo electrónico no es válido.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string token = _controladorAutenticacion.GenerarTokenRecuperacion(email);

                if (!string.IsNullOrEmpty(token))
                {
                    // Para pruebas: mostrar token. En producción: enviarlo por email.
                    MessageBox.Show($"Token generado: {token}\n\nCopia este token para completar la recuperación.",
                        "Token de Recuperación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtToken.Enabled = true;
                    txtNuevoPassword.Enabled = true;
                    btnRecuperar.Enabled = true;
                    btnCambiarContrasena.Enabled = true;
                    lblToken.Text = $"Token generado: {token}";
                    lblToken.Visible = true;
                    txtToken.Focus();
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado con ese correo.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al generar token",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Resetear contraseña usando token
        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            string token = txtToken.Text.Trim();
            string nuevaPassword = txtNuevoPassword.Text.Trim();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(nuevaPassword))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidadorDatos.ValidarPassword(nuevaPassword))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluir mayúsculas, minúsculas y números.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                bool exito = _controladorAutenticacion.ResetPassword(token, nuevaPassword);

                if (exito)
                {
                    MessageBox.Show("Contraseña cambiada exitosamente. Inicie sesión con su nueva contraseña.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtEmail.Clear();
                    txtToken.Clear();
                    txtNuevoPassword.Clear();

                    Hide();
                    var loginForm = new VistaLogin { StartPosition = FormStartPosition.CenterScreen };
                    loginForm.Show();
                }
                else
                {
                    MessageBox.Show("Token inválido o expirado.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cambiar contraseña",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Alias: el botón Cambiar Contraseña ejecuta la misma lógica que Recuperar
        private void btnCambiarContrasena_Click(object sender, EventArgs e)
        {
            btnRecuperar_Click(sender, e);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lblEmail_Click(object sender, EventArgs e) { }
        private void lblToken_Click(object sender, EventArgs e) { }
    }
}
