namespace SistemaDeTickets.Vista
{
    using System;

    partial class UserControlNotificacion
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.timerAutoOcultar = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            //
            // lblMensaje
            //
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Location = new System.Drawing.Point(10, 10);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(50, 16);
            this.lblMensaje.TabIndex = 0;
            this.lblMensaje.Text = "Mensaje";
            //
            // timerAutoOcultar
            //
            this.timerAutoOcultar.Interval = 5000;
            // Timer event handler will be set in the code-behind
            //
            // UserControlNotificacion
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMensaje);
            this.Name = "UserControlNotificacion";
            this.Size = new System.Drawing.Size(300, 40);
            this.Visible = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Timer timerAutoOcultar;
    }
}