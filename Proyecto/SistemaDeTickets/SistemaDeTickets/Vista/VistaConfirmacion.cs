using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using SistemaDeTickets.Services;
using SistemaDeTickets.Controlador;

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
            this.Text = "Sistema de Tickets - Confirmación de Compra";
            this.BackColor = Color.FromArgb(247, 247, 251);

            // Conectar eventos de botones después de InitializeComponent
            ConectarEventosBotones();

            ConfigurarInterfazConfirmacion();
        }

        private void ConfigurarInterfazConfirmacion()
        {
            // Configurar título
            if (this.Controls.ContainsKey("lblTitulo"))
            {
                var lblTitulo = this.Controls["lblTitulo"] as Label;
                if (lblTitulo != null)
                {
                    lblTitulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
                    lblTitulo.ForeColor = Color.FromArgb(76, 175, 80);
                    lblTitulo.Text = "¡COMPRA CONFIRMADA!";
                    lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
                }
            }

            // Configurar botones
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Name.Contains("Descargar") || btn.Name.Contains("Recibo"))
                        ConfigurarBoton(btn, "Descargar Recibo PDF", Color.FromArgb(76, 175, 80));
                    else if (btn.Name.Contains("VerHistorial") || btn.Name.Contains("Historial"))
                        ConfigurarBoton(btn, "Ver Mi Historial", Color.FromArgb(48, 63, 159));
                    else if (btn.Name.Contains("Cerrar") || btn.Name.Contains("Salir"))
                        ConfigurarBoton(btn, "Volver al Inicio", Color.FromArgb(158, 158, 158));
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
            btn.Width = 180;
            btn.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Conecta los eventos Click de los botones después de InitializeComponent
        /// </summary>
        private void ConectarEventosBotones()
        {
            // Conectar eventos de botones
            if (btnDescargarRecibo != null)
                btnDescargarRecibo.Click += btnDescargarRecibo_Click;

            if (btnRealizarOtraCompra != null)
                btnRealizarOtraCompra.Click += btnRealizarOtraCompra_Click;

            if (btnVolver != null)
                btnVolver.Click += btnVolver_Click;
        }

        /// <summary>
        /// Carga los datos de la compra confirmada en la interfaz
        /// </summary>
        private void CargarDatosCompra(Compra compraConfirmada)
        {
            try
            {
                // Buscar datos relacionados usando repositorios
                var repoUsuarios = new RepositorioUsuarios();
                var repoEventos = new RepositorioEventos();
                var usuario = repoUsuarios.BuscarPorId(compraConfirmada.UsuarioId);
                var evento = repoEventos.BuscarPorId(compraConfirmada.EventoId);

                // Mostrar información en los labels
                if (this.Controls.ContainsKey("lblMensajeExito"))
                {
                    var lblMensaje = this.Controls["lblMensajeExito"] as Label;
                    if (lblMensaje != null)
                    {
                        lblMensaje.Text = $"¡Compra confirmada exitosamente!\nID: {compraConfirmada.Id}";
                        lblMensaje.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                        lblMensaje.ForeColor = Color.FromArgb(76, 175, 80);
                    }
                }

                if (this.Controls.ContainsKey("lblCodigoCompra"))
                {
                    var lblCodigo = this.Controls["lblCodigoCompra"] as Label;
                    if (lblCodigo != null)
                    {
                        lblCodigo.Text = $"Código: {compraConfirmada.Id}";
                        lblCodigo.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    }
                }

                if (this.Controls.ContainsKey("lblDetallesCompra"))
                {
                    var lblDetalles = this.Controls["lblDetallesCompra"] as Label;
                    if (lblDetalles != null)
                    {
                        lblDetalles.Text = $"{evento?.Nombre ?? "Evento"}\n" +
                                         $"{compraConfirmada.Cantidad} ticket(s)\n" +
                                         $"Total: ₡{compraConfirmada.PrecioTotal:N0}";
                        lblDetalles.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    }
                }

                // Guardar referencias para el PDF
                _compraConfirmada = compraConfirmada;
                _evento = evento;
                _usuario = usuario;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos de la compra: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Constructor con objeto Compra - busca datos relacionados automáticamente
        /// </summary>
        public VistaConfirmacion(Compra compraConfirmada) : this()
        {
            _compraConfirmada = compraConfirmada;

            // Buscar datos relacionados usando repositorios
            var repoUsuarios = new RepositorioUsuarios();
            var repoEventos = new RepositorioEventos();
            _usuario = repoUsuarios.BuscarPorId(_compraConfirmada.UsuarioId);
            _evento = repoEventos.BuscarPorId(_compraConfirmada.EventoId);

            MostrarDetallesCompra();
        }

        private void MostrarDetallesCompra()
        {
            if (_compraConfirmada != null && _evento != null && _usuario != null)
            {
                // Llenar labels con información detallada de la compra
                if (this.Controls.ContainsKey("lblMensajeExito"))
                    this.Controls["lblMensajeExito"].Text = "¡Compra realizada con éxito!";

                if (this.Controls.ContainsKey("lblCodigoCompra"))
                    this.Controls["lblCodigoCompra"].Text = $"Código: {_compraConfirmada.Id}";

                if (this.Controls.ContainsKey("lblDetallesCompra"))
                {
                    string detalles = $"Evento: {_evento.Nombre}\n" +
                                    $"Fecha: {_evento.Fecha:dd/MM/yyyy}\n" +
                                    $"Recinto: {_evento.Recinto}\n" +
                                    $"Tipo: {_evento.Tipo}\n" +
                                    $"Cantidad: {_compraConfirmada.Cantidad}\n" +
                                    $"Precio Unitario: {_evento.Precio:C}\n" +
                                    $"TOTAL: {_compraConfirmada.PrecioTotal:C}\n" +
                                    $"Fecha de Compra: {_compraConfirmada.FechaCompra:dd/MM/yyyy HH:mm}\n" +
                                    $"Usuario: {_usuario.Nombre} ({_usuario.Email})";

                    this.Controls["lblDetallesCompra"].Text = detalles;
                }

                // Habilitar botones funcionales
                if (this.Controls.ContainsKey("btnDescargarRecibo"))
                    this.Controls["btnDescargarRecibo"].Enabled = true;

                if (this.Controls.ContainsKey("btnVerHistorial"))
                    this.Controls["btnVerHistorial"].Enabled = true;

                if (this.Controls.ContainsKey("btnRealizarOtraCompra"))
                    this.Controls["btnRealizarOtraCompra"].Enabled = true;
            }
            else
            {
                // Estado de error o compra no encontrada
                if (this.Controls.ContainsKey("lblMensajeExito"))
                    this.Controls["lblMensajeExito"].Text = "Error: No se pudo cargar la información de la compra";

                // Deshabilitar botones
                if (this.Controls.ContainsKey("btnDescargarRecibo"))
                    this.Controls["btnDescargarRecibo"].Enabled = false;

                if (this.Controls.ContainsKey("btnVerHistorial"))
                    this.Controls["btnVerHistorial"].Enabled = false;

                if (this.Controls.ContainsKey("btnRealizarOtraCompra"))
                    this.Controls["btnRealizarOtraCompra"].Enabled = false;
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

                // Generar PDF del recibo usando GeneradorPDF
                string rutaPdf = GeneradorPDF.GenerarRecibo(_compraConfirmada, _evento, _usuario);
                byte[] pdfBytes = File.ReadAllBytes(rutaPdf);

                // Mostrar diálogo para que el usuario elija carpeta y nombre
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                    saveDialog.FileName = $"Recibo_Compra_{_compraConfirmada.Id}.pdf";
                    saveDialog.Title = "Guardar Recibo de Compra";
                    saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Copiar el PDF generado a la ubicación seleccionada por el usuario
                        System.IO.File.Copy(rutaPdf, saveDialog.FileName, true);

                        // También guardar automáticamente en Receipts/ (opcional)
                        try
                        {
                            string receiptsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Receipts");
                            if (!Directory.Exists(receiptsDir))
                                Directory.CreateDirectory(receiptsDir);

                            string autoSavePath = Path.Combine(receiptsDir, $"{_compraConfirmada.Id}.pdf");
                            File.Copy(rutaPdf, autoSavePath, true);
                        }
                        catch
                        {
                            // Si falla el guardado automático, continuar
                        }

                        MessageBox.Show($"Recibo guardado exitosamente en:\n{saveDialog.FileName}",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Abrir el archivo automáticamente para que el usuario pueda verlo
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

        private void btnRealizarOtraCompra_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener el userId de la compra actual
                int userId = _compraConfirmada?.UsuarioId ?? ServicioAutenticacion.CurrentUser?.Id ?? 0;

                if (userId == 0)
                {
                    MessageBox.Show("No se pudo identificar al usuario. Inicie sesión nuevamente.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cerrar esta ventana primero para evitar conflictos
                this.Hide();

                // Crear nueva instancia de VistaEvento (listado de eventos) con refresco forzado
                var eventosForm = new VistaEvento();
                eventosForm.StartPosition = FormStartPosition.CenterScreen;
                eventosForm.ShowDialog();

                // Forzar refresco de VistaEvento después de la compra
                if (eventosForm != null && !eventosForm.IsDisposed)
                {
                    eventosForm.RefrescarVista();
                }

                // Cerrar completamente esta ventana después de que VistaEvento se cierre
                if (!this.IsDisposed)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar nueva compra: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                // Cerrar esta ventana
                this.Hide();

                // Abrir VistaEvento (lista de eventos) para que el usuario pueda elegir otro evento con refresco
                var eventosForm = new VistaEvento();
                eventosForm.StartPosition = FormStartPosition.CenterScreen;
                eventosForm.ShowDialog();

                // Forzar refresco de VistaEvento después de la compra
                if (eventosForm != null && !eventosForm.IsDisposed)
                {
                    eventosForm.RefrescarVista();
                }

                // Cerrar completamente esta ventana después de que VistaEvento se cierre
                if (!this.IsDisposed)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al volver al listado de eventos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VistaConfirmacion_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Limpiar handlers de eventos para evitar memory leaks
            if (btnDescargarRecibo != null)
                btnDescargarRecibo.Click -= btnDescargarRecibo_Click;

            if (btnRealizarOtraCompra != null)
                btnRealizarOtraCompra.Click -= btnRealizarOtraCompra_Click;

            if (btnVolver != null)
                btnVolver.Click -= btnVolver_Click;

            // Si el usuario cierra la ventana con X, mostrar confirmación
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro que desea cerrar la ventana de confirmación?",
                    "Confirmar cierre",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                // Si dice Yes, permitir cerrar (no salir de aplicación)
            }
        }
    }
}
