using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cerene_App
{
    public class ExamForm : Form
    {
        public int? IdExamen { get; private set; }
        public string ApiLink { get; set; } = ApiConfig.InsertExamen;

        private ComboBox cmbAreas = new();
        private TextBox txtUsuario = new();
        private TextBox txtNombre = new();
        private Button btnGuardar = new();

        public ExamForm()
        {
            Text = "Datos del Examen";
            Width = 350;
            Height = 200;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var lblArea = new Label { Text = "Área", Left = 10, Top = 15, Width = 80 };
            cmbAreas.Left = 100;
            cmbAreas.Top = 10;
            cmbAreas.Width = 200;

            var lblUsuario = new Label { Text = "Usuario", Left = 10, Top = 45, Width = 80 };
            txtUsuario.Left = 100;
            txtUsuario.Top = 40;
            txtUsuario.Width = 200;

            var lblNombre = new Label { Text = "Nombre", Left = 10, Top = 75, Width = 80 };
            txtNombre.Left = 100;
            txtNombre.Top = 70;
            txtNombre.Width = 200;

            btnGuardar.Text = "Guardar";
            btnGuardar.Left = 220;
            btnGuardar.Top = 110;
            btnGuardar.Click += async (s, e) => await GuardarAsync();

            Controls.Add(lblArea);
            Controls.Add(cmbAreas);
            Controls.Add(lblUsuario);
            Controls.Add(txtUsuario);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(btnGuardar);

            Load += async (s, e) => await CargarAreasAsync();
        }

        private async Task CargarAreasAsync()
        {
            try
            {
                using var http = new HttpClient();
                string json = await http.GetStringAsync("https://terapia.clinicacerene.com/areas/get_areas.php");
                var areas = JsonSerializer.Deserialize<List<Area>>(json);
                cmbAreas.DataSource = areas;
                cmbAreas.DisplayMember = "nombre_area";
                cmbAreas.ValueMember = "id_area";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar áreas: {ex.Message}");
            }
        }

        private async Task GuardarAsync()
        {
            if (cmbAreas.SelectedValue == null || string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Complete todos los campos");
                return;
            }

            var datos = new
            {
                id_area = Convert.ToInt32(cmbAreas.SelectedValue),
                id_usuario = int.TryParse(txtUsuario.Text, out int uid) ? uid : 0,
                nombre_examen = txtNombre.Text
            };

            try
            {
                using var http = new HttpClient();
                string json = JsonSerializer.Serialize(datos);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await http.PostAsync(ApiLink, content);
                string respJson = await resp.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ExamenResponse>(respJson);
                if (result != null && result.success)
                {
                    IdExamen = result.id_examen;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show($"Error al guardar examen: {result?.error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar examen: {ex.Message}");
            }
        }

        private class Area
        {
            public int id_area { get; set; }
            public string nombre_area { get; set; } = string.Empty;
        }

        private class ExamenResponse
        {
            public bool success { get; set; }
            public int id_examen { get; set; }
            public string? error { get; set; }
        }
    }
}
