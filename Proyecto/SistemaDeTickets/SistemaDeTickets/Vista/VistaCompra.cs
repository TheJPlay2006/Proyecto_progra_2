using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaDeTickets.Controlador;
using SistemaDeTickets.Controlador.Patrones;
using SistemaDeTickets.Modelo;
using SistemaDeTickets.Services;
using SistemaDeTickets.Utils;

namespace SistemaDeTickets.Vista
{
    /// <summary>
    /// Formulario para procesar compras de tickets.
    /// Incluye validación de datos de pago y confirmación de compra.
    /// </summary>
    public partial class VistaCompra : Form
    {
        private DetalleCompra _detalleCompra;
        private FachadaCompraTique _fachadaCompra;

        public VistaCompra(DetalleCompra detalleCompra)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema de Tickets - Procesar Compra";
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

            _detalleCompra = detalleCompra;
            _fachadaCompra = new FachadaCompraTique(
                new ControladorUsuario(),
                new ControladorCompra(),
                new GestorInventario(),
                new GestorEventos()
            );

            ConfigurarInterfazCompra();
            ConfigurarNumericUpDownCantidad();
            CargarDatosCompra();
        }

        private void ConfigurarInterfazCompra()
        {
            // Configurar título
            if (this.Controls.ContainsKey("lblTitulo"))
            {
                var lblTitulo = this.Controls["lblTitulo"] as Label;
                if (lblTitulo != null)
                {
                    lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                    lblTitulo.ForeColor = Color.FromArgb(30, 31, 59);
                    lblTitulo.Text = "Procesar Compra de Tickets";
                }
            }

            // Configurar panelDetalles como único panel de resumen (anclado a la derecha)
            var panelDetalles = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "panelDetalles");
            if (panelDetalles != null)
            {
                // Configurar panelDetalles para resumen
                panelDetalles.BackColor = Color.FromArgb(250, 250, 250);
                panelDetalles.BorderStyle = BorderStyle.FixedSingle;
                panelDetalles.Anchor = AnchorStyles.Top | AnchorStyles.Right; // Anclado arriba-derecha
                panelDetalles.Location = new Point(this.ClientSize.Width - 340, 20); // Margen derecho
                panelDetalles.Size = new Size(320, 280);
                panelDetalles.Padding = new Padding(10);
            }

            // Configurar panelErrores fijo y compacto
            var panelErrores = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "panelErrores");
            if (panelErrores != null)
            {
                // Configurar panelErrores fijo debajo del campo "Nombre del titular"
                panelErrores.BackColor = Color.MistyRose; // Fondo rosado suave para errores
                panelErrores.BorderStyle = BorderStyle.FixedSingle;
                panelErrores.Anchor = AnchorStyles.Top | AnchorStyles.Left; // Fijo, no se mueve
                panelErrores.Location = new Point(20, 320); // Posición fija debajo del campo nombre
                panelErrores.Size = new Size(400, 60); // Tamaño fijo compacto
                panelErrores.Padding = new Padding(5);
                panelErrores.AutoScroll = true; // Scroll si el contenido excede
                panelErrores.Visible = true; // Siempre visible, solo cambia contenido
            }

            // Configurar botones con colores originales (blanco como estaban antes)
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Name.Contains("Confirmar") || btn.Name.Contains("Pagar"))
                        ConfigurarBoton(btn, "Confirmar Pago", Color.White); // Blanco original
                    else if (btn.Name.Contains("Cancelar"))
                        ConfigurarBoton(btn, "Cancelar", Color.White); // Blanco original
                }
                else if (ctrl is TextBox txt)
                {
                    txt.Font = new Font("Segoe UI", 10);
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        /// <summary>
        /// Configura el control NumericUpDown existente para selección de cantidad de entradas
        /// </summary>
        private void ConfigurarNumericUpDownCantidad()
        {
            // Configurar NumericUpDown existente (creado por diseñador)
            if (numericUpDownCantidad != null)
            {
                numericUpDownCantidad.Minimum = 1;
                numericUpDownCantidad.Maximum = Math.Min(10, _detalleCompra.Evento.TiquetesDisponibles);
                numericUpDownCantidad.Value = 1; // Siempre iniciar en 1 para experiencia limpia
                numericUpDownCantidad.Font = new Font("Segoe UI", 10);

                // Conectar evento ValueChanged para actualización dinámica
                numericUpDownCantidad.ValueChanged += NumericUpDownCantidad_ValueChanged;
            }
        }

        /// <summary>
        /// Inicializa el formulario completamente vacío para experiencia real del usuario
        /// </summary>
        private void InicializarFormularioVacio()
        {
            // Asegurar que todos los campos de pago estén completamente vacíos
            txtNombreTitular.Text = string.Empty;
            txtNumeroTarjeta.Text = string.Empty;
            txtCVV.Text = string.Empty;
            txtFechaExpiracion.Text = string.Empty;

            // Configurar NumericUpDown en valor mínimo (1)
            if (numericUpDownCantidad != null)
            {
                numericUpDownCantidad.Value = 1;
            }

            // Limpiar cualquier mensaje de validación previo en panelErrores
            var panelErrores = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "panelErrores");
            if (panelErrores != null)
            {
                panelErrores.Controls.Clear();
                panelErrores.Visible = false;
            }

            // Asegurar fondos blancos en campos (sin colores de validación previa)
            txtNombreTitular.BackColor = Color.White;
            txtNumeroTarjeta.BackColor = Color.White;
            txtCVV.BackColor = Color.White;
            txtFechaExpiracion.BackColor = Color.White;

            // Inicializar estado del botón (deshabilitado al inicio)
            ActualizarEstadoBotonConfirmar();

            // Actualizar resumen con cantidad inicial (1)
            ConstruirResumenEnPanelDetalles();
        }

        /// <summary>
        /// Evento que se dispara cuando cambia la cantidad seleccionada
        /// </summary>
        private void NumericUpDownCantidad_ValueChanged(object sender, EventArgs e)
        {
            // Actualizar el detalle de compra con la nueva cantidad
            _detalleCompra.Cantidad = (int)numericUpDownCantidad.Value;
            _detalleCompra.Total = (decimal)(_detalleCompra.Cantidad * _detalleCompra.Evento.Precio);

            // Actualizar labels existentes
            if (this.Controls.ContainsKey("lblTotal"))
                lblTotal.Text = $"₡{_detalleCompra.Total:N0}";

            // Reconstruir resumen en panelDetalles con datos actualizados
            ConstruirResumenEnPanelDetalles();

            // Actualizar estado del botón confirmar
            ActualizarEstadoBotonConfirmar();
        }

        private void ConfigurarBoton(Button btn, string texto, Color colorFondo)
        {
            btn.Text = texto;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.BackColor = colorFondo;
            btn.ForeColor = Color.Black;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Height = 40;
            btn.Width = 150;

            // Mantener colores originales del sistema (blanco)
            if (btn.Name.Contains("Confirmar") || btn.Name.Contains("Pagar"))
                btn.BackColor = Color.White; // Blanco original
            else if (btn.Name.Contains("Cancelar"))
                btn.BackColor = Color.White; // Blanco original
            btn.Cursor = Cursors.Hand;
        }

        private void CargarDatosCompra()
        {
            // Cargar datos en labels existentes
            if (this.Controls.ContainsKey("lblNombreEvento"))
                lblNombreEvento.Text = _detalleCompra.Evento.Nombre;

            if (this.Controls.ContainsKey("lblFecha"))
                lblFecha.Text = _detalleCompra.Evento.Fecha.ToString("dd/MM/yyyy");

            // NO configurar NumericUpDown con cantidad del detalle - mantener vacío/inicial
            // La inicialización se hace en InicializarFormularioVacio()

            if (this.Controls.ContainsKey("lblTotal"))
                lblTotal.Text = $"₡{_detalleCompra.Total:N0}";

            // Construir resumen del pedido en panelDetalles
            ConstruirResumenEnPanelDetalles();
        }

        /// <summary>
        /// Construye el resumen del pedido únicamente en panelDetalles
        /// </summary>
        private void ConstruirResumenEnPanelDetalles()
        {
            var panelDetalles = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "panelDetalles");
            if (panelDetalles == null)
                return;

            // Limpiar controles anteriores del panelDetalles
            panelDetalles.Controls.Clear();

            int yPos = 10;

            // Título del resumen (más grande y en negrita)
            var lblResumenTitulo = new Label();
            lblResumenTitulo.Text = "RESUMEN DEL PEDIDO";
            lblResumenTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblResumenTitulo.ForeColor = Color.FromArgb(30, 31, 59);
            lblResumenTitulo.Location = new Point(10, yPos);
            lblResumenTitulo.Size = new Size(300, 25);
            lblResumenTitulo.TextAlign = ContentAlignment.MiddleCenter;
            panelDetalles.Controls.Add(lblResumenTitulo);
            yPos += 35;

            // Línea separadora
            var lineaSuperior = new Label();
            lineaSuperior.BorderStyle = BorderStyle.Fixed3D;
            lineaSuperior.Location = new Point(10, yPos);
            lineaSuperior.Size = new Size(300, 2);
            panelDetalles.Controls.Add(lineaSuperior);
            yPos += 15;

            // Información del evento (fuente normal)
            var lblEvento = new Label();
            lblEvento.Text = $"Evento: {_detalleCompra.Evento.Nombre}";
            lblEvento.Font = new Font("Segoe UI", 10);
            lblEvento.Location = new Point(10, yPos);
            lblEvento.Size = new Size(300, 20);
            panelDetalles.Controls.Add(lblEvento);
            yPos += 25;

            var lblFechaHora = new Label();
            lblFechaHora.Text = $"Fecha y hora: {_detalleCompra.Evento.Fecha:dd/MM/yyyy HH:mm}";
            lblFechaHora.Font = new Font("Segoe UI", 10);
            lblFechaHora.Location = new Point(10, yPos);
            lblFechaHora.Size = new Size(300, 20);
            panelDetalles.Controls.Add(lblFechaHora);
            yPos += 25;

            var lblRecinto = new Label();
            lblRecinto.Text = $"Recinto: {_detalleCompra.Evento.Recinto}";
            lblRecinto.Font = new Font("Segoe UI", 10);
            lblRecinto.Location = new Point(10, yPos);
            lblRecinto.Size = new Size(300, 20);
            panelDetalles.Controls.Add(lblRecinto);
            yPos += 25;

            var lblCantidad = new Label();
            lblCantidad.Text = $"Cantidad de entradas: {_detalleCompra.Cantidad}";
            lblCantidad.Font = new Font("Segoe UI", 10);
            lblCantidad.Location = new Point(10, yPos);
            lblCantidad.Size = new Size(300, 20);
            panelDetalles.Controls.Add(lblCantidad);
            yPos += 25;

            var lblPrecioUnitario = new Label();
            lblPrecioUnitario.Text = $"Precio unitario: ₡{_detalleCompra.Evento.Precio:N0}";
            lblPrecioUnitario.Font = new Font("Segoe UI", 10);
            lblPrecioUnitario.Location = new Point(10, yPos);
            lblPrecioUnitario.Size = new Size(300, 20);
            panelDetalles.Controls.Add(lblPrecioUnitario);
            yPos += 25;

            // Línea separadora antes del total
            var lineaInferior = new Label();
            lineaInferior.BorderStyle = BorderStyle.Fixed3D;
            lineaInferior.Location = new Point(10, yPos);
            lineaInferior.Size = new Size(300, 2);
            panelDetalles.Controls.Add(lineaInferior);
            yPos += 15;

            // Total (más grande y en negrita, color verde)
            var lblTotal = new Label();
            lblTotal.Text = $"TOTAL A PAGAR: ₡{_detalleCompra.Total:N0}";
            lblTotal.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(76, 175, 80); // Verde
            lblTotal.Location = new Point(10, yPos);
            lblTotal.Size = new Size(300, 30);
            lblTotal.TextAlign = ContentAlignment.MiddleCenter;
            panelDetalles.Controls.Add(lblTotal);
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            // Validar cantidad seleccionada contra stock disponible
            int cantidadSeleccionada = (int)numericUpDownCantidad.Value;
            if (cantidadSeleccionada > _detalleCompra.Evento.TiquetesDisponibles)
            {
                return;
            }

            // Validar todos los campos antes de procesar
            if (!ValidarCamposPago())
                return;

            string numeroTarjeta = txtNumeroTarjeta.Text.Trim();
            string cvv = txtCVV.Text.Trim();

            try
            {
                // Actualizar detalle de compra con cantidad seleccionada
                _detalleCompra.Cantidad = cantidadSeleccionada;
                _detalleCompra.Total = (decimal)(cantidadSeleccionada * _detalleCompra.Evento.Precio);

                // Procesar pago
                bool pagoExitoso = _fachadaCompra.ProcesarPago(numeroTarjeta, cvv, _detalleCompra);

                if (pagoExitoso)
                {
                    // Confirmar compra (esto incluye actualizar stock)
                    Compra compraConfirmada = _fachadaCompra.ConfirmarCompra(_detalleCompra);

                    // DEBUG: Verificar que el JSON se actualizó
                    var eventosDebug = GestorJSON.LeerArchivo<List<Modelo.Evento>>("Data/MisEventos.json") ?? new List<Modelo.Evento>();
                    // Debug eliminado

                    // Navegar a confirmación con datos de la compra
                    this.Hide();
                    var confirmacionForm = new VistaConfirmacion(compraConfirmada);
                    confirmacionForm.StartPosition = FormStartPosition.CenterScreen;
                    confirmacionForm.ShowDialog();

                    // REFRESCO FORZADO: Cerrar VistaCompra y forzar refresco en VistaEvento
                    // Buscar instancia existente de VistaEvento y refrescarla
                    foreach (Form form in Application.OpenForms)
                    {
                        if (form is VistaEvento vistaEvento && !form.IsDisposed)
                        {
                            vistaEvento.RefrescarVistaDesdeCompra();
                            break;
                        }
                    }
                    this.Close(); // Cerrar VistaCompra después de confirmación
                }
                else
                {
                    // Error silencioso
                }
            }
            catch (Exception ex)
            {
                // Error silencioso - variable ex no utilizada
            }
        }

        private bool ValidarCamposPago()
        {
            string numeroTarjeta = txtNumeroTarjeta.Text.Trim();
            string cvv = txtCVV.Text.Trim();
            string fechaExpiracion = txtFechaExpiracion.Text.Trim();
            string nombreTitular = txtNombreTitular.Text.Trim();

            // Validar campos vacíos
            if (string.IsNullOrEmpty(numeroTarjeta) || string.IsNullOrEmpty(cvv) ||
                string.IsNullOrEmpty(fechaExpiracion) || string.IsNullOrEmpty(nombreTitular))
            {
                MostrarMensajeValidacion("Por favor, complete todos los campos de pago.", Color.Red);
                return false;
            }

            // Validar número de tarjeta (con Luhn)
            if (!ValidarNumeroTarjeta(numeroTarjeta))
            {
                MostrarMensajeValidacion("Número de tarjeta inválido.", Color.Red);
                txtNumeroTarjeta.Focus();
                return false;
            }

            // Validar CVV
            if (!ValidadorTarjeta.ValidarCVV(cvv))
            {
                MostrarMensajeValidacion("CVV inválido.", Color.Red);
                txtCVV.Focus();
                return false;
            }

            // Validar fecha de expiración
            if (!ValidadorTarjeta.ValidarFechaExpiracion(fechaExpiracion))
            {
                MostrarMensajeValidacion("Fecha de expiración inválida.", Color.Red);
                txtFechaExpiracion.Focus();
                return false;
            }

            // Validar nombre del titular
            if (!ValidadorTarjeta.ValidarNombreTitular(nombreTitular))
            {
                MostrarMensajeValidacion("Nombre del titular inválido.", Color.Red);
                txtNombreTitular.Focus();
                return false;
            }

            MostrarMensajeValidacion("Datos válidos. Puede proceder con el pago.", Color.Green);
            return true;
        }

        /// <summary>
        /// Valida número de tarjeta usando algoritmo Luhn realista
        /// Soporta Visa (16 dígitos), Mastercard (16 dígitos), Amex (15 dígitos)
        /// </summary>
        private bool ValidarNumeroTarjeta(string numero)
        {

            // Paso 1: Limpiar entrada (remover espacios)
            if (string.IsNullOrWhiteSpace(numero))
            {
                return false;
            }

            numero = numero.Replace(" ", "").Replace("-", "");

            // Paso 2: Verificar formato (solo dígitos)
            if (!System.Text.RegularExpressions.Regex.IsMatch(numero, @"^\d+$"))
            {
                return false;
            }

            // Paso 3: Verificar longitud según tipo de tarjeta
            bool longitudValida = false;

            if (numero.Length == 16)
            {
                // Visa o Mastercard
                if (numero.StartsWith("4") || numero.StartsWith("5") || numero.StartsWith("2"))
                {
                    longitudValida = true;
                }
            }
            else if (numero.Length == 15 && numero.StartsWith("3"))
            {
                // American Express
                longitudValida = true;
            }

            if (!longitudValida)
            {
                return false;
            }

            // Paso 4: Algoritmo Luhn
            int sum = 0;
            bool alternate = false;

            // Procesar dígitos de derecha a izquierda
            for (int i = numero.Length - 1; i >= 0; i--)
            {
                int digit = numero[i] - '0';

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9; // Equivalente a sumar dígitos
                }

                sum += digit;
                alternate = !alternate;
            }

            bool esValido = sum % 10 == 0;

            return esValido;
        }

        /// <summary>
        /// Muestra mensajes de validación en el panelErrores fijo
        /// Panel siempre visible con tamaño fijo, contenido con scroll si es necesario
        /// </summary>
        private void MostrarMensajeValidacion(string mensaje, Color color)
        {
            var panelErrores = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "panelErrores");

            if (panelErrores != null)
            {
                // Limpiar controles anteriores en panelErrores
                panelErrores.Controls.Clear();

                // Cambiar fondo según tipo de mensaje
                if (color == Color.Green || mensaje.Contains("correctos") || mensaje.Contains("válido"))
                {
                    panelErrores.BackColor = Color.Honeydew; // Verde claro para éxito
                }
                else
                {
                    panelErrores.BackColor = Color.MistyRose; // Rosado para errores
                }

                // Crear label fijo que ocupe todo el panel
                var lblErrores = new Label();
                lblErrores.Name = "lblErrores";
                lblErrores.Text = string.IsNullOrEmpty(mensaje) ? "✔ Todos los datos correctos. Puede confirmar la compra." : mensaje;
                lblErrores.ForeColor = string.IsNullOrEmpty(mensaje) ? Color.Green : color;
                lblErrores.Font = new Font("Segoe UI", 9); // Fuente estándar, no bold
                lblErrores.AutoSize = false;
                lblErrores.Dock = DockStyle.Fill; // Ocupa todo el panel fijo
                lblErrores.TextAlign = ContentAlignment.TopLeft; // Alineado arriba-izquierda
                lblErrores.Padding = new Padding(5);
                lblErrores.AutoEllipsis = true; // Muestra "..." si el texto es muy largo

                panelErrores.Controls.Add(lblErrores);

                // Panel siempre visible con tamaño fijo - no cambia altura ni posición
                panelErrores.Visible = true;
            }

        }

        // Validación en tiempo real
        private void txtNumeroTarjeta_TextChanged(object sender, EventArgs e)
        {
            string numero = txtNumeroTarjeta.Text.Trim();
            if (!string.IsNullOrEmpty(numero))
            {
                if (ValidarNumeroTarjeta(numero))
                {
                    txtNumeroTarjeta.BackColor = Color.LightGreen;
                    // Mostrar marca detectada
                    string marca = DetectarMarcaTarjeta(numero);
                    MostrarMensajeValidacion($"✓ Número válido - {marca}", Color.Green);
                }
                else
                {
                    txtNumeroTarjeta.BackColor = Color.LightCoral;
                    MostrarMensajeValidacion("✖ Número de tarjeta inválido: debe contener 16 dígitos y pasar la validación.", Color.Red);
                }
            }
            else
            {
                txtNumeroTarjeta.BackColor = Color.White;
                MostrarMensajeValidacion("", Color.Black);
            }
            ActualizarEstadoBotonConfirmar();
        }

        /// <summary>
        /// Detecta la marca de la tarjeta basado en el BIN
        /// </summary>
        private string DetectarMarcaTarjeta(string numero)
        {
            if (numero.StartsWith("4"))
                return "Visa";
            else if (numero.StartsWith("5") || numero.StartsWith("2"))
                return "Mastercard";
            else if (numero.StartsWith("3"))
                return "American Express";
            else
                return "Desconocida";
        }

        private void txtCVV_TextChanged(object sender, EventArgs e)
        {
            string cvv = txtCVV.Text.Trim();
            if (!string.IsNullOrEmpty(cvv))
            {
                if (ValidadorTarjeta.ValidarCVV(cvv))
                {
                    txtCVV.BackColor = Color.LightGreen;
                    MostrarMensajeValidacion("CVV válido", Color.Green);
                }
                else
                {
                    txtCVV.BackColor = Color.LightCoral;
                    MostrarMensajeValidacion("CVV inválido (3-4 dígitos)", Color.Red);
                }
            }
            else
            {
                txtCVV.BackColor = Color.White;
                MostrarMensajeValidacion("", Color.Black);
            }
            ActualizarEstadoBotonConfirmar();
        }

        private void txtFechaExpiracion_TextChanged(object sender, EventArgs e)
        {
            string fecha = txtFechaExpiracion.Text.Trim();
            if (!string.IsNullOrEmpty(fecha))
            {
                if (ValidadorTarjeta.ValidarFechaExpiracion(fecha))
                {
                    txtFechaExpiracion.BackColor = Color.LightGreen;
                    MostrarMensajeValidacion("Fecha de expiración válida", Color.Green);
                }
                else
                {
                    txtFechaExpiracion.BackColor = Color.LightCoral;
                    MostrarMensajeValidacion("Fecha inválida (MM/YY ≥ fecha actual)", Color.Red);
                }
            }
            else
            {
                txtFechaExpiracion.BackColor = Color.White;
                MostrarMensajeValidacion("", Color.Black);
            }
            ActualizarEstadoBotonConfirmar();
        }

        private void txtNombreTitular_TextChanged(object sender, EventArgs e)
        {
            string nombre = txtNombreTitular.Text.Trim();
            if (!string.IsNullOrEmpty(nombre))
            {
                if (ValidadorTarjeta.ValidarNombreTitular(nombre))
                {
                    txtNombreTitular.BackColor = Color.LightGreen;
                    MostrarMensajeValidacion("Nombre del titular válido", Color.Green);
                }
                else
                {
                    txtNombreTitular.BackColor = Color.LightCoral;
                    MostrarMensajeValidacion("Nombre del titular inválido", Color.Red);
                }
            }
            else
            {
                txtNombreTitular.BackColor = Color.White;
                MostrarMensajeValidacion("", Color.Black);
            }
            ActualizarEstadoBotonConfirmar();
        }

        private void ActualizarEstadoBotonConfirmar()
        {
            // DEPURACIÓN PROFUNDA: Verificar cada condición paso a paso

            // 1. Verificar campos llenos
            string numeroTarjeta = txtNumeroTarjeta.Text.Trim();
            string cvv = txtCVV.Text.Trim();
            string fechaExpiracion = txtFechaExpiracion.Text.Trim();
            string nombreTitular = txtNombreTitular.Text.Trim();

            bool camposLlenos = !string.IsNullOrEmpty(numeroTarjeta) &&
                               !string.IsNullOrEmpty(cvv) &&
                               !string.IsNullOrEmpty(fechaExpiracion) &&
                               !string.IsNullOrEmpty(nombreTitular);

            // 2. Verificar campos válidos
            bool numeroValido = ValidarNumeroTarjeta(numeroTarjeta);
            bool cvvValido = ValidadorTarjeta.ValidarCVV(cvv);
            bool fechaValida = ValidadorTarjeta.ValidarFechaExpiracion(fechaExpiracion);
            bool nombreValido = ValidadorTarjeta.ValidarNombreTitular(nombreTitular);

            bool camposValidos = numeroValido && cvvValido && fechaValida && nombreValido;

            // 3. Verificar cantidad
            int cantidadSeleccionada = numericUpDownCantidad != null ? (int)numericUpDownCantidad.Value : 0;
            bool cantidadValida = cantidadSeleccionada > 0 &&
                                cantidadSeleccionada <= _detalleCompra.Evento.TiquetesDisponibles;

            // 4. Resultado final
            bool todosValidos = camposLlenos && camposValidos && cantidadValida;

            // DEPURACIÓN: Mostrar estado detallado
            string debugInfo = $"DEBUG VALIDACIÓN:\n" +
                             $"Campos llenos: {camposLlenos} (Num:{numeroTarjeta.Length}, CVV:{cvv.Length}, Fecha:{fechaExpiracion.Length}, Nombre:{nombreTitular.Length})\n" +
                             $"Campos válidos: {camposValidos} (Num:{numeroValido}, CVV:{cvvValido}, Fecha:{fechaValida}, Nombre:{nombreValido})\n" +
                             $"Cantidad válida: {cantidadValida} (Sel:{cantidadSeleccionada}, Disp:{_detalleCompra.Evento.TiquetesDisponibles})\n" +
                             $"TOTAL VÁLIDO: {todosValidos}";


            // Actualizar botón
            var btnConfirmar = this.Controls.OfType<Button>().FirstOrDefault(b => b.Name.Contains("Confirmar") || b.Name.Contains("Pagar"));
            if (btnConfirmar != null)
            {
                btnConfirmar.Enabled = todosValidos;
                btnConfirmar.BackColor = todosValidos ? Color.FromArgb(76, 175, 80) : Color.Gray;

                // Mostrar feedback al usuario
                if (todosValidos)
                {
                    MostrarMensajeValidacion("✅ TODOS LOS DATOS SON VÁLIDOS. Puede confirmar la compra.", Color.Green);
                }
                else
                {
                    string mensajeError = "❌ Datos incompletos o inválidos:\n";
                    if (!camposLlenos)
                    {
                        mensajeError += "• Complete todos los campos de pago\n";
                    }
                    else if (!camposValidos)
                    {
                        mensajeError += "• Corrija los datos inválidos:\n";
                        if (!numeroValido) mensajeError += "  - Número de tarjeta inválido\n";
                        if (!cvvValido) mensajeError += "  - CVV inválido\n";
                        if (!fechaValida) mensajeError += "  - Fecha de expiración inválida\n";
                        if (!nombreValido) mensajeError += "  - Nombre del titular inválido\n";
                    }
                    if (!cantidadValida)
                    {
                        mensajeError += $"• Cantidad inválida (máximo: {_detalleCompra.Evento.TiquetesDisponibles})\n";
                    }
                    MostrarMensajeValidacion(mensajeError.Trim(), Color.Red);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Cancelar compra y volver a vista de eventos con refresco
            _fachadaCompra.CancelarCompra(0); // TODO: Obtener ID de compra real
            this.Hide();
            var eventosForm = new VistaEvento();
            eventosForm.StartPosition = FormStartPosition.CenterScreen;
            eventosForm.ShowDialog();

            // Forzar refresco de VistaEvento al regresar
            if (eventosForm != null && !eventosForm.IsDisposed)
            {
                eventosForm.RefrescarVistaDesdeCompra();
            }

            if (!this.IsDisposed)
            {
                this.Close(); // Cerrar VistaCompra después de volver
            }
        }

        private void VistaCompra_Load(object sender, EventArgs e)
        {
            // Inicializar formulario completamente vacío para experiencia real del usuario
            InicializarFormularioVacio();

            // DEPURACIÓN: Agregar botón temporal de debug (solo para desarrollo)
            // Comentar/descomentar según necesidad de desarrollo
            // AgregarBotonDebug();
        }

        /// <summary>
        /// Carga datos de ejemplo válidos para desarrollo/debugging (NO USAR EN PRODUCCIÓN)
        /// Solo activar en modo desarrollo para pruebas
        /// </summary>
        private void CargarDatosEjemploValidos()
        {
            // ⚠️ SOLO PARA DESARROLLO - NO ACTIVAR EN PRODUCCIÓN
            // Para activar temporalmente: descomentar las líneas siguientes

            /*
            // Datos de ejemplo que pasan todas las validaciones
            txtNombreTitular.Text = "Jairo Steven Herrera Romero";
            txtNumeroTarjeta.Text = "4111111111111111"; // Número Visa válido (pasa Luhn)
            txtCVV.Text = "123"; // CVV válido de 3 dígitos
            txtFechaExpiracion.Text = "10/2028"; // Fecha futura válida

            // Configurar cantidad (asegurarse de que no exceda el stock)
            if (numericUpDownCantidad != null)
            {
                int cantidadEjemplo = Math.Min(3, _detalleCompra.Evento.TiquetesDisponibles);
                numericUpDownCantidad.Value = cantidadEjemplo;
            }

            // Mostrar mensaje explicativo
            MostrarMensajeValidacion(
                "📋 DATOS DE EJEMPLO CARGADOS (MODO DESARROLLO):\n" +
                "• Nombre: Jairo Steven Herrera Romero\n" +
                "• Tarjeta: 4111111111111111 (Visa válida)\n" +
                "• CVV: 123\n" +
                "• Fecha: 10/2028\n" +
                $"• Cantidad: {Math.Min(3, _detalleCompra.Evento.TiquetesDisponibles)}\n\n" +
                "✅ TODOS LOS DATOS SON VÁLIDOS. El botón debería estar habilitado.",
                Color.Blue
            );

            // Forzar actualización del estado después de cargar datos
            this.BeginInvoke(new Action(() =>
            {
                ActualizarEstadoBotonConfirmar();
            }));
            */
        }

        /// <summary>
        /// Agrega botón temporal de debug para verificar validaciones (SOLO DESARROLLO)
        /// ⚠️ COMENTADO PARA PRODUCCIÓN - Descomentar solo para debugging
        /// </summary>
        private void AgregarBotonDebug()
        {
            // ⚠️ SOLO PARA DESARROLLO - NO ACTIVAR EN PRODUCCIÓN
            // Para activar temporalmente: descomentar el código siguiente

            /*
            var btnDebug = new Button();
            btnDebug.Text = "🔍 DEBUG";
            btnDebug.Size = new Size(80, 30);
            btnDebug.Location = new Point(10, 400);
            btnDebug.BackColor = Color.Yellow;
            btnDebug.Click += (s, e) =>
            {
                string debug = $"ESTADO ACTUAL:\n\n" +
                              $"Número tarjeta: '{txtNumeroTarjeta.Text}' (Largo: {txtNumeroTarjeta.Text.Length})\n" +
                              $"CVV: '{txtCVV.Text}' (Largo: {txtCVV.Text.Length})\n" +
                              $"Fecha: '{txtFechaExpiracion.Text}' (Largo: {txtFechaExpiracion.Text.Length})\n" +
                              $"Nombre: '{txtNombreTitular.Text}' (Largo: {txtNombreTitular.Text.Length})\n" +
                              $"Cantidad: {numericUpDownCantidad?.Value ?? 0}\n" +
                              $"Stock disponible: {_detalleCompra.Evento.TiquetesDisponibles}\n\n" +
                              $"VALIDACIONES:\n" +
                              $"Número válido: {ValidarNumeroTarjeta(txtNumeroTarjeta.Text.Trim())}\n" +
                              $"CVV válido: {ValidadorTarjeta.ValidarCVV(txtCVV.Text.Trim())}\n" +
                              $"Fecha válida: {ValidadorTarjeta.ValidarFechaExpiracion(txtFechaExpiracion.Text.Trim())}\n" +
                              $"Nombre válido: {ValidadorTarjeta.ValidarNombreTitular(txtNombreTitular.Text.Trim())}\n" +
                              $"Cantidad válida: {(numericUpDownCantidad != null && (int)numericUpDownCantidad.Value > 0 && (int)numericUpDownCantidad.Value <= _detalleCompra.Evento.TiquetesDisponibles)}\n";

            };

            this.Controls.Add(btnDebug);

            // Agregar botón para cargar datos de ejemplo
            var btnEjemplo = new Button();
            btnEjemplo.Text = "📝 EJEMPLO";
            btnEjemplo.Size = new Size(80, 30);
            btnEjemplo.Location = new Point(100, 400);
            btnEjemplo.BackColor = Color.LightGreen;
            btnEjemplo.Click += (s, e) =>
            {
                CargarDatosEjemploValidos();
            };

            this.Controls.Add(btnEjemplo);
            */
        }
    }
}
