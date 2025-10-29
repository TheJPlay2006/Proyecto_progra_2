using System;

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
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnCambiarContrasena = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRecuperar
            // 
            this.btnRecuperar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecuperar.Location = new System.Drawing.Point(662, 163);
            this.btnRecuperar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRecuperar.Name = "btnRecuperar";
            this.btnRecuperar.Size = new System.Drawing.Size(177, 27);
            this.btnRecuperar.TabIndex = 0;
            this.btnRecuperar.Text = "Recuperar ";
            this.btnRecuperar.UseVisualStyleBackColor = true;
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(261, 90);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(326, 21);
            this.txtEmail.TabIndex = 1;
            // 
            // txtToken
            // 
            this.txtToken.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToken.Location = new System.Drawing.Point(261, 163);
            this.txtToken.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(326, 21);
            this.txtToken.TabIndex = 2;
            // 
            // txtNuevoPassword
            // 
            this.txtNuevoPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNuevoPassword.Location = new System.Drawing.Point(261, 244);
            this.txtNuevoPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtNuevoPassword.Name = "txtNuevoPassword";
            this.txtNuevoPassword.Size = new System.Drawing.Size(326, 21);
            this.txtNuevoPassword.TabIndex = 3;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(72, 90);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(176, 24);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Correo electrónico: ";
            this.lblEmail.Click += new System.EventHandler(this.lblEmail_Click);
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.BackColor = System.Drawing.Color.Transparent;
            this.lblToken.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToken.Location = new System.Drawing.Point(72, 160);
            this.lblToken.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(154, 24);
            this.lblToken.TabIndex = 5;
            this.lblToken.Text = "Ingrese el token: ";
            this.lblToken.Visible = false;
            this.lblToken.Click += new System.EventHandler(this.lblToken_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(72, 244);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "Nueva contraseña: ";
            // 
            // btnEnviarToken
            // 
            this.btnEnviarToken.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviarToken.Location = new System.Drawing.Point(662, 83);
            this.btnEnviarToken.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEnviarToken.Name = "btnEnviarToken";
            this.btnEnviarToken.Size = new System.Drawing.Size(177, 34);
            this.btnEnviarToken.TabIndex = 7;
            this.btnEnviarToken.Text = "Enviar token";
            this.btnEnviarToken.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(299, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Recuperación de contraseña";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(347, 303);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(145, 31);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnCambiarContrasena
            // 
            this.btnCambiarContrasena.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiarContrasena.Location = new System.Drawing.Point(662, 238);
            this.btnCambiarContrasena.Name = "btnCambiarContrasena";
            this.btnCambiarContrasena.Size = new System.Drawing.Size(177, 30);
            this.btnCambiarContrasena.TabIndex = 10;
            this.btnCambiarContrasena.Text = "Cambiar contraseña";
            this.btnCambiarContrasena.UseVisualStyleBackColor = true;
            this.btnCambiarContrasena.Click += new System.EventHandler(this.btnCambiarContrasena_Click);
            // 
            // VistaRecuperacionPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 370);
            this.Controls.Add(this.btnCambiarContrasena);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEnviarToken);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblToken);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtNuevoPassword);
            this.Controls.Add(this.txtToken);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.btnRecuperar);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnCambiarContrasena;
    }
}