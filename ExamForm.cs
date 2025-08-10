using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cerene_App
{
    public class ExamForm : Form
    {
        public int? IdExamen { get; private set; }
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string ApiLink { get; set; }

        private ComboBox cmbAreas = new();
        private ComboBox cmbUsuario = new();
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
            cmbUsuario.Left = 100;
            cmbUsuario.Top = 40;
            cmbUsuario.Width = 200;

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
            Controls.Add(cmbUsuario);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(btnGuardar);

            Load += async (s, e) => await CargarAreasAsync();
            Load += async (s, e) => await CargarUsuariosAsync();
            UIStyleHelper.ApplyTheme(this);
        }

        //necesito cargar los usuarios en el combobox cmbUsuario
        private async Task CargarUsuariosAsync()
        {
            try
            {
                using var http = new HttpClient();
                ApiLink = ApiConfig.GetUsuarios;
                string json = await http.GetStringAsync(ApiLink);
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                cmbUsuario.DataSource = usuarios;
                cmbUsuario.DisplayMember = "name";
                cmbUsuario.ValueMember = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}");
            }
        }

        private async Task CargarAreasAsync()
        {
            try
            {
                using var http = new HttpClient();
                ApiLink = ApiConfig.GetAreas;
                string json = await http.GetStringAsync(ApiLink);
                var areas = JsonConvert.DeserializeObject<List<Area>>(json);
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
            if (cmbAreas.SelectedValue == null || cmbUsuario.SelectedValue == null || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Complete todos los campos");
                return;
            }

            var datos = new
            {
                id_area = Convert.ToInt32(cmbAreas.SelectedValue),
                id_usuario = Convert.ToInt32(cmbUsuario.SelectedValue),
                nombre_examen = txtNombre.Text
            };

            try
            {
                using var http = new HttpClient();
                string json = JsonConvert.SerializeObject(datos);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                ApiLink = ApiConfig.InsertExamen;
                var resp = await http.PostAsync(ApiLink, content);
                string respJson = await resp.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ExamenResponse>(respJson);
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
            public string descripcion { get; set; } = string.Empty;
        }

        private class Usuario
        {
            public int id { get; set; }

            public string name { get; set; } = string.Empty;   
            

        }

        private class ExamenResponse
        {
            public bool success { get; set; }
            public int id_examen { get; set; }
            public string? error { get; set; }
        }
    }
}
