namespace Cerene_App
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
            dataTable1 = new DataGridView();
            dataOpciones = new DataGridView();
            btnTraer = new Button();
            cmbSecciones = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataTable1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataOpciones).BeginInit();
            SuspendLayout();
            // 
            // dataTable1
            // 
            dataTable1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataTable1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataTable1.Location = new Point(12, 86);
            dataTable1.Name = "dataTable1";
            dataTable1.Size = new Size(776, 250);
            dataTable1.TabIndex = 0;

            // dataOpciones
            //
            dataOpciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataOpciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataOpciones.Location = new Point(12, 350);
            dataOpciones.Name = "dataOpciones";
            dataOpciones.Size = new Size(776, 150);
            dataOpciones.TabIndex = 3;
            // 
            // btnTraer
            // 
            btnTraer.Location = new Point(21, 12);
            btnTraer.Name = "btnTraer";
            btnTraer.Size = new Size(75, 23);
            btnTraer.TabIndex = 1;
            btnTraer.Text = "button1";
            btnTraer.UseVisualStyleBackColor = true;
            btnTraer.Click += btnTraer_Click;
            // 
            // cmbSecciones
            // 
            cmbSecciones.FormattingEnabled = true;
            cmbSecciones.Location = new Point(115, 12);
            cmbSecciones.Name = "cmbSecciones";
            cmbSecciones.Size = new Size(121, 23);
            cmbSecciones.TabIndex = 2;
            cmbSecciones.SelectedIndexChanged += comboBoxSecciones_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 545);
            Controls.Add(cmbSecciones);
            Controls.Add(btnTraer);
            Controls.Add(dataOpciones);
            Controls.Add(dataTable1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataTable1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataOpciones).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataTable1;
        private DataGridView dataOpciones;
        private Button btnTraer;
        private ComboBox cmbSecciones;
    }
}
