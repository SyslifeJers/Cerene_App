using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cerene_App
{
    public class PreguntaForm : Form
    {
        public Pregunta? Result { get; private set; }

        private NumericUpDown numNumero;
        private TextBox txtTexto;
        private ComboBox cmbTipo;
        private TextBox txtSeccion;
        private CheckBox chkMultiple;
        private Button btnOk;
        private Button btnCancel;

        public PreguntaForm(string seccionActual, int numeroSugerido)
        {
            Text = "Nueva Pregunta";
            Width = 360;
            Height = 250;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var lblNumero = new Label { Left = 15, Top = 20, Text = "Número" };
            numNumero = new NumericUpDown { Left = 120, Top = 18, Width = 200, Minimum = 1, Maximum = 10000, Value = numeroSugerido };

            var lblTexto = new Label { Left = 15, Top = 50, Text = "Pregunta" };
            txtTexto = new TextBox { Left = 120, Top = 48, Width = 200 };

            var lblTipo = new Label { Left = 15, Top = 80, Text = "Tipo" };
            cmbTipo = new ComboBox { Left = 120, Top = 78, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipo.DataSource = Enum.GetValues(typeof(TipoPregunta));

            var lblSeccion = new Label { Left = 15, Top = 110, Text = "Sección" };
            txtSeccion = new TextBox { Left = 120, Top = 108, Width = 200, Text = seccionActual };

            chkMultiple = new CheckBox { Left = 120, Top = 138, Text = "¿Múltiple?" };

            btnOk = new Button { Text = "OK", Left = 120, Width = 80, Top = 170, DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Cancelar", Left = 210, Width = 80, Top = 170, DialogResult = DialogResult.Cancel };

            Controls.AddRange(new Control[] { lblNumero, numNumero, lblTexto, txtTexto, lblTipo, cmbTipo, lblSeccion, txtSeccion, chkMultiple, btnOk, btnCancel });

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            btnOk.Click += (s, e) =>
            {
                Result = new Pregunta
                {
                    Numero = (int)numNumero.Value,
                    Texto = txtTexto.Text,
                    Tipo = (TipoPregunta)cmbTipo.SelectedItem!,
                    Seccion = txtSeccion.Text,
                    Multiple = chkMultiple.Checked
                };
                DialogResult = DialogResult.OK;
                Close();
            };
            UIStyleHelper.ApplyTheme(this);
        }
    }
}
