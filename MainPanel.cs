using System;
using System.Windows.Forms;

namespace Cerene_App
{
    public class MainPanel : Form
    {
        public MainPanel()
        {
            Text = "Panel Inicial";
            Width = 300;
            Height = 200;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var btnConvertPdf = new Button
            {
                Text = "Convertir PDF",
                Width = 200,
                Height = 40,
                Top = 10,
                Left = 40
            };

            var btnUpload = new Button
            {
                Text = "Subir Evaluación",
                Width = 200,
                Height = 40,
                Top = 60,
                Left = 40
            };

            var btnDownload = new Button
            {
                Text = "Descargar Formato",
                Width = 200,
                Height = 40,
                Top = 110,
                Left = 40
            };

            btnConvertPdf.Click += (s, e) =>
            {
                using var form = new PdfConverterForm();
                form.ShowDialog();
            };
            btnUpload.Click += (s, e) =>
            {
                using var form = new Form1();
                form.ShowDialog();
            };
            btnDownload.Click += (s, e) => MessageBox.Show("Funcionalidad en desarrollo");

            Controls.Add(btnConvertPdf);
            Controls.Add(btnUpload);
            Controls.Add(btnDownload);

            UIStyleHelper.ApplyTheme(this);
        }
    }
}
