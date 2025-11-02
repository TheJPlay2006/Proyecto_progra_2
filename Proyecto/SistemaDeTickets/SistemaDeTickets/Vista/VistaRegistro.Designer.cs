namespace SistemaDeTickets.Vista
{
    partial class VistaRegistro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VistaRegistro));
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblConfirmarPassword = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.txtConfirmarPassword = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.progressBarContrasena = new System.Windows.Forms.ProgressBar();
            this.progressBarConfirmarContrasena = new System.Windows.Forms.ProgressBar();
            this.lblFortalezaContrasena = new System.Windows.Forms.Label();
            this.lblFortalezaConfirmarContrasena = new System.Windows.Forms.Label();
            this.btnOcultarVerContra = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(274, 120);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(2);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(238, 22);
            this.txtNombre.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Controls.Add(this.lblTitulo);
            this.panel1.Controls.Add(this.lblConfirmarPassword);
            this.panel1.Controls.Add(this.lblPassword);
            this.panel1.Controls.Add(this.lblEmail);
            this.panel1.Controls.Add(this.lblNombre);
            this.panel1.Controls.Add(this.btnVolver);
            this.panel1.Controls.Add(this.btnRegistrar);
            this.panel1.Controls.Add(this.txtConfirmarPassword);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.txtNombre);
            this.panel1.Controls.Add(this.progressBarContrasena);
            this.panel1.Controls.Add(this.progressBarConfirmarContrasena);
            this.panel1.Controls.Add(this.lblFortalezaContrasena);
            this.panel1.Controls.Add(this.lblFortalezaConfirmarContrasena);
            this.panel1.Controls.Add(this.btnOcultarVerContra);
            this.panel1.Location = new System.Drawing.Point(-3, -1);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(676, 462);
            this.panel1.TabIndex = 1;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lblTitulo.Font = new System.Drawing.Font("Bahnschrift Condensed", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(416, 27);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(179, 33);
            this.lblTitulo.TabIndex = 10;
            this.lblTitulo.Text = "Registro ticketera ";
            this.lblTitulo.Click += new System.EventHandler(this.lblTitulo_Click);
            // 
            // lblConfirmarPassword
            // 
            this.lblConfirmarPassword.AutoSize = true;
            this.lblConfirmarPassword.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmarPassword.Location = new System.Drawing.Point(70, 285);
            this.lblConfirmarPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConfirmarPassword.Name = "lblConfirmarPassword";
            this.lblConfirmarPassword.Size = new System.Drawing.Size(199, 25);
            this.lblConfirmarPassword.TabIndex = 9;
            this.lblConfirmarPassword.Text = "Confirmar contraseña:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(70, 234);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(117, 25);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Contraseña: ";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(70, 172);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(67, 25);
            this.lblEmail.TabIndex = 7;
            this.lblEmail.Text = "Email: ";
            // 
            // lblNombre
            // 
            this.lblNombre.AccessibleName = "";
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.Location = new System.Drawing.Point(70, 117);
            this.lblNombre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(85, 25);
            this.lblNombre.TabIndex = 6;
            this.lblNombre.Text = "Nombre:";
            // 
            // btnVolver
            // 
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVolver.Location = new System.Drawing.Point(390, 377);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(2);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(130, 37);
            this.btnVolver.TabIndex = 5;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistrar.Location = new System.Drawing.Point(151, 376);
            this.btnRegistrar.Margin = new System.Windows.Forms.Padding(2);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(131, 37);
            this.btnRegistrar.TabIndex = 4;
            this.btnRegistrar.Text = "Registrarse";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // txtConfirmarPassword
            // 
            this.txtConfirmarPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmarPassword.Location = new System.Drawing.Point(274, 288);
            this.txtConfirmarPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtConfirmarPassword.Name = "txtConfirmarPassword";
            this.txtConfirmarPassword.Size = new System.Drawing.Size(238, 22);
            this.txtConfirmarPassword.TabIndex = 3;
            this.txtConfirmarPassword.TextChanged += new System.EventHandler(this.txtConfirmarPassword_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(274, 237);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(238, 22);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(274, 175);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(2);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(238, 22);
            this.txtEmail.TabIndex = 1;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // progressBarContrasena
            // 
            this.progressBarContrasena.Location = new System.Drawing.Point(274, 265);
            this.progressBarContrasena.Name = "progressBarContrasena";
            this.progressBarContrasena.Size = new System.Drawing.Size(238, 10);
            this.progressBarContrasena.TabIndex = 11;
            // 
            // progressBarConfirmarContrasena
            // 
            this.progressBarConfirmarContrasena.Location = new System.Drawing.Point(274, 316);
            this.progressBarConfirmarContrasena.Name = "progressBarConfirmarContrasena";
            this.progressBarConfirmarContrasena.Size = new System.Drawing.Size(238, 10);
            this.progressBarConfirmarContrasena.TabIndex = 12;
            // 
            // lblFortalezaContrasena
            // 
            this.lblFortalezaContrasena.AutoSize = true;
            this.lblFortalezaContrasena.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFortalezaContrasena.Location = new System.Drawing.Point(520, 265);
            this.lblFortalezaContrasena.Name = "lblFortalezaContrasena";
            this.lblFortalezaContrasena.Size = new System.Drawing.Size(0, 13);
            this.lblFortalezaContrasena.TabIndex = 13;
            // 
            // lblFortalezaConfirmarContrasena
            // 
            this.lblFortalezaConfirmarContrasena.AutoSize = true;
            this.lblFortalezaConfirmarContrasena.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFortalezaConfirmarContrasena.Location = new System.Drawing.Point(520, 316);
            this.lblFortalezaConfirmarContrasena.Name = "lblFortalezaConfirmarContrasena";
            this.lblFortalezaConfirmarContrasena.Size = new System.Drawing.Size(0, 13);
            this.lblFortalezaConfirmarContrasena.TabIndex = 14;
            // 
            // btnOcultarVerContra
            // 
            this.btnOcultarVerContra.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOcultarVerContra.Location = new System.Drawing.Point(520, 237);
            this.btnOcultarVerContra.Name = "btnOcultarVerContra";
            this.btnOcultarVerContra.Size = new System.Drawing.Size(30, 25);
            this.btnOcultarVerContra.TabIndex = 15;
            this.btnOcultarVerContra.Text = "👁";
            this.btnOcultarVerContra.UseVisualStyleBackColor = true;
            this.btnOcultarVerContra.Click += new System.EventHandler(this.btnOcultarVerContra_Click);
            // 
            // VistaRegistro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 447);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VistaRegistro";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblConfirmarPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.TextBox txtConfirmarPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.ProgressBar progressBarContrasena;
        private System.Windows.Forms.ProgressBar progressBarConfirmarContrasena;
        private System.Windows.Forms.Label lblFortalezaContrasena;
        private System.Windows.Forms.Label lblFortalezaConfirmarContrasena;
        private System.Windows.Forms.Button btnOcultarVerContra;
    }
}