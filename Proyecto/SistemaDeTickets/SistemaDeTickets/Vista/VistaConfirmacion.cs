using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using SistemaDeTickets.Services;

namespace SistemaDeTickets.Vista
{
    /// <summary>
    /// Formulario de confirmación de compra con funcionalidades completas
    /// </summary>
    public partial class VistaConfirmacion : Form
    {
        private Compra _compraConfirmada;
        private Modelo.Evento _evento;
        private Usuario _usuario;

        public VistaConfirmacion()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Constructor con datos de compra para mostrar información completa
        /// </summary>
        public VistaConfirmacion(Compra compra, Modelo.Evento evento, Usuario usuario) : this()
        {
            _compraConfirmada = compra;
            _evento = evento;
            _usuario = usuario;

            MostrarDetallesCompra();
        }

        private void MostrarDetallesCompra()
        {
            if (_compraConfirmada != null && _evento != null && _usuario != null)
            {
                // Mostrar información de la compra (usando MessageBox para demo ya que no hay labels)
                string mensaje = $"Compra #{_compraConfirmada.Id} realizada exitosamente!\n\n" +
                                $"Usuario: {_usuario.Nombre} ({_usuario.Email})\n" +
                                $"Evento: {_evento.Nombre}\n" +
                                $"Fecha del Evento: {_evento.Fecha:dd/MM/yyyy HH:mm}\n" +
                                $"Recinto: {_evento.Recinto}\n" +
                                $"Cantidad: {_compraConfirmada.Cantidad}\n" +
                                $"Precio Unitario: {_evento.Precio:C}\n" +
                                $"TOTAL: {_compraConfirmada.PrecioTotal:C}\n" +
                                $"Fecha de Compra: {_compraConfirmada.FechaCompra:dd/MM/yyyy HH:mm}";

                MessageBox.Show(mensaje, "Compra Confirmada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Habilitar botones (si existen en el designer)
                // btnDescargarRecibo.Enabled = true;
                // btnVerHistorial.Enabled = true;
            }
            else
            {
                // Estado de error o compra no encontrada
                MessageBox.Show("Error: No se pudo cargar la información de la compra",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // btnDescargarRecibo.Enabled = false;
                // btnVerHistorial.Enabled = false;
            }
        }

        private void btnDescargarRecibo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_compraConfirmada == null || _evento == null || _usuario == null)
                {
                    MessageBox.Show("No hay información de compra para generar el recibo.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Generar PDF del recibo
                byte[] pdfBytes = GeneradorPDF.GenerarRecibo(_compraConfirmada, _evento, _usuario);

                // Guardar PDF en el directorio Receipts
                GeneradorPDF.GuardarReciboPDF(pdfBytes, _compraConfirmada.Id);

                // Mostrar diálogo de guardado
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                    saveDialog.FileName = $"Recibo_Compra_{_compraConfirmada.Id}.pdf";
                    saveDialog.Title = "Guardar Recibo de Compra";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllBytes(saveDialog.FileName, pdfBytes);
                        MessageBox.Show("Recibo guardado exitosamente.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el recibo: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para manejar el evento del botón (si existe en el designer)
        private void btnDescargarRecibo_EventHandler(object sender, EventArgs e)
        {
            btnDescargarRecibo_Click(sender, e);
        }

        private void btnVerHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                // Abrir formulario de historial de compras
                var historialForm = new VistaHistorial();
                historialForm.StartPosition = FormStartPosition.CenterScreen;
                historialForm.Show();

                // Cerrar esta ventana
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el historial: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            // Cerrar aplicación o volver al menú principal
            DialogResult result = MessageBox.Show(
                "¿Desea volver al menú principal?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Volver al menú principal (VistaEvento)
                var eventoForm = new VistaEvento();
                eventoForm.StartPosition = FormStartPosition.CenterScreen;
                eventoForm.Show();
                this.Hide();
            }
        }

        private void VistaConfirmacion_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si el usuario cierra la ventana, mostrar confirmación
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro que desea cerrar la aplicación?",
                    "Confirmar salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
