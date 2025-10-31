using SistemaDeTickets.Modelo;
using SistemaDeTickets.Services;
using SistemaDeTickets.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaDeTickets.Vista
{
    public partial class VistaHistorial : Form
    {
        private List<Compra> _historialCompras;

        public VistaHistorial()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema de Tickets - Mi Historial de Compras";
            this.BackColor = Color.FromArgb(247, 247, 251);

            ConfigurarInterfazHistorial();
            CargarHistorialCompras();
        }

        private void ConfigurarInterfazHistorial()
        {
            // Configurar título
            if (this.Controls.ContainsKey("lblTitulo"))
            {
                var lblTitulo = this.Controls["lblTitulo"] as Label;
                if (lblTitulo != null)
                {
                    lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                    lblTitulo.ForeColor = Color.FromArgb(30, 31, 59);
                    lblTitulo.Text = "MI HISTORIAL DE COMPRAS";
                    lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
                }
            }

            // Configurar DataGridView si existe
            if (this.Controls.ContainsKey("dgvHistorial"))
            {
                var dgv = this.Controls["dgvHistorial"] as DataGridView;
                if (dgv != null)
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.BorderStyle = BorderStyle.FixedSingle;
                    dgv.GridColor = Color.LightGray;
                    dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 31, 59);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgv.EnableHeadersVisualStyles = false;
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgv.ReadOnly = true;
                }
            }

            // Configurar botones
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Name.Contains("Descargar") || btn.Name.Contains("Recibo"))
                        ConfigurarBoton(btn, "Descargar Recibo", Color.FromArgb(76, 175, 80));
                    else if (btn.Name.Contains("Volver") || btn.Name.Contains("Cerrar"))
                        ConfigurarBoton(btn, "Volver", Color.FromArgb(158, 158, 158));
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
            btn.Width = 150;
            btn.Cursor = Cursors.Hand;
        }

        private void CargarHistorialCompras()
        {
            try
            {
                // Cargar compras desde JSON
                _historialCompras = GestorJSON.LeerArchivo<List<Compra>>("Data/Compras.json") ?? new List<Compra>();

                // Filtrar solo las compras del usuario actual
                if (ServicioAutenticacion.CurrentUser != null)
                {
                    _historialCompras = _historialCompras
                        .Where(c => c.UsuarioId == ServicioAutenticacion.CurrentUser.Id)
                        .OrderByDescending(c => c.FechaCompra)
                        .ToList();
                }

                // Mostrar en DataGridView si existe
                if (this.Controls.ContainsKey("dgvHistorial"))
                {
                    var dgv = this.Controls["dgvHistorial"] as DataGridView;
                    if (dgv != null)
                    {
                        // Crear DataTable para mejor presentación
                        DataTable dt = new DataTable();
                        dt.Columns.Add("ID Compra", typeof(int));
                        dt.Columns.Add("Fecha Compra", typeof(string));
                        dt.Columns.Add("Evento", typeof(string));
                        dt.Columns.Add("Cantidad", typeof(int));
                        dt.Columns.Add("Total", typeof(string));
                        dt.Columns.Add("Estado", typeof(string));

                        foreach (var compra in _historialCompras)
                        {
                            // Cargar información del evento (simplificado)
                            string nombreEvento = $"Evento #{compra.EventoId}"; // En implementación real, cargar desde eventos
                            string estado = "Completada"; // Estado por defecto para compras mostradas

                            dt.Rows.Add(
                                compra.Id,
                                compra.FechaCompra.ToString("dd/MM/yyyy HH:mm"),
                                nombreEvento,
                                compra.Cantidad,
                                $"₡{compra.PrecioTotal:N0}",
                                estado
                            );
                        }

                        dgv.DataSource = dt;
                    }
                }

                // Mostrar mensaje si no hay compras
                if (_historialCompras.Count == 0)
                {
                    MessageBox.Show("No tienes compras registradas aún.",
                        "Historial vacío", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el historial: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDescargarRecibo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Controls.ContainsKey("dgvHistorial"))
                {
                    var dgv = this.Controls["dgvHistorial"] as DataGridView;
                    if (dgv != null && dgv.SelectedRows.Count > 0)
                    {
                        int compraId = (int)dgv.SelectedRows[0].Cells["ID Compra"].Value;

                        // Buscar archivo PDF en Receipts
                        string rutaPDF = Path.Combine("Receipts", $"{compraId}.pdf");

                        if (File.Exists(rutaPDF))
                        {
                            // Intentar abrir el PDF
                            Process.Start(rutaPDF);
                            MessageBox.Show("Recibo abierto exitosamente.",
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("El recibo no está disponible. Puede haber un problema con el archivo.",
                                "Recibo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor, selecciona una compra del historial.",
                            "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el recibo: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Volver a vista de eventos
            var eventosForm = new VistaEvento();
            eventosForm.StartPosition = FormStartPosition.CenterScreen;
            eventosForm.Show();
            this.Hide();
        }
    }
}
