namespace SistemaDeTickets.Vista
{
    partial class VistaDetalleEvento
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNombreEvento = new System.Windows.Forms.Label();
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblDisponibles = new System.Windows.Forms.Label();
            this.btnComprar = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.numCantidad = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblNombreEvento
            // 
            this.lblNombreEvento.AutoSize = true;
            this.lblNombreEvento.Location = new System.Drawing.Point(86, 50);
            this.lblNombreEvento.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombreEvento.Name = "lblNombreEvento";
            this.lblNombreEvento.Size = new System.Drawing.Size(80, 13);
            this.lblNombreEvento.TabIndex = 0;
            this.lblNombreEvento.Text = "Nombre evento";
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(86, 94);
            this.lblFecha.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(79, 13);
            this.lblFecha.TabIndex = 1;
            this.lblFecha.Text = "Fecha evento: ";
            // 
            // lblPrecio
            // 
            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Location = new System.Drawing.Point(86, 135);
            this.lblPrecio.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(73, 13);
            this.lblPrecio.TabIndex = 2;
            this.lblPrecio.Text = "Precio evento";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(88, 172);
            this.lblDescripcion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(99, 13);
            this.lblDescripcion.TabIndex = 3;
            this.lblDescripcion.Text = "Descripcion evento";
            // 
            // lblDisponibles
            // 
            this.lblDisponibles.AutoSize = true;
            this.lblDisponibles.Location = new System.Drawing.Point(88, 230);
            this.lblDisponibles.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDisponibles.Name = "lblDisponibles";
            this.lblDisponibles.Size = new System.Drawing.Size(61, 13);
            this.lblDisponibles.TabIndex = 4;
            this.lblDisponibles.Text = "Disponibles";
            // 
            // btnComprar
            // 
            this.btnComprar.BackColor = System.Drawing.Color.Transparent;
            this.btnComprar.Location = new System.Drawing.Point(164, 283);
            this.btnComprar.Margin = new System.Windows.Forms.Padding(2);
            this.btnComprar.Name = "btnComprar";
            this.btnComprar.Size = new System.Drawing.Size(112, 40);
            this.btnComprar.TabIndex = 5;
            this.btnComprar.Text = "Comprar";
            this.btnComprar.UseVisualStyleBackColor = false;
            this.btnComprar.Click += new System.EventHandler(this.btnComprar_Click);
            // 
            // btnVolver
            // 
            this.btnVolver.Location = new System.Drawing.Point(318, 283);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(2);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(116, 40);
            this.btnVolver.TabIndex = 6;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // numCantidad
            // 
            this.numCantidad.AutoSize = true;
            this.numCantidad.Location = new System.Drawing.Point(303, 102);
            this.numCantidad.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.numCantidad.Name = "numCantidad";
            this.numCantidad.Size = new System.Drawing.Size(71, 13);
            this.numCantidad.TabIndex = 7;
            this.numCantidad.Text = "num cantidad";
            // 
            // VistaDetalleEvento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 406);
            this.Controls.Add(this.numCantidad);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnComprar);
            this.Controls.Add(this.lblDisponibles);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.lblPrecio);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.lblNombreEvento);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VistaDetalleEvento";
            this.Text = "VistaDetalleEvento";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNombreEvento;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Label lblDisponibles;
        private System.Windows.Forms.Button btnComprar;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Label numCantidad;
    }
}