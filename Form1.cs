using ClosedXML.Excel;
using System.Windows.Forms;
using System.Drawing;

namespace Cerene_App
{
    public partial class Form1 : Form
    {
        private List<Pregunta> List_preguntas;
        private List<OpcionRespuesta> CatalogoOpciones = new();
        public Form1()
        {
            InitializeComponent();
            List_preguntas = new();
            dataTable1.AllowUserToAddRows = true;
            dataTable1.AllowUserToDeleteRows = true;
            dataOpciones.AllowUserToAddRows = true;
            dataOpciones.AllowUserToDeleteRows = true;
            dataTable1.SelectionChanged += dataTable1_SelectionChanged;
            dataTable1.UserAddedRow += dataTable1_UserAddedRow;
            dataTable1.UserDeletingRow += dataTable1_UserDeletingRow;
            dataTable1.CellValueChanged += dataTable1_CellValueChanged;

            dataOpciones.UserAddedRow += dataOpciones_UserAddedRow;
            dataOpciones.UserDeletingRow += dataOpciones_UserDeletingRow;
            dataOpciones.CellValueChanged += dataOpciones_CellValueChanged;
        }

        private void btnTraer_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                List<Pregunta> preguntas;
                List<OpcionRespuesta> catalogo;

                Importar(ofd.FileName, out preguntas, out catalogo);

                MostrarEnTabla(preguntas);
                var secciones = preguntas.Select(p => p.Seccion).Distinct().ToList();
                List_preguntas = new(preguntas);
                CatalogoOpciones = new(catalogo);
                cmbSecciones.DataSource = secciones;
                if (dataTable1.Rows.Count > 0)
                {
                    dataTable1.Rows[0].Selected = true;
                    MostrarOpciones(List_preguntas[0].Opciones);
                }
            }

         

        }
        private void comboBoxSecciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seccionSeleccionada = cmbSecciones.SelectedItem.ToString();
            var filtradas = List_preguntas.Where(p => p.Seccion == seccionSeleccionada).ToList();
            MostrarEnTabla(filtradas);
            if (filtradas.Count > 0)
            {
                MostrarOpciones(filtradas[0].Opciones);
            }
        }
        private void MostrarEnTabla(List<Pregunta> preguntas)
        {
            dataTable1.Rows.Clear();
            dataTable1.Columns.Clear();

            dataTable1.Columns.Add("Numero", "#");
            dataTable1.Columns.Add("Texto", "Pregunta");
            dataTable1.Columns.Add("Tipo", "Tipo");
            dataTable1.Columns.Add("Seccion", "Sección");
            dataTable1.Columns.Add("Multiple", "¿Múltiple?");
            dataTable1.Columns.Add("Opciones", "Opciones");
            dataTable1.Columns.Add("Respuesta", "Respuesta Correcta");

            foreach (var p in preguntas)
            {
                dataTable1.Rows.Add(
                    p.Numero.ToString(),
                    p.Texto,
                    p.Tipo.ToString(),
                    p.Seccion,
                    p.Multiple ? "Sí" : "No",
                    p.OpcionesResumen,
                    p.RespuestaCorrecta?.Texto ?? ""
                );
            }

            if (preguntas.Count > 0)
            {
                dataTable1.Rows[0].Selected = true;
                MostrarOpciones(preguntas[0].Opciones);
            }
        }

        public static void Importar(string rutaArchivo, out List<Pregunta> preguntas, out List<OpcionRespuesta> catalogoOpciones)
        {
            preguntas = new();
            catalogoOpciones = new();

            using var libro = new XLWorkbook(rutaArchivo);

            var hojaOpciones = libro.Worksheet("Opciones");
            foreach (var fila in hojaOpciones.RowsUsed().Skip(1)) // saltar encabezado
            {
                int id = int.Parse(fila.Cell(1).GetString());
                string texto = fila.Cell(2).GetString();
                catalogoOpciones.Add(new OpcionRespuesta { Id = id, Texto = texto });
            }

            var hojaPreguntas = libro.Worksheet("Preguntas");
            foreach (var fila in hojaPreguntas.RowsUsed().Skip(1))
            {
                var pregunta = new Pregunta
                {
                    Numero = int.Parse(fila.Cell(1).GetString()),
                    Texto = fila.Cell(2).GetString(),
                    Tipo = Enum.TryParse(fila.Cell(3).GetString(), out TipoPregunta tipo) ? tipo : TipoPregunta.Desconocido,
                    Seccion = fila.Cell(4).GetString(),
                    Multiple = bool.TryParse(fila.Cell(5).GetString(), out bool mult) && mult
                };

                string opcionesIds = fila.Cell(6).GetString();
                if (!string.IsNullOrWhiteSpace(opcionesIds))
                {
                    var ids = opcionesIds.Split('|').Select(id => int.Parse(id.Trim()));
                    pregunta.Opciones = catalogoOpciones.Where(o => ids.Contains(o.Id)).ToList();
                }

                if (int.TryParse(fila.Cell(7).GetString(), out int idResp))
                {
                    pregunta.RespuestaCorrecta = catalogoOpciones.FirstOrDefault(o => o.Id == idResp);
                }

                preguntas.Add(pregunta);
            }
        }
        private void MostrarPorSecciones(List<Pregunta> preguntas)
        {
            dataTable1.Rows.Clear();
            dataTable1.Columns.Clear();

            dataTable1.Columns.Add("Numero", "#");
            dataTable1.Columns.Add("Texto", "Pregunta");
            dataTable1.Columns.Add("Tipo", "Tipo");
            dataTable1.Columns.Add("Multiple", "¿Múltiple?");
            dataTable1.Columns.Add("Opciones", "Opciones");
            dataTable1.Columns.Add("Respuesta", "Respuesta");

            var agrupado = preguntas.GroupBy(p => p.Seccion);

            foreach (var grupo in agrupado)
            {
                // Insertar una fila como encabezado de sección
                int index = dataTable1.Rows.Add();
                var fila = dataTable1.Rows[index];
                fila.DefaultCellStyle.BackColor = Color.LightGray;
                fila.Cells[0].Value = $"▶ {grupo.Key}";
                fila.Cells[0].Style.Font = new Font(dataTable1.Font, FontStyle.Bold);

                foreach (var p in grupo)
                {
                    dataTable1.Rows.Add(
                        p.Numero.ToString(),
                        p.Texto,
                        p.Tipo.ToString(),
                        p.Multiple ? "Sí" : "No",
                        p.OpcionesResumen,
                        p.RespuestaCorrecta?.Texto ?? ""
                    );
                }
            }
        }

        private void dataTable1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataTable1.SelectedRows.Count > 0)
            {
                int idx = dataTable1.SelectedRows[0].Index;
                if (idx >= 0 && idx < List_preguntas.Count)
                {
                    MostrarOpciones(List_preguntas[idx].Opciones);
                }
            }
        }

        private void MostrarOpciones(List<OpcionRespuesta> opciones)
        {
            dataOpciones.Rows.Clear();
            dataOpciones.Columns.Clear();
            dataOpciones.Columns.Add("Id", "Id");
            dataOpciones.Columns.Add("Texto", "Opción");

            foreach (var o in opciones)
            {
                dataOpciones.Rows.Add(o.Id, o.Texto);
            }
        }

        private void dataTable1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (e.Row.Index >= List_preguntas.Count)
            {
                List_preguntas.Add(new Pregunta());
            }
        }

        private void dataTable1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            int idx = e.Row.Index;
            if (idx >= 0 && idx < List_preguntas.Count)
            {
                List_preguntas.RemoveAt(idx);
            }
        }

        private void dataTable1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= List_preguntas.Count)
                return;

            var p = List_preguntas[e.RowIndex];
            switch (e.ColumnIndex)
            {
                case 0:
                    int.TryParse(dataTable1.Rows[e.RowIndex].Cells[0].Value?.ToString(), out int num);
                    p.Numero = num;
                    break;
                case 1:
                    p.Texto = dataTable1.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? string.Empty;
                    break;
                case 2:
                    if (Enum.TryParse(dataTable1.Rows[e.RowIndex].Cells[2].Value?.ToString(), out TipoPregunta tipo))
                        p.Tipo = tipo;
                    break;
                case 3:
                    p.Seccion = dataTable1.Rows[e.RowIndex].Cells[3].Value?.ToString() ?? string.Empty;
                    break;
                case 4:
                    bool.TryParse(dataTable1.Rows[e.RowIndex].Cells[4].Value?.ToString(), out bool mult);
                    p.Multiple = mult;
                    break;
            }
        }

        private void dataOpciones_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (dataTable1.SelectedRows.Count == 0) return;
            int idx = dataTable1.SelectedRows[0].Index;
            if (idx < 0 || idx >= List_preguntas.Count) return;
            List_preguntas[idx].Opciones.Add(new OpcionRespuesta());
        }

        private void dataOpciones_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (dataTable1.SelectedRows.Count == 0) return;
            int idx = dataTable1.SelectedRows[0].Index;
            if (idx < 0 || idx >= List_preguntas.Count) return;
            int optIndex = e.Row.Index;
            if (optIndex >= 0 && optIndex < List_preguntas[idx].Opciones.Count)
                List_preguntas[idx].Opciones.RemoveAt(optIndex);
        }

        private void dataOpciones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataTable1.SelectedRows.Count == 0) return;
            int idx = dataTable1.SelectedRows[0].Index;
            if (idx < 0 || idx >= List_preguntas.Count) return;
            if (e.RowIndex < 0 || e.RowIndex >= List_preguntas[idx].Opciones.Count) return;
            var opt = List_preguntas[idx].Opciones[e.RowIndex];
            switch (e.ColumnIndex)
            {
                case 0:
                    int.TryParse(dataOpciones.Rows[e.RowIndex].Cells[0].Value?.ToString(), out int id);
                    opt.Id = id;
                    break;
                case 1:
                    opt.Texto = dataOpciones.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? string.Empty;
                    break;
            }
        }

    }
}
