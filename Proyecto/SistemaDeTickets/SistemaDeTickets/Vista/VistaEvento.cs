using SistemaDeTickets.Controlador;
using SistemaDeTickets.Services;
using SistemaDeTickets.Controlador.Patrones;
using SistemaDeTickets.Modelo;
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
    public partial class VistaEvento : Form, IObservador
    {
        // Variable para poder usar las funciones del controlador de eventos
        private ControladorEvento controlador;
        private Label lblStockBajo; // Label para mostrar alertas de stock bajo

        // Constructor del formulario
        public VistaEvento()
        {
            InitializeComponent(); // Carga los componentes del formulario
            controlador = new ControladorEvento(); // Se crea el controlador para manejar los eventos

            // Configurar apariencia moderna
            ConfigurarInterfazEvento();

            // Inicializar label de stock bajo
            InicializarLabelStockBajo();

            // Registrar como observador con el GestorEventos
            var gestorEventos = new GestorEventos();
            gestorEventos.AgregarObservador(this);
        }

        private void ConfigurarInterfazEvento()
        {
            this.Text = "Sistema de Tickets - Eventos Disponibles";
            this.BackColor = Color.FromArgb(247, 247, 251);

            // Restaurar imagen de fondo original
            try
            {
                string rutaImagen = System.IO.Path.Combine(Application.StartupPath, "Images", "Fondo_Pop-Conciertos_Fondo-claro_F7F7FB_3840x2160.png");
                if (System.IO.File.Exists(rutaImagen))
                {
                    this.BackgroundImage = Image.FromFile(rutaImagen);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch
            {
                // Si no se puede cargar la imagen, mantener color sólido
            }
            this.StartPosition = FormStartPosition.CenterScreen;

            // Configurar título
            if (this.Controls.ContainsKey("lblTitulo"))
            {
                var lblTitulo = this.Controls["lblTitulo"] as Label;
                if (lblTitulo != null)
                {
                    lblTitulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
                    lblTitulo.ForeColor = Color.FromArgb(30, 31, 59);
                    lblTitulo.Text = "EVENTOS DISPONIBLES";
                }
            }

            // Configurar botones con colores originales (blanco)
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Name.Contains("Comprar"))
                        ConfigurarBoton(btn, "Comprar Tickets", Color.White);
                    else if (btn.Name.Contains("VerDetalles"))
                        ConfigurarBoton(btn, "Ver Detalles", Color.White);
                    else if (btn.Name.Contains("Buscar"))
                        ConfigurarBoton(btn, "Buscar", Color.White);
                    else if (btn.Name.Contains("Login") || btn.Name.Contains("Iniciar"))
                        ConfigurarBoton(btn, "Iniciar Sesión", Color.White);
                    else if (btn.Name.Contains("Registro"))
                        ConfigurarBoton(btn, "Registrarse", Color.White);
                    else if (btn.Name.Contains("Salir"))
                        ConfigurarBoton(btn, "Salir", Color.White);
                }
            }

            // Configurar DataGridView
            if (this.Controls.ContainsKey("dgvEventos"))
            {
                var dgv = this.Controls["dgvEventos"] as DataGridView;
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
                }
            }
        }

        private void ConfigurarBoton(Button btn, string texto, Color colorFondo)
        {
            btn.Text = texto;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.BackColor = colorFondo;
            btn.ForeColor = Color.Black;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Height = 35;
            btn.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Inicializa el label para mostrar alertas de stock bajo
        /// </summary>
        private void InicializarLabelStockBajo()
        {
            lblStockBajo = new Label();
            lblStockBajo.Text = "";
            lblStockBajo.ForeColor = Color.White;
            lblStockBajo.BackColor = Color.FromArgb(244, 67, 54); // Rojo para alerta
            lblStockBajo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblStockBajo.AutoSize = true;
            lblStockBajo.Visible = false;
            lblStockBajo.Padding = new Padding(10);
            lblStockBajo.BorderStyle = BorderStyle.FixedSingle;

            // Posicionar en la parte superior del formulario
            lblStockBajo.Location = new Point(10, 10);
            lblStockBajo.BringToFront();
            this.Controls.Add(lblStockBajo);
        }

        
        // Cargar los eventos dentro del DataGridView
       
        private void CargarEventos(List<Modelo.Evento> lista = null)
        {
            // RECARGAR SIEMPRE DESDE JSON PARA TENER DATOS FRESCOS
            var eventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();

            // Si se recibió una lista filtrada, aplicar filtro sobre datos frescos
            if (lista != null && lista.Any())
            {
                var idsFiltrados = lista.Select(e => e.Id).ToHashSet();
                eventos = eventos.Where(e => idsFiltrados.Contains(e.Id)).ToList();
            }

            // MARCAR EVENTOS AGOTADOS
            foreach (var evento in eventos)
            {
                if (evento.TiquetesDisponibles <= 0)
                {
                    evento.TiquetesDisponibles = 0; // Asegurar no negativo
                }
            }

            // Limpia y vuelve a asignar los datos al DataGridView
            dgvEventos.DataSource = null;
            dgvEventos.DataSource = eventos;

            // Oculta columnas que no quiero que se muestren en la tabla
            if (dgvEventos.Columns.Contains("Id"))
                dgvEventos.Columns["Id"].Visible = false;
            if (dgvEventos.Columns.Contains("Descripcion"))
                dgvEventos.Columns["Descripcion"].Visible = false;
            if (dgvEventos.Columns.Contains("Precio"))
                dgvEventos.Columns["Precio"].Visible = false;

            // Ajusta automáticamente el ancho de las columnas
            dgvEventos.AutoResizeColumns();

            // CONFIGURAR FORMATEO VISUAL PARA EVENTOS AGOTADOS
            if (dgvEventos.Columns.Contains("TiquetesDisponibles"))
            {
                // Usar CellFormatting para mejor control visual
                dgvEventos.CellFormatting += DgvEventos_CellFormatting;
            }
        }

        
        // Cargar los filtros (combos de tipo, recinto y fecha)
       
        private void CargarFiltros()
        {
            // CARGAR FILTROS DESDE JSON FRESCO
            var eventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();

            // Carga las fechas únicas de los eventos al ComboBox
            cmbBuscarFecha.DataSource = eventos
                .Where(e => e.TiquetesDisponibles > 0) // Solo eventos con stock
                .Select(e => e.Fecha.ToString("yyyy-MM-dd"))
                .Distinct()
                .ToList();
            cmbBuscarFecha.SelectedIndex = -1; // Sin selección por defecto

            // Carga los tipos de evento
            cmbTipoEvento.DataSource = eventos
                .Where(e => e.TiquetesDisponibles > 0) // Solo eventos con stock
                .Select(e => e.Tipo)
                .Distinct()
                .ToList();
            cmbTipoEvento.SelectedIndex = -1;

            // Carga los recintos
            cmbRecinto.DataSource = eventos
                .Where(e => e.TiquetesDisponibles > 0) // Solo eventos con stock
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

            // Carga todos los eventos DESDE JSON FRESCO
            List<Modelo.Evento> todosEventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();
            List<Modelo.Evento> filtrados = todosEventos;

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
            // Cargar filtros desde datos frescos del JSON
            CargarFiltros();

            // Cargar todos los eventos en la tabla desde JSON fresco
            CargarEventos();

            // Configurar visibilidad del botón de historial
            if (btnVerHistorial != null)
            {
                btnVerHistorial.Visible = ServicioAutenticacion.IsLoggedIn();
            }
        }

        
        // Botón Comprar → permite comprar un tiquete
        
        private void btnComprar_Click(object sender, EventArgs e)
        {
            // Verificar selección de evento primero
            if (dgvEventos.CurrentRow == null)
            {
                MessageBox.Show("Por favor, selecciona un evento de la lista.",
                                "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Modelo.Evento eventoSeleccionado = (Modelo.Evento)dgvEventos.CurrentRow.DataBoundItem;

            // RECARGAR DATOS FRESCOS DEL ARCHIVO para verificar stock real
            var eventosActualizados = GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();
            var eventoActual = eventosActualizados.FirstOrDefault(ev => ev.Id == eventoSeleccionado.Id);

            if (eventoActual == null)
            {
                MessageBox.Show("El evento seleccionado ya no está disponible.",
                                "Evento No Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                RefrescarEventosDesdeJson(); // Forzar refresco completo
                return;
            }

            // Verificar stock usando datos DIRECTOS del archivo JSON
            if (eventoActual.TiquetesDisponibles < 1)
            {
                MessageBox.Show("Este evento está AGOTADO. No hay tickets disponibles.",
                                "Evento Agotado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                RefrescarEventosDesdeJson(); // Forzar refresco completo
                return;
            }

            // Verificar sesión de manera consistente
            if (!ServicioAutenticacion.IsLoggedIn() || ServicioAutenticacion.CurrentUser == null)
            {
                var result = MessageBox.Show("Debes iniciar sesión para comprar tickets. ¿Quieres iniciar sesión ahora?",
                                              "Inicio de Sesión Requerido", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Abrir login con contexto de compra
                    this.Hide();
                    var loginForm = new VistaLogin();
                    loginForm.ContextoOrigen = VistaLogin.ContextoNavegacion.DesdeCompraEvento;
                    loginForm.EventoSeleccionadoCache = eventoSeleccionado; // Guardar evento seleccionado
                    loginForm.StartPosition = FormStartPosition.CenterScreen;
                    loginForm.ShowDialog();
                    this.Show(); // Mostrar VistaEvento nuevamente cuando se cierre login
                    return; // Salir para que el login maneje la navegación posterior
                }
                else
                {
                    return; // Usuario canceló
                }
            }

            // Usuario autenticado - proceder con compra usando datos frescos
            this.Hide();
            var compraForm = new VistaCompra(new DetalleCompra
            {
                Evento = eventoActual, // Usar datos frescos del archivo
                Cantidad = 1, // Cantidad por defecto
                Total = (decimal)eventoActual.Precio
            });

            compraForm.StartPosition = FormStartPosition.CenterScreen;
            compraForm.ShowDialog();
            this.Show(); // Mostrar VistaEvento nuevamente cuando se cierre compra
        }

       
        // Botón Ver Detalles → muestra la info completa del evento
       
        private void btnVerDetalles_Click(object sender, EventArgs e)
        {
            if (dgvEventos.CurrentRow != null)
            {
                Modelo.Evento seleccionado = (Modelo.Evento)dgvEventos.CurrentRow.DataBoundItem;

                // RECARGAR DATOS FRESCOS DEL ARCHIVO ANTES DE MOSTRAR DETALLES
                var eventosActualizados = GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();
                var eventoActual = eventosActualizados.FirstOrDefault(ev => ev.Id == seleccionado.Id);

                if (eventoActual == null)
                {
                    MessageBox.Show("El evento seleccionado ya no está disponible.",
                                    "Evento No Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarEventos(); // Recargar grilla
                    return;
                }

                // CREAR NUEVA INSTANCIA DE FORMULARIO PARA EVITAR OBJETOS DISPOSEADOS
                this.Hide();
                var vistaDetalle = new VistaDetalleEvento(ServicioAutenticacion.CurrentUser?.Id ?? 0, eventoActual.Id);
                vistaDetalle.StartPosition = FormStartPosition.CenterScreen;
                vistaDetalle.ShowDialog();
                this.Show(); // Mostrar VistaEvento nuevamente cuando se cierre detalles
            }
        }


        
        // Botón que lleva a la pantalla de Login

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new VistaLogin();
            loginForm.ContextoOrigen = VistaLogin.ContextoNavegacion.DesdeInicio; // Desde VistaEvento
            loginForm.StartPosition = FormStartPosition.CenterScreen; // Centrar ventana
            loginForm.ShowDialog();
            this.Show(); // Mostrar VistaEvento nuevamente cuando se cierre login
        }

        
        // Botón que lleva a la pantalla de Registro

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var registroForm = new VistaRegistro();
            registroForm.StartPosition = FormStartPosition.CenterScreen;
            registroForm.ShowDialog();
            this.Show(); // Mostrar VistaEvento nuevamente cuando se cierre registro
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
                // Cerrar sesión si hay usuario logueado
                if (ServicioAutenticacion.IsLoggedIn())
                {
                    ServicioAutenticacion.Logout();
                }
                Application.Exit();
            }
        }

        /// <summary>
        /// Evento para cerrar sesión y volver al login
        /// </summary>
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // Cerrar sesión
            ServicioAutenticacion.Logout();

            // Mostrar login nuevamente
            this.Hide();
            var loginForm = new VistaLogin();
            loginForm.ContextoOrigen = VistaLogin.ContextoNavegacion.DesdeInicio; // Volver a inicio después de logout
            loginForm.StartPosition = FormStartPosition.CenterScreen;
            loginForm.ShowDialog();

            // Cerrar vista de eventos después de que login se cierre
            this.Close();
        }

        /// <summary>
        /// Evento para ver el historial de compras del usuario
        /// </summary>
        private void btnVerHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar sesión
                if (!ServicioAutenticacion.IsLoggedIn() || ServicioAutenticacion.CurrentUser == null)
                {
                    MessageBox.Show("Debe iniciar sesión para ver su historial de compras.", "Sesión requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Hide();
                    var loginForm = new VistaLogin();
                    loginForm.ContextoOrigen = VistaLogin.ContextoNavegacion.DesdeInicio;
                    loginForm.StartPosition = FormStartPosition.CenterScreen;
                    loginForm.ShowDialog();
                    this.Show();
                    return;
                }

                // Abrir historial
                this.Hide();
                var historialForm = new VistaHistorial();
                historialForm.StartPosition = FormStartPosition.CenterScreen;
                historialForm.ShowDialog();

                // Mostrar VistaEvento nuevamente cuando se cierre historial
                if (!this.IsDisposed)
                {
                    this.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el historial: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Implementación del patrón Observer - recibe actualizaciones del sistema
        /// </summary>
        public void Actualizar(TipoNotificacion tipo, object datos)
        {
            // Ejecutar en el hilo de la UI
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => Actualizar(tipo, datos)));
                return;
            }

            switch (tipo)
            {
                case TipoNotificacion.NuevoEvento:
                    // Mostrar notificación de nuevo evento
                    var nuevoEvento = datos as Modelo.Evento;
                    if (nuevoEvento != null)
                    {
                        MessageBox.Show($"¡Nuevo evento disponible: {nuevoEvento.Nombre}!",
                            "Nuevo Evento", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Recargar eventos
                        CargarEventos();
                    }
                    break;

                case TipoNotificacion.BajoInventario:
                    // Mostrar alerta de stock bajo
                    var infoStock = datos as dynamic;
                    if (infoStock != null)
                    {
                        string eventoId = infoStock.EventoId;
                        int cantidadRestante = infoStock.CantidadRestante;

                        // Mostrar badge de alerta en la parte superior
                        lblStockBajo.Text = $"⚠ ALERTA: Solo {cantidadRestante} tickets restantes para evento #{eventoId}";
                        lblStockBajo.Visible = true;

                        // Mostrar notificación emergente (opcional, solo si es crítico)
                        if (cantidadRestante <= 5)
                        {
                            MessageBox.Show($"¡CRÍTICO! Solo quedan {cantidadRestante} tickets para el evento #{eventoId}.\n¡Compra ahora!",
                                "Stock Muy Bajo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        // Auto-ocultar después de 8 segundos
                        Task.Delay(8000).ContinueWith(_ =>
                        {
                            if (!this.IsDisposed)
                            {
                                this.Invoke(new Action(() => lblStockBajo.Visible = false));
                            }
                        });

                        // Actualizar la vista de eventos para reflejar stock actual
                        CargarEventos();
                    }
                    break;

                case TipoNotificacion.CompraExitosa:
                    // Forzar refresco completo desde JSON después de una compra exitosa
                    RefrescarEventosDesdeJson();
                    break;
            }
        }

        /// <summary>
        /// Método público para refresco forzado desde otras ventanas
        /// </summary>
        public void RefrescarVista()
        {
            RefrescarEventosDesdeJson();
        }

        /// <summary>
        /// Método auxiliar para recargar eventos sin parámetros
        /// </summary>
        private void CargarEventos()
        {
            CargarEventos(null);
        }

        private void cmbTipoEvento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Evento que se dispara cuando la ventana recibe el foco - refresca datos
        /// </summary>
        private void VistaEvento_Activated(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG VistaEvento] Form Activated - Refrescando datos");
            // REFRESCO FORZADO: Cada vez que la ventana vuelve a tener foco
            RefrescarEventosDesdeJson();
        }

        /// <summary>
        /// Evento adicional para refresco cuando la ventana se muestra
        /// </summary>
        private void VistaEvento_Shown(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG VistaEvento] Form Shown - Refrescando datos adicionales");
            // REFRESCO ADICIONAL: Cuando la ventana se muestra completamente
            RefrescarEventosDesdeJson();
        }

        /// <summary>
        /// Método para refrescar eventos desde JSON de forma forzada - IMPLEMENTACIÓN ROBUSTA
        /// </summary>
        private void RefrescarEventosDesdeJson()
        {
            try
            {
                Console.WriteLine("[DEBUG VistaEvento] Refrescando eventos desde JSON...");

                // PASO 1: LEER SIEMPRE DEL JSON FRESCO (fuente real de verdad)
                var eventos = GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();

                Console.WriteLine($"[DEBUG VistaEvento] Eventos cargados: {eventos.Count}");

                // Mostrar stock de cada evento para debug
                foreach (var evento in eventos)
                {
                    Console.WriteLine($"[DEBUG VistaEvento] Evento {evento.Id} ({evento.Nombre}): {evento.TiquetesDisponibles} tickets");
                }

                // PASO 2: MARCAR EVENTOS AGOTADOS (lógica de negocio)
                foreach (var evento in eventos)
                {
                    if (evento.TiquetesDisponibles <= 0)
                    {
                        evento.TiquetesDisponibles = 0; // Asegurar no negativo
                    }
                }

                // PASO 3: REFRESCO ROBUSTO DEL DATAGRIDVIEW - VACÍAR Y REPOPULAR
                // Método universal recomendado por la comunidad .NET
                dgvEventos.DataSource = null;  // IMPORTANTE: Vaciar completamente
                dgvEventos.DataSource = eventos;  // Repoblar con datos frescos

                // PASO 4: RECONFIGURAR COLUMNAS (después del refresco)
                if (dgvEventos.Columns.Contains("Id"))
                    dgvEventos.Columns["Id"].Visible = false;
                if (dgvEventos.Columns.Contains("Descripcion"))
                    dgvEventos.Columns["Descripcion"].Visible = false;
                if (dgvEventos.Columns.Contains("Precio"))
                    dgvEventos.Columns["Precio"].Visible = false;

                // PASO 5: AUTO-RESIZE Y FORMATEO VISUAL
                dgvEventos.AutoResizeColumns();

                // PASO 6: RECARGAR FILTROS SOBRE DATOS FRESCOS
                CargarFiltros();

                Console.WriteLine("[DEBUG VistaEvento] Refresco completado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG VistaEvento] Error al refrescar: {ex.Message}");
                MessageBox.Show($"Error al refrescar eventos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Formateo visual de celdas para mostrar "AGOTADO" en eventos sin stock - IMPLEMENTACIÓN ROBUSTA
        /// </summary>
        private void DgvEventos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verificar que estamos en la columna correcta y el valor es numérico
            if (dgvEventos.Columns[e.ColumnIndex].Name == "TiquetesDisponibles" &&
                e.Value is int valor &&
                valor == 0)
            {
                // MOSTRAR "AGOTADO" TEXTUALMENTE EN LUGAR DEL NÚMERO
                e.Value = "AGOTADO";

                // FORMATEO VISUAL PARA DESTACAR EVENTOS AGOTADOS
                e.CellStyle.ForeColor = Color.Red;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                e.CellStyle.BackColor = Color.LightGray;

                // OPCIONAL: Cambiar alineación para mejor legibilidad
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
    }
}
