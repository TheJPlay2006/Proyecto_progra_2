using SistemaDeTickets.Controlador;
using SistemaDeTickets.Services;
using SistemaDeTickets.Utils;
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
    public partial class VistaGestionEventos : Form
    {
        private ControladorEvento controlador;
        public VistaGestionEventos()
        {
            InitializeComponent();
            controlador = new ControladorEvento();
            // Cargar eventos leyendo SIEMPRE del JSON
            CargarEventos();
        }
        // ------------------------------
        // Cargar los eventos en la tabla - SIEMPRE LEE DEL JSON
        // ------------------------------
        private void CargarEventos()
        {
            // SIEMPRE leer directamente del JSON para obtener datos frescos
            var eventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();

            dgvEventos.DataSource = null;
            dgvEventos.DataSource = eventos;
            dgvEventos.AutoResizeColumns();
        }
        // Botón Agregar nuevo evento
        private void btnCrear_Click(object sender, EventArgs e)
        {
            var formCambios = new VistaCambiosEventos(); // abre el formulario para crear un evento
            if (formCambios.ShowDialog() == DialogResult.OK)
            {
                // REFRESCAR leyendo del JSON actualizado
                CargarEventos();
            }
        }
        // Botón Editar evento seleccionado

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvEventos.CurrentRow != null)
            {
                var eventoSeleccionado = (Modelo.Evento)dgvEventos.CurrentRow.DataBoundItem;
                var formEditar = new VistaCambiosEventos(eventoSeleccionado) ; // pasa el evento al nuevo formulario

                if (formEditar.ShowDialog() == DialogResult.OK)
                {
                    // REFRESCAR leyendo del JSON actualizado
                    CargarEventos();
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona un evento para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // Botón Eliminar evento
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEventos.CurrentRow != null)
            {
                Modelo.Evento seleccionado = (Modelo.Evento)dgvEventos.CurrentRow.DataBoundItem;
                DialogResult result = MessageBox.Show(
                    $"¿Seguro que deseas eliminar el evento '{seleccionado.Nombre}'?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    if (controlador.EliminarEvento(seleccionado.Id))
                    {
                        MessageBox.Show("Evento eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // REFRESCAR leyendo del JSON actualizado
                        CargarEventos();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el evento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona un evento para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // Botón Salir
        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Cerrar sesión si hay usuario logueado
            if (ServicioAutenticacion.IsLoggedIn())
            {
                ServicioAutenticacion.Logout();
            }
            this.Close(); // Cierra solo esta ventana
        }
    }
}
