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
using SistemaDeTickets.Controlador.Patrones;
using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;

namespace SistemaDeTickets.Vista
{
    /// <summary>
    /// Formulario para procesar compras de tickets.
    /// Incluye validación de datos de pago y confirmación de compra.
    /// </summary>
    public partial class VistaCompra : Form
    {
        private DetalleCompra _detalleCompra;
        private FachadaCompraTique _fachadaCompra;

        public VistaCompra(DetalleCompra detalleCompra)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar ventana en pantalla
            _detalleCompra = detalleCompra;
            _fachadaCompra = new FachadaCompraTique(
                new ControladorUsuario(),
                new ControladorCompra(),
                new GestorInventario(),
                new GestorEventos()
            );

            CargarDatosCompra();
        }

        private void CargarDatosCompra()
        {
            lblNombreEvento.Text = _detalleCompra.Evento.Nombre;
            lblFecha.Text = _detalleCompra.Evento.Fecha.ToString("dd/MM/yyyy");
            lblCantidad.Text = _detalleCompra.Cantidad.ToString();
            lblTotal.Text = $"₡{_detalleCompra.Total:N0}";
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            string numeroTarjeta = txtNumeroTarjeta.Text.Trim();
            string cvv = txtCVV.Text.Trim();
            string fechaExpiracion = txtFechaExpiracion.Text.Trim();
            string nombreTitular = txtNombreTitular.Text.Trim();

            // Validaciones
            if (string.IsNullOrEmpty(numeroTarjeta) || string.IsNullOrEmpty(cvv) ||
                string.IsNullOrEmpty(fechaExpiracion) || string.IsNullOrEmpty(nombreTitular))
            {
                MessageBox.Show("Por favor, complete todos los campos de pago.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidadorTarjeta.ValidarNumero(numeroTarjeta))
            {
                MessageBox.Show("Número de tarjeta inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidadorTarjeta.ValidarCVV(cvv))
            {
                MessageBox.Show("CVV inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidadorTarjeta.ValidarFechaExpiracion(fechaExpiracion))
            {
                MessageBox.Show("Fecha de expiración inválida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidadorTarjeta.ValidarNombreTitular(nombreTitular))
            {
                MessageBox.Show("Nombre del titular inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Procesar pago
                bool pagoExitoso = _fachadaCompra.ProcesarPago(numeroTarjeta, cvv, _detalleCompra);

                if (pagoExitoso)
                {
                    // Confirmar compra
                    Compra compraConfirmada = _fachadaCompra.ConfirmarCompra(_detalleCompra);

                    MessageBox.Show("Compra realizada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Navegar a confirmación: ocultar ventana anterior (estilo dispose() de Java) y centrar nueva
                    // Usar Hide() para mantener la aplicación viva y permitir navegación fluida
                    var confirmacionForm = new VistaConfirmacion();
                    confirmacionForm.StartPosition = FormStartPosition.CenterScreen;
                    confirmacionForm.Show();
                    this.Hide(); // Oculta la ventana actual sin terminar la aplicación
                }
                else
                {
                    MessageBox.Show("Error al procesar el pago.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la compra: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Cancelar compra y volver
            _fachadaCompra.CancelarCompra(0); // TODO: Obtener ID de compra real
            this.Close();
        }
    }
}
