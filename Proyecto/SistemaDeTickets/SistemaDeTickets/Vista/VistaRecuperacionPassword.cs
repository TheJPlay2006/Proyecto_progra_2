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
    /// Formulario para recuperación de contraseña.
    /// Permite enviar token por correo y resetear contraseña.
    /// </summary>
    public partial class VistaRecuperacionPassword : Form
    {
        private ControladorAutenticacion _controladorAutenticacion;

        public VistaRecuperacionPassword()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar ventana en pantalla
            _controladorAutenticacion = new ControladorAutenticacion();
        }

        /// <summary>
        /// Evento para enviar token de recuperación al correo ingresado.
        /// Genera token, lo almacena en el usuario y lo envía por correo.
        /// </summary>
        private void btnEnviarToken_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Por favor, ingrese su correo electrónico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidadorDatos.ValidarEmail(email))
            {
                MessageBox.Show("El formato del correo electrónico no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Generar token (para pruebas locales - sin envío de email)
            string token = _controladorAutenticacion.GenerarTokenRecuperacion(email);

            if (token != null)
            {
                // Mostrar token directamente en pantalla para pruebas locales
                // NOTA: En producción, aquí se enviaría por email/SMTP
                MessageBox.Show($"Token generado: {token}\n\nCopia este token para completar la recuperación.\n\nNota: Esta es una versión de prueba local sin envío de email.", "Token de Recuperación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtToken.Enabled = true;
                txtNuevoPassword.Enabled = true;
                btnRecuperar.Enabled = true;

                // Opcional: Mostrar token en un label visible para copiar fácilmente
                lblToken.Text = $"Token generado: {token}";
                lblToken.Visible = true;
            }
            else
            {
                MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento para resetear la contraseña usando el token.
        /// Valida token y expiración, luego actualiza la contraseña.
        /// </summary>
        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            string token = txtToken.Text.Trim();
            string nuevaPassword = txtNuevoPassword.Text.Trim();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(nuevaPassword))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidadorDatos.ValidarPassword(nuevaPassword))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluir mayúsculas, minúsculas y números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar token y resetear contraseña
            bool exito = _controladorAutenticacion.ResetPassword(token, nuevaPassword);

            if (exito)
            {
                MessageBox.Show("Contraseña cambiada exitosamente. Inicie sesión con su nueva contraseña.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Navegar de vuelta al login: ocultar ventana anterior (estilo dispose() de Java) y centrar nueva
                // Usar Hide() para mantener la aplicación viva y permitir navegación fluida
                this.Hide(); // Oculta la ventana modal sin terminar la aplicación
                var loginForm = new VistaLogin();
                loginForm.StartPosition = FormStartPosition.CenterScreen;
                loginForm.Show(); // Muestra la nueva ventana
            }
            else
            {
                MessageBox.Show("Token inválido o expirado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
