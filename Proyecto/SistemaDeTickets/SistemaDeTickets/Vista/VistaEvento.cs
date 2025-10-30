using SistemaDeTickets.Controlador;
using SistemaDeTickets.Services;
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
    public partial class VistaEvento : Form
    {
        // Variable para poder usar las funciones del controlador de eventos
        private ControladorEvento controlador;

        // Constructor del formulario
        public VistaEvento()
        {
            InitializeComponent(); // Carga los componentes del formulario
            controlador = new ControladorEvento(); // Se crea el controlador para manejar los eventos
        }

        
        // Cargar los eventos dentro del DataGridView
       
        private void CargarEventos(List<Modelo.Evento> lista = null)
        {
            // Si no se recibe una lista, se cargan todos los eventos desde el controlador
            var eventos = lista ?? controlador.ObtenerEventos();

            // Limpia y vuelve a asignar los datos al DataGridView
            dgvEventos.DataSource = null;
            dgvEventos.DataSource = eventos;

            // Oculta columnas que no quiero que se muestren en la tabla
            dgvEventos.Columns["Id"].Visible = false;
            dgvEventos.Columns["Descripcion"].Visible = false;
            dgvEventos.Columns["Precio"].Visible = false;

            // Ajusta automáticamente el ancho de las columnas
            dgvEventos.AutoResizeColumns();
        }

        
        // Cargar los filtros (combos de tipo, recinto y fecha)
       
        private void CargarFiltros()
        {
            var eventos = controlador.ObtenerEventos();

            // Carga las fechas únicas de los eventos al ComboBox
            cmbBuscarFecha.DataSource = eventos
                .Select(e => e.Fecha.ToString("yyyy-MM-dd"))
                .Distinct()
                .ToList();
            cmbBuscarFecha.SelectedIndex = -1; // Sin selección por defecto

            // Carga los tipos de evento
            cmbTipoEvento.DataSource = eventos
                .Select(e => e.Tipo)
                .Distinct()
                .ToList();
            cmbTipoEvento.SelectedIndex = -1;

            // Carga los recintos
            cmbRecinto.DataSource = eventos
                .Select(e => e.Recinto)
                .Distinct()
                .ToList();
            cmbRecinto.SelectedIndex = -1;
        }

        
        // Botón Buscar → filtra los eventos según los combos
       
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtiene lo que el usuario seleccionó en los ComboBox
            string fecha = cmbBuscarFecha.Text;
            string tipo = cmbTipoEvento.Text;
            string recinto = cmbRecinto.Text;

            // Carga todos los eventos
            List<Modelo.Evento> filtrados = controlador.ObtenerEventos();

            // Si se eligió un tipo, filtra por tipo
            if (!string.IsNullOrEmpty(tipo))
            {
                Predicate<Modelo.Evento> match = ev => ev.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase);
                filtrados = filtrados.FindAll(match);
            }

            // Si se eligió un recinto, filtra por recinto
            if (!string.IsNullOrEmpty(recinto))
                filtrados = filtrados.FindAll(ev => ev.Recinto.Equals(recinto, StringComparison.OrdinalIgnoreCase));

            // Si se eligió una fecha, filtra por fecha
            if (!string.IsNullOrEmpty(fecha))
            {
                DateTime fechaSeleccionada = DateTime.Parse(fecha);
                filtrados = filtrados.FindAll(ev => ev.Fecha.Date == fechaSeleccionada.Date);
            }

            // Muestra los eventos filtrados en la tabla
            CargarEventos(filtrados);
        }

        
        // Cuando se carga el formulario
       
        private void VistaEvento_Load(object sender, EventArgs e)
        {
            // Obtiene todos los eventos
            List<Modelo.Evento> eventos = controlador.ObtenerEventos();

            // Llenar los ComboBox con valores únicos
            var tipos = eventos.Select(ev => ev.Tipo).Distinct().ToList();
            cmbTipoEvento.DataSource = tipos;

            var recintos = eventos.Select(ev => ev.Recinto).Distinct().ToList();
            cmbRecinto.DataSource = recintos;

            var fechas = eventos.Select(ev => ev.Fecha.ToString("yyyy-MM-dd")).Distinct().ToList();
            cmbBuscarFecha.DataSource = fechas;

            // Cargar todos los eventos en la tabla
            CargarEventos(eventos);
        }

        
        // Botón Comprar → permite comprar un tiquete
        
        private void btnComprar_Click(object sender, EventArgs e)
        {
            // Primero se verifica si el usuario ha iniciado sesión
            if (!ServicioAutenticacion.IsLoggedIn())
            {
                // Si no está logeado, se pregunta si quiere hacerlo
                var result = MessageBox.Show("Debes iniciar sesión para comprar. ¿Quieres iniciar sesión ahora?",
                                             "Login requerido", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    // Abre el formulario de login
                    using (var login = new VistaLogin())
                    {
                        var dlg = login.ShowDialog();

                        // Si aún no inició sesión, no puede seguir
                        if (!ServicioAutenticacion.IsLoggedIn())
                        {
                            MessageBox.Show("No se inició sesión. La compra no puede continuar.",
                                            "Login requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                else
                {
                    // Si el usuario dijo que no, se cancela la compra
                    return;
                }
            }

            // Si el usuario ya está logeado
            if (dgvEventos.CurrentRow != null)
            {
                // Obtiene el evento seleccionado de la tabla
                Modelo.Evento seleccionado = (Modelo.Evento)dgvEventos.CurrentRow.DataBoundItem;

                // Se podría agregar una validación de cantidad o un formulario de pago aquí
                if (controlador.Comprar(seleccionado, 1))
                {
                    MessageBox.Show("Compra realizada con éxito.", "Compra", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Actualiza la tabla con la nueva cantidad de tiques
                    CargarEventos(controlador.ObtenerEventos());
                }
                else
                {
                    MessageBox.Show("No hay tiques disponibles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

       
        // Botón Ver Detalles → muestra la info completa del evento
       
        private void btnVerDetalles_Click(object sender, EventArgs e)
        {
            if (dgvEventos.CurrentRow != null)
            {
                Modelo.Evento seleccionado = (Modelo.Evento)dgvEventos.CurrentRow.DataBoundItem;

                // Arma el texto con toda la información del evento
                string mensaje =
                    $"Nombre: {seleccionado.Nombre}\n" +
                    $"Fecha: {seleccionado.Fecha:dd/MM/yyyy HH:mm}\n" +
                    $"Recinto: {seleccionado.Recinto}\n" +
                    $"Tipo: {seleccionado.Tipo}\n" +
                    $"Tiques disponibles: {seleccionado.TiquetesDisponibles}\n" +
                    $"Descripción: {seleccionado.Descripcion}\n" +
                    $"Precio: {seleccionado.Precio:C}";

                // Muestra los detalles en un mensaje
                MessageBox.Show(mensaje, "Detalles del Evento");
            }
        }


        
        // Botón que lleva a la pantalla de Login
       
        private void button2_Click(object sender, EventArgs e)
        {
            var loginForm = new VistaLogin();
            loginForm.StartPosition = FormStartPosition.CenterScreen; // Centrar ventana
            loginForm.Show();
            this.Hide(); // Oculta la ventana actual pero no cierra la app
        }

        
        // Botón que lleva a la pantalla de Registro
        
        private void button1_Click(object sender, EventArgs e)
        {
            var registroForm = new VistaRegistro();
            registroForm.StartPosition = FormStartPosition.CenterScreen;
            registroForm.Show();
            this.Hide();
        }

        // Botón de salir del sistema
       
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
               "¿Deseas salir del sistema?",
               "Confirmar salida",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question
            );

            // Si el usuario confirma, se cierra toda la aplicación
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
