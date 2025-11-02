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
using SistemaDeTickets.Controlador;
using SistemaDeTickets.Services;
using SistemaDeTickets.Utils;

namespace SistemaDeTickets.Vista
{
    /// <summary>
    /// Formulario para mostrar detalles completos de un evento y gestionar compras
    /// </summary>
    public partial class VistaDetalleEvento : Form
    {
        private int _usuarioId;
        private int _eventoId;
        private Modelo.Evento _evento;

        public VistaDetalleEvento(int usuarioId, int eventoId)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema de Tickets - Detalles del Evento";

            _usuarioId = usuarioId;
            _eventoId = eventoId;

            // REFRESCAR DATOS FRESCOS DESDE JSON
            RefrescarDatosEvento();
            ConfigurarControles();

            // Configurar eventos para refresco automático
            this.Activated += VistaDetalleEvento_Activated;
            this.Shown += VistaDetalleEvento_Shown;
        }

        /// <summary>
        /// Evento que se dispara cuando la ventana recibe el foco - refresca datos
        /// </summary>
        private void VistaDetalleEvento_Activated(object sender, EventArgs e)
        {
            // REFRESCO FORZADO: Cada vez que la ventana vuelve a tener foco
            RefrescarDatosEvento();
        }

        /// <summary>
        /// Evento adicional para refresco cuando la ventana se muestra
        /// </summary>
        private void VistaDetalleEvento_Shown(object sender, EventArgs e)
        {
            // REFRESCO ADICIONAL: Cuando la ventana se muestra completamente
            RefrescarDatosEvento();
        }

        private void ConfigurarControles()
        {
            // Configurar NumericUpDown para cantidad
            if (this.Controls.ContainsKey("numCantidad"))
            {
                var numCantidad = this.Controls["numCantidad"] as NumericUpDown;
                if (numCantidad != null)
                {
                    numCantidad.Minimum = 1;
                    numCantidad.Maximum = _evento?.TiquetesDisponibles ?? 10;
                    numCantidad.Value = 1;
                }
            }

            // Configurar botones
            if (this.Controls.ContainsKey("btnComprar"))
            {
                var btnComprar = this.Controls["btnComprar"] as Button;
                if (btnComprar != null)
                {
                    btnComprar.Text = "Comprar Tickets";
                    btnComprar.BackColor = Color.FromArgb(76, 175, 80);
                    btnComprar.ForeColor = Color.White;
                    btnComprar.FlatStyle = FlatStyle.Flat;
                    btnComprar.Height = 40;
                    btnComprar.Width = 150;
                }
            }

            if (this.Controls.ContainsKey("btnVolver"))
            {
                var btnVolver = this.Controls["btnVolver"] as Button;
                if (btnVolver != null)
                {
                    btnVolver.Text = "Volver";
                    btnVolver.BackColor = Color.FromArgb(158, 158, 158);
                    btnVolver.ForeColor = Color.White;
                    btnVolver.FlatStyle = FlatStyle.Flat;
                    btnVolver.Height = 40;
                    btnVolver.Width = 100;
                }
            }
        }

        /// <summary>
        /// Método centralizado para refrescar datos del evento desde JSON
        /// </summary>
        public void RefrescarDatosEvento()
        {
            try
            {
                // SIEMPRE leer directamente del JSON para datos frescos
                var eventos = SistemaDeTickets.Utils.GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();
                _evento = eventos.FirstOrDefault(e => e.Id == _eventoId);

                if (_evento != null)
                {
                    // Llenar labels con información del evento
                    if (this.Controls.ContainsKey("lblNombreEvento"))
                        this.Controls["lblNombreEvento"].Text = _evento.Nombre;

                    if (this.Controls.ContainsKey("lblFecha"))
                        this.Controls["lblFecha"].Text = _evento.Fecha.ToString("dd/MM/yyyy HH:mm");

                    if (this.Controls.ContainsKey("lblRecinto"))
                        this.Controls["lblRecinto"].Text = _evento.Recinto;

                    if (this.Controls.ContainsKey("lblTipo"))
                        this.Controls["lblTipo"].Text = _evento.Tipo;

                    if (this.Controls.ContainsKey("lblDescripcion"))
                        this.Controls["lblDescripcion"].Text = _evento.Descripcion;

                    if (this.Controls.ContainsKey("lblPrecio"))
                        this.Controls["lblPrecio"].Text = $"₡{_evento.Precio:N0}";

                    if (this.Controls.ContainsKey("lblDisponibles"))
                        this.Controls["lblDisponibles"].Text = $"{_evento.TiquetesDisponibles} disponibles";

                    // Actualizar NumericUpDown máximo
                    if (this.Controls.ContainsKey("numCantidad"))
                    {
                        var numCantidad = this.Controls["numCantidad"] as NumericUpDown;
                        if (numCantidad != null)
                        {
                            numCantidad.Maximum = _evento.TiquetesDisponibles;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Evento no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del evento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void CargarDatos()
        {
            RefrescarDatosEvento();
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar sesión
                if (!ServicioAutenticacion.IsLoggedIn() || ServicioAutenticacion.CurrentUser == null)
                {
                    MessageBox.Show("Debe iniciar sesión para comprar tickets.", "Sesión requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Hide();
                    var loginForm = new VistaLogin();
                    loginForm.ContextoOrigen = VistaLogin.ContextoNavegacion.DesdeInicio;
                    loginForm.StartPosition = FormStartPosition.CenterScreen;
                    loginForm.ShowDialog();
                    this.Show();
                    return;
                }

                // Obtener cantidad seleccionada
                int cantidad = 1;
                if (this.Controls.ContainsKey("numCantidad"))
                {
                    var numCantidad = this.Controls["numCantidad"] as NumericUpDown;
                    if (numCantidad != null)
                    {
                        cantidad = (int)numCantidad.Value;
                    }
                }

                // Validar cantidad
                if (cantidad <= 0)
                {
                    MessageBox.Show("Debe seleccionar al menos 1 ticket.", "Cantidad inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_evento != null && cantidad > _evento.TiquetesDisponibles)
                {
                    MessageBox.Show($"No hay suficientes tickets disponibles. Máximo: {_evento.TiquetesDisponibles}",
                        "Inventario insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear detalle de compra
                var detalleCompra = new DetalleCompra
                {
                    Evento = _evento,
                    Cantidad = cantidad,
                    Total = (decimal)(cantidad * _evento.Precio)
                };

                // Abrir formulario de compra
                this.Hide();
                var compraForm = new VistaCompra(detalleCompra);
                compraForm.StartPosition = FormStartPosition.CenterScreen;
                compraForm.ShowDialog();

                // REFRESCAR DATOS DESPUÉS DE LA COMPRA - Leer JSON fresco
                RefrescarDatosEvento();

                // Mostrar este formulario nuevamente cuando se cierre compra
                if (!this.IsDisposed)
                {
                    this.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la compra: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                // Volver al listado de eventos
                this.Hide();
                var eventosForm = new VistaEvento();
                eventosForm.StartPosition = FormStartPosition.CenterScreen;
                eventosForm.ShowDialog();

                // Cerrar este formulario después de que VistaEvento se cierre
                if (!this.IsDisposed)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al volver al listado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VistaDetalleEvento_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si el usuario cierra la ventana con X, volver al listado de eventos
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro que desea cerrar la ventana de detalles?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Volver al listado de eventos
                    var eventosForm = new VistaEvento();
                    eventosForm.StartPosition = FormStartPosition.CenterScreen;
                    eventosForm.Show();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
