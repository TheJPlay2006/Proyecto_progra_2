﻿namespace SistemaDeTickets.Vista
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
            this.lblMensajeExito = new System.Windows.Forms.Label();
            this.lblCodigoCompra = new System.Windows.Forms.Label();
            this.lblDetallesCompra = new System.Windows.Forms.Label();
            this.btnDescargarRecibo = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMensajeExito
            // 
            this.lblMensajeExito.AutoSize = true;
            this.lblMensajeExito.Location = new System.Drawing.Point(122, 99);
            this.lblMensajeExito.Name = "lblMensajeExito";
            this.lblMensajeExito.Size = new System.Drawing.Size(90, 16);
            this.lblMensajeExito.TabIndex = 0;
            this.lblMensajeExito.Text = "Mensaje exito";
            // 
            // lblCodigoCompra
            // 
            this.lblCodigoCompra.AutoSize = true;
            this.lblCodigoCompra.Location = new System.Drawing.Point(303, 99);
            this.lblCodigoCompra.Name = "lblCodigoCompra";
            this.lblCodigoCompra.Size = new System.Drawing.Size(100, 16);
            this.lblCodigoCompra.TabIndex = 1;
            this.lblCodigoCompra.Text = "Codigo compra";
            // 
            // lblDetallesCompra
            // 
            this.lblDetallesCompra.AutoSize = true;
            this.lblDetallesCompra.Location = new System.Drawing.Point(461, 99);
            this.lblDetallesCompra.Name = "lblDetallesCompra";
            this.lblDetallesCompra.Size = new System.Drawing.Size(106, 16);
            this.lblDetallesCompra.TabIndex = 2;
            this.lblDetallesCompra.Text = "Detalles compra";
            // 
            // btnDescargarRecibo
            // 
            this.btnDescargarRecibo.Location = new System.Drawing.Point(157, 241);
            this.btnDescargarRecibo.Name = "btnDescargarRecibo";
            this.btnDescargarRecibo.Size = new System.Drawing.Size(153, 39);
            this.btnDescargarRecibo.TabIndex = 3;
            this.btnDescargarRecibo.Text = "Descargar recibo";
            this.btnDescargarRecibo.UseVisualStyleBackColor = true;
            // 
            // btnVolver
            // 
            this.btnVolver.Location = new System.Drawing.Point(385, 249);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(101, 31);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            // 
            // VistaConfirmacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnDescargarRecibo);
            this.Controls.Add(this.lblDetallesCompra);
            this.Controls.Add(this.lblCodigoCompra);
            this.Controls.Add(this.lblMensajeExito);
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
    }
}