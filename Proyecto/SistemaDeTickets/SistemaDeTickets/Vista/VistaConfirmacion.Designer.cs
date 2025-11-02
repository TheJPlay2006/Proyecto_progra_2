namespace SistemaDeTickets.Vista
{
    partial class VistaConfirmacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VistaConfirmacion));
            this.lblMensajeExito = new System.Windows.Forms.Label();
            this.lblCodigoCompra = new System.Windows.Forms.Label();
            this.lblDetallesCompra = new System.Windows.Forms.Label();
            this.btnDescargarRecibo = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnRealizarOtraCompra = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMensajeExito
            // 
            this.lblMensajeExito.AutoSize = true;
            this.lblMensajeExito.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensajeExito.Location = new System.Drawing.Point(59, 86);
            this.lblMensajeExito.Name = "lblMensajeExito";
            this.lblMensajeExito.Size = new System.Drawing.Size(105, 21);
            this.lblMensajeExito.TabIndex = 0;
            this.lblMensajeExito.Text = "Mensaje exito";
            // 
            // lblCodigoCompra
            // 
            this.lblCodigoCompra.AutoSize = true;
            this.lblCodigoCompra.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigoCompra.Location = new System.Drawing.Point(338, 86);
            this.lblCodigoCompra.Name = "lblCodigoCompra";
            this.lblCodigoCompra.Size = new System.Drawing.Size(117, 21);
            this.lblCodigoCompra.TabIndex = 1;
            this.lblCodigoCompra.Text = "Codigo compra";
            // 
            // lblDetallesCompra
            // 
            this.lblDetallesCompra.AutoSize = true;
            this.lblDetallesCompra.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetallesCompra.Location = new System.Drawing.Point(605, 86);
            this.lblDetallesCompra.Name = "lblDetallesCompra";
            this.lblDetallesCompra.Size = new System.Drawing.Size(122, 21);
            this.lblDetallesCompra.TabIndex = 2;
            this.lblDetallesCompra.Text = "Detalles compra";
            // 
            // btnDescargarRecibo
            // 
            this.btnDescargarRecibo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDescargarRecibo.Location = new System.Drawing.Point(63, 292);
            this.btnDescargarRecibo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDescargarRecibo.Name = "btnDescargarRecibo";
            this.btnDescargarRecibo.Size = new System.Drawing.Size(153, 39);
            this.btnDescargarRecibo.TabIndex = 3;
            this.btnDescargarRecibo.Text = "Descargar recibo";
            this.btnDescargarRecibo.UseVisualStyleBackColor = true;
            // 
            // btnVolver
            // 
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVolver.Location = new System.Drawing.Point(574, 292);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(153, 39);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            // 
            // btnRealizarOtraCompra
            // 
            this.btnRealizarOtraCompra.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRealizarOtraCompra.Location = new System.Drawing.Point(317, 292);
            this.btnRealizarOtraCompra.Name = "btnRealizarOtraCompra";
            this.btnRealizarOtraCompra.Size = new System.Drawing.Size(170, 39);
            this.btnRealizarOtraCompra.TabIndex = 5;
            this.btnRealizarOtraCompra.Text = "Realizar otra compra";
            this.btnRealizarOtraCompra.UseVisualStyleBackColor = true;
            // 
            // VistaConfirmacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(791, 374);
            this.Controls.Add(this.btnRealizarOtraCompra);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnDescargarRecibo);
            this.Controls.Add(this.lblDetallesCompra);
            this.Controls.Add(this.lblCodigoCompra);
            this.Controls.Add(this.lblMensajeExito);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "VistaConfirmacion";
            this.Text = "VistaConfirmacion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMensajeExito;
        private System.Windows.Forms.Label lblCodigoCompra;
        private System.Windows.Forms.Label lblDetallesCompra;
        private System.Windows.Forms.Button btnDescargarRecibo;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Button btnRealizarOtraCompra;
    }
}