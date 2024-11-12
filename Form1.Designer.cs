namespace Articulos_Ecommerce
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Consultar = new Button();
            Inicio = new DateTimePicker();
            Fin = new DateTimePicker();
            saveFileDialog1 = new SaveFileDialog();
            SuspendLayout();
            // 
            // Consultar
            // 
            Consultar.Cursor = Cursors.Hand;
            Consultar.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            Consultar.Location = new Point(394, 224);
            Consultar.Name = "Consultar";
            Consultar.Size = new Size(137, 51);
            Consultar.TabIndex = 0;
            Consultar.Text = "Consultar";
            Consultar.UseVisualStyleBackColor = true;
            Consultar.Click += Consultar_Click;
            // 
            // Inicio
            // 
            Inicio.CustomFormat = "MMMM dd, yyyy";
            Inicio.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            Inicio.Location = new Point(302, 45);
            Inicio.Name = "Inicio";
            Inicio.Size = new Size(341, 31);
            Inicio.TabIndex = 1;
            // 
            // Fin
            // 
            Fin.CustomFormat = "MMMM dd, yyyy";
            Fin.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            Fin.Location = new Point(302, 138);
            Fin.Name = "Fin";
            Fin.Size = new Size(341, 31);
            Fin.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(994, 525);
            Controls.Add(Fin);
            Controls.Add(Inicio);
            Controls.Add(Consultar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Buscar Folios E-Commerce";
            ResumeLayout(false);
        }

        #endregion

        private Button Consultar;
        private DateTimePicker Inicio;
        private DateTimePicker Fin;
        private SaveFileDialog saveFileDialog1;
    }
}