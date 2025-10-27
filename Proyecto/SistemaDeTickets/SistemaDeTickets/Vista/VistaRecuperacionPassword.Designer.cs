namespace SistemaDeTickets.Vista
{
    partial class VistaRecuperacionPassword
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
            this.btnRecuperar = new System.Windows.Forms.Button();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtToken = new System.Windows.Forms.TextBox();
            this.txtNuevoPassword = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblToken = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEnviarToken = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRecuperar
            // 
            this.btnRecuperar.Location = new System.Drawing.Point(472, 193);
            this.btnRecuperar.Name = "btnRecuperar";
            this.btnRecuperar.Size = new System.Drawing.Size(89, 33);
            this.btnRecuperar.TabIndex = 0;
            this.btnRecuperar.Text = "Recuperar ";
            this.btnRecuperar.UseVisualStyleBackColor = true;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(274, 83);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(162, 22);
            this.txtEmail.TabIndex = 1;
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(297, 204);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(100, 22);
            this.txtToken.TabIndex = 2;
            // 
            // txtNuevoPassword
            // 
            this.txtNuevoPassword.Location = new System.Drawing.Point(336, 304);
            this.txtNuevoPassword.Name = "txtNuevoPassword";
            this.txtNuevoPassword.Size = new System.Drawing.Size(146, 22);
            this.txtNuevoPassword.TabIndex = 3;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(160, 83);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(54, 16);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Correo: ";
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.Location = new System.Drawing.Point(160, 204);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(108, 16);
            this.lblToken.TabIndex = 5;
            this.lblToken.Text = "Ingrese el token: ";
            this.lblToken.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(160, 304);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Nueva contraseña: ";
            // 
            // btnEnviarToken
            // 
            this.btnEnviarToken.Location = new System.Drawing.Point(482, 82);
            this.btnEnviarToken.Name = "btnEnviarToken";
            this.btnEnviarToken.Size = new System.Drawing.Size(128, 23);
            this.btnEnviarToken.TabIndex = 7;
            this.btnEnviarToken.Text = "Enviar token";
            this.btnEnviarToken.UseVisualStyleBackColor = true;
            // 
            // VistaRecuperacionPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnEnviarToken);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblToken);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtNuevoPassword);
            this.Controls.Add(this.txtToken);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.btnRecuperar);
            this.Name = "VistaRecuperacionPassword";
            this.Text = "VistaRecuperacionPassword";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRecuperar;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtToken;
        private System.Windows.Forms.TextBox txtNuevoPassword;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnEnviarToken;
    }
}