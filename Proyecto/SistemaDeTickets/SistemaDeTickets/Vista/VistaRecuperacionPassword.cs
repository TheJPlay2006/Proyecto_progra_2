using System;
using System.Windows.Forms;
using SistemaDeTickets.Controlador;
using SistemaDeTickets.Utils;
using SistemaDeTickets.Services;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

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
                MessageBox.Show("Por favor, ingrese su correo electrónico.", "Campo Requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidadorDatos.ValidarEmail(email))
            {
                MessageBox.Show("El formato del correo electrónico no es válido.", "Email Inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string token = ServicioAutenticacion.GenerarTokenRecuperacion(email);

                if (!string.IsNullOrEmpty(token))
                {
                    // Generar token seguro con SHA-256
                    string tokenHash = GenerarHashSHA256(token);

                    // Crear archivo .txt en escritorio con token y expiración
                    string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string nombreArchivo = $"Token_Recuperacion_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                    string rutaCompleta = Path.Combine(rutaEscritorio, nombreArchivo);

                    string contenidoArchivo = $"TOKEN DE RECUPERACIÓN DE CONTRASEÑA\n" +
                                            $"===================================\n\n" +
                                            $"Email: {email}\n" +
                                            $"Token: {token}\n" +
                                            $"Hash SHA-256: {tokenHash}\n" +
                                            $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm:ss}\n" +
                                            $"Expira en: 30 minutos\n\n" +
                                            $"INSTRUCCIONES:\n" +
                                            $"1. Copia el token mostrado arriba\n" +
                                            $"2. Pégalo en el campo 'Token'\n" +
                                            $"3. Ingresa tu nueva contraseña\n" +
                                            $"4. Haz clic en 'Cambiar Contraseña'\n\n" +
                                            $"Este archivo se eliminará automáticamente cuando uses el token o expire.";

                    File.WriteAllText(rutaCompleta, contenidoArchivo);

                    // Mostrar mensaje de confirmación
                    MessageBox.Show($"Token generado exitosamente.\n\n" +
                                  $"Se ha creado un archivo en tu escritorio:\n{nombreArchivo}\n\n" +
                                  $"El archivo contiene el token y expirará en 30 minutos.\n" +
                                  $"Copia el token del archivo para continuar.",
                        "Token Generado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Programar eliminación automática del archivo después de 30 minutos
                    System.Windows.Forms.Timer timerEliminacion = new System.Windows.Forms.Timer();
                    timerEliminacion.Interval = 30 * 60 * 1000; // 30 minutos
                    timerEliminacion.Tick += (s, args) =>
                    {
                        try
                        {
                            if (File.Exists(rutaCompleta))
                                File.Delete(rutaCompleta);
                        }
                        catch { }
                        timerEliminacion.Stop();
                        timerEliminacion.Dispose();
                    };
                    timerEliminacion.Start();

                    // Habilitar campos para continuar
                    txtToken.Enabled = true;
                    txtNuevoPassword.Enabled = true;
                    btnRecuperar.Enabled = true;
                    btnCambiarContrasena.Enabled = true;
                    lblToken.Text = $"Archivo creado en escritorio: {nombreArchivo}";
                    lblToken.Visible = true;
                    txtToken.Focus();
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado con ese correo electrónico.", "Usuario No Encontrado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar token: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Genera hash SHA-256 del token para verificación adicional
        /// </summary>
        private string GenerarHashSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        // Resetear contraseña usando token
        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            string token = txtToken.Text.Trim();
            string nuevaPassword = txtNuevoPassword.Text.Trim();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(nuevaPassword))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Campos Requeridos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidadorDatos.ValidarPassword(nuevaPassword))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluir mayúsculas, minúsculas y números.", "Contraseña Débil",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool exito = ServicioAutenticacion.ResetPassword(token, nuevaPassword);

                if (exito)
                {
                    MessageBox.Show("¡Contraseña cambiada exitosamente!\n\n" +
                                  "Ahora puedes iniciar sesión con tu nueva contraseña.", "Contraseña Actualizada",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Limpiar campos
                    txtEmail.Clear();
                    txtToken.Clear();
                    txtNuevoPassword.Clear();

                    // Cerrar formulario de recuperación y abrir login
                    this.Close();
                    var loginForm = new VistaLogin { StartPosition = FormStartPosition.CenterScreen };
                    loginForm.Show();
                }
                else
                {
                    MessageBox.Show("Token inválido o expirado.\n\n" +
                                  "Verifica que el token sea correcto y no haya expirado (30 minutos máximo).\n" +
                                  "Si necesitas un nuevo token, solicita uno nuevamente.", "Token Inválido",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtToken.Focus();
                    txtToken.SelectAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar contraseña: {ex.Message}", "Error",
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
