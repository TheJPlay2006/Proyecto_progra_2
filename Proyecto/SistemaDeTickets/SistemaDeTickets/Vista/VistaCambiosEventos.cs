using SistemaDeTickets.Controlador;
using SistemaDeTickets.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaDeTickets.Vista
{
    public partial class VistaCambiosEventos : Form
    {
        private ControladorEvento controlador;
        private Modelo.Evento eventoActual;
        private bool esEdicion;

        // Constructor para CREAR un evento nuevo
        public VistaCambiosEventos()
        {
            InitializeComponent();
            controlador = new ControladorEvento();
            esEdicion = false;
        }

        // Constructor para EDITAR un evento existente
        public VistaCambiosEventos(Modelo.Evento eventoEditar)
        {
            InitializeComponent();
            controlador = new ControladorEvento();
            eventoActual = eventoEditar;
            esEdicion = true;

            // Cargar los datos del evento en los campos del formulario
            txtNombre.Text = eventoEditar.Nombre;
            txtTipo.Text = eventoEditar.Tipo;
            txtRecinto.Text = eventoEditar.Recinto;
            txtDescripcion.Text = eventoEditar.Descripcion;
            dtpFecha.Value = eventoEditar.Fecha;
            nudTiquetes.Value = eventoEditar.TiquetesDisponibles;
            txtPrecio.Text = eventoEditar.Precio.ToString();
        }
        // Botón Guardar
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtTipo.Text))
            {
                MessageBox.Show("Por favor completa todos los campos obligatorios.");
                return;
            }

            if (esEdicion)
            {
                // Actualiza el evento existente
                eventoActual.Nombre = txtNombre.Text;
                eventoActual.Tipo = txtTipo.Text;
                eventoActual.Recinto = txtRecinto.Text;
                eventoActual.Descripcion = txtDescripcion.Text;
                eventoActual.Fecha = dtpFecha.Value;
                eventoActual.TiquetesDisponibles = (int)nudTiquetes.Value;
                eventoActual.Precio = double.Parse(txtPrecio.Text);

                controlador.EditarEvento(eventoActual);
                MessageBox.Show("Evento editado correctamente.", "Éxito");
            }
            else
            {
                // Crea un nuevo evento
                var nuevo = new Modelo.Evento
                {
                    Nombre = txtNombre.Text,
                    Tipo = txtTipo.Text,
                    Recinto = txtRecinto.Text,
                    Descripcion = txtDescripcion.Text,
                    Fecha = dtpFecha.Value,
                    TiquetesDisponibles = (int)nudTiquetes.Value,
                    Precio = double.Parse(txtPrecio.Text)
                };

                controlador.CrearEvento(nuevo);
                MessageBox.Show("Evento creado correctamente.", "Éxito");
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        // Botón Cancelar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
