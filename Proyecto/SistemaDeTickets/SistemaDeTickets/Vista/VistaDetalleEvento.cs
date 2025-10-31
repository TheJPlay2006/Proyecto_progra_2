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

            CargarDatos();
            ConfigurarControles();
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

        private void CargarDatos()
        {
            try
            {
                var repoEventos = new RepositorioEventos();
                _evento = repoEventos.BuscarPorId(_eventoId);

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

        private void btnComprar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar sesión
                if (!ServicioAutenticacion.IsLoggedIn())
                {
                    MessageBox.Show("Debe iniciar sesión para comprar tickets.", "Sesión requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                var compraForm = new VistaCompra(detalleCompra);
                compraForm.StartPosition = FormStartPosition.CenterScreen;
                compraForm.Show();

                // Cerrar este formulario
                this.Hide();
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
                var eventosForm = new VistaEvento();
                eventosForm.StartPosition = FormStartPosition.CenterScreen;
                eventosForm.Show();

                // Cerrar este formulario
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al volver al listado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VistaDetalleEvento_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si el usuario cierra la ventana, mostrar confirmación
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro que desea cerrar la ventana?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
