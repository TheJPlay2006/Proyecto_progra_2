namespace SistemaDeTickets.Vista
{
    partial class VistaHistorial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VistaHistorial));
            this.dataGridViewCompras = new System.Windows.Forms.DataGridView();
            this.btnDescargarRecibo = new System.Windows.Forms.Button();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCompras)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewCompras
            // 
            this.dataGridViewCompras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCompras.Location = new System.Drawing.Point(9, 11);
            this.dataGridViewCompras.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewCompras.Name = "dataGridViewCompras";
            this.dataGridViewCompras.RowHeadersWidth = 51;
            this.dataGridViewCompras.RowTemplate.Height = 24;
            this.dataGridViewCompras.Size = new System.Drawing.Size(806, 359);
            this.dataGridViewCompras.TabIndex = 0;
            // 
            // btnDescargarRecibo
            // 
            this.btnDescargarRecibo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDescargarRecibo.Location = new System.Drawing.Point(70, 399);
            this.btnDescargarRecibo.Margin = new System.Windows.Forms.Padding(2);
            this.btnDescargarRecibo.Name = "btnDescargarRecibo";
            this.btnDescargarRecibo.Size = new System.Drawing.Size(158, 37);
            this.btnDescargarRecibo.TabIndex = 1;
            this.btnDescargarRecibo.Text = "Descargar recibo";
            this.btnDescargarRecibo.UseVisualStyleBackColor = true;
            this.btnDescargarRecibo.Click += new System.EventHandler(this.btnDescargarRecibo_Click);
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiltrar.Location = new System.Drawing.Point(334, 399);
            this.btnFiltrar.Margin = new System.Windows.Forms.Padding(2);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(158, 37);
            this.btnFiltrar.TabIndex = 2;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // btnVolver
            // 
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVolver.Location = new System.Drawing.Point(600, 399);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(2);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(158, 37);
            this.btnVolver.TabIndex = 3;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // VistaHistorial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(826, 472);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.btnDescargarRecibo);
            this.Controls.Add(this.dataGridViewCompras);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VistaHistorial";
            this.Text = "VistaHistorial";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCompras)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewCompras;
        private System.Windows.Forms.Button btnDescargarRecibo;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnVolver;
    }
}