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
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar ventana en pantalla
            _controladorAutenticacion = new ControladorAutenticacion();
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

            if (password != confirmarPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var usuario = _controladorAutenticacion.Registrar(nombre, email, password, Modelo.RolUsuario.Usuario);
                MessageBox.Show($"Usuario {usuario.Nombre} registrado exitosamente. Ahora puede iniciar sesión.", "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Navegar a login: ocultar ventana anterior (estilo dispose() de Java) y centrar nueva
                // Usar Hide() para mantener la aplicación viva y permitir navegación fluida
                var loginForm = new VistaLogin();
                loginForm.StartPosition = FormStartPosition.CenterScreen;
                loginForm.Show();
                this.Hide(); // Oculta la ventana actual sin terminar la aplicación
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Volver a la pantalla de inicio: ocultar ventana anterior (estilo dispose() de Java) y centrar nueva
            // Usar Hide() para mantener la aplicación viva y permitir navegación fluida
            var inicioForm = new Inicio();
            inicioForm.StartPosition = FormStartPosition.CenterScreen;
            inicioForm.Show();
            this.Hide(); // Oculta la ventana actual sin terminar la aplicación
        }
    }
}
