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

namespace SistemaDeTickets.Vista
{
    /// <summary>
    /// Formulario de login del sistema.
    /// Maneja autenticación de usuarios y navegación a recuperación de contraseña.
    /// </summary>
    public partial class VistaLogin : Form
    {
        private ControladorAutenticacion _controladorAutenticacion;

        public VistaLogin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar ventana en pantalla
            _controladorAutenticacion = new ControladorAutenticacion();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var usuario = _controladorAutenticacion.IniciarSesion(email, password);
                MessageBox.Show($"Bienvenido, {usuario.Nombre}!", "Login exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Navegar al panel principal del usuario (estilo dispose() de Java)
                // Usar Hide() para mantener la aplicación viva y permitir navegación fluida
                var inicioForm = new Inicio();
                inicioForm.StartPosition = FormStartPosition.CenterScreen;
                inicioForm.Show();
                this.Hide(); // Oculta el login sin terminar la aplicación
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // NO cerrar la ventana en caso de error - permitir reintento
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
            // Botón "Volver": regresar a la ventana de inicio (estilo dispose() de Java)
            // Usar Hide() para mantener la aplicación viva
            var inicioForm = new Inicio();
            inicioForm.StartPosition = FormStartPosition.CenterScreen;
            inicioForm.Show();
            this.Hide(); // Oculta el login sin terminar la aplicación
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
    }
}
