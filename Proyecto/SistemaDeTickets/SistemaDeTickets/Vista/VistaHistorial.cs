using SistemaDeTickets.Controlador;
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

            // Conectar eventos de botones después de InitializeComponent
            ConectarEventosBotones();
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
            if (this.Controls.ContainsKey("dataGridViewCompras"))
            {
                var dgv = this.Controls["dataGridViewCompras"] as DataGridView;
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

        /// <summary>
        /// Conecta los eventos Click de los botones después de InitializeComponent
        /// </summary>
        private void ConectarEventosBotones()
        {
            // Conectar eventos de botones (ya están conectados en el Designer, pero por seguridad)
            if (btnDescargarRecibo != null)
                btnDescargarRecibo.Click += btnDescargarRecibo_Click;

            if (btnVolver != null)
                btnVolver.Click += btnVolver_Click;
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
                if (this.Controls.ContainsKey("dataGridViewCompras"))
                {
                    var dgv = this.Controls["dataGridViewCompras"] as DataGridView;
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
                            // Cargar información del evento desde el repositorio
                            var repoEventos = new RepositorioEventos();
                            var evento = repoEventos.BuscarPorId(compra.EventoId);
                            string nombreEvento = evento != null ? evento.Nombre : $"Evento #{compra.EventoId}";
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

                        // Limpiar datos anteriores y asignar nuevos
                        dgv.DataSource = null;
                        dgv.DataSource = dt;

                        // Configurar propiedades del DataGridView
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dgv.ReadOnly = true;
                        dgv.AllowUserToAddRows = false;
                        dgv.AllowUserToDeleteRows = false;
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
                if (this.Controls.ContainsKey("dataGridViewCompras"))
                {
                    var dgv = this.Controls["dataGridViewCompras"] as DataGridView;
                    if (dgv != null && dgv.SelectedRows.Count > 0)
                    {
                        int compraId = (int)dgv.SelectedRows[0].Cells["ID Compra"].Value;

                        // Buscar la compra completa en la lista
                        var compraSeleccionada = _historialCompras.FirstOrDefault(c => c.Id == compraId);
                        if (compraSeleccionada == null)
                        {
                            MessageBox.Show("No se pudo encontrar la información de la compra.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Obtener datos relacionados
                        var repoUsuarios = new RepositorioUsuarios();
                        var repoEventos = new RepositorioEventos();
                        var usuario = repoUsuarios.BuscarPorId(compraSeleccionada.UsuarioId);
                        var evento = repoEventos.BuscarPorId(compraSeleccionada.EventoId);

                        if (usuario == null || evento == null)
                        {
                            MessageBox.Show("No se pudieron obtener los datos completos de la compra.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Generar PDF del recibo
                        string rutaPdf = GeneradorPDF.GenerarRecibo(compraSeleccionada, evento, usuario);

                        // Mostrar diálogo para guardar
                        using (SaveFileDialog saveDialog = new SaveFileDialog())
                        {
                            saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                            saveDialog.FileName = $"Recibo_Compra_{compraSeleccionada.Id}.pdf";
                            saveDialog.Title = "Guardar Recibo de Compra";
                            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                            if (saveDialog.ShowDialog() == DialogResult.OK)
                            {
                                // Copiar el PDF generado a la ubicación seleccionada
                                System.IO.File.Copy(rutaPdf, saveDialog.FileName, true);

                                // También guardar automáticamente en Receipts/
                                try
                                {
                                    string receiptsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Receipts");
                                    if (!Directory.Exists(receiptsDir))
                                        Directory.CreateDirectory(receiptsDir);

                                    string autoSavePath = Path.Combine(receiptsDir, $"{compraSeleccionada.Id}.pdf");
                                    File.Copy(rutaPdf, autoSavePath, true);
                                }
                                catch
                                {
                                    // Si falla el guardado automático, continuar
                                }

                                MessageBox.Show($"Recibo guardado exitosamente en:\n{saveDialog.FileName}",
                                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Abrir el archivo automáticamente
                                try
                                {
                                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                    {
                                        FileName = saveDialog.FileName,
                                        UseShellExecute = true
                                    });
                                }
                                catch
                                {
                                    // Si no se puede abrir automáticamente, mostrar mensaje
                                    MessageBox.Show("El recibo se guardó correctamente, pero no se pudo abrir automáticamente.",
                                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
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
                MessageBox.Show($"Error al descargar el recibo: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Volver a vista de eventos
            this.Hide();
            var eventosForm = new VistaEvento();
            eventosForm.StartPosition = FormStartPosition.CenterScreen;
            eventosForm.ShowDialog();
            if (!this.IsDisposed)
            {
                this.Close(); // Cerrar historial después de volver
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            // Implementar funcionalidad básica de filtrado
            // Recargar todas las compras del usuario (simulando quitar filtros)
            CargarHistorialCompras();
            MessageBox.Show("Se han recargado todas las compras. Funcionalidad de filtrado avanzado próximamente disponible.",
                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
