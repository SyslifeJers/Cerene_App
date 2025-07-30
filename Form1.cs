using ClosedXML.Excel;
using System.Windows.Forms;

namespace Cerene_App
{
    public partial class Form1 : Form
    {
        private List<Pregunta> List_preguntas;
        public Form1()
        {
            InitializeComponent();
            List_preguntas = new();
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

                MostrarEnTabla(preguntas); // este método lo puedes crear abajo
                var secciones = preguntas.Select(p => p.Seccion).Distinct().ToList();
                List_preguntas = new(preguntas);
                cmbSecciones.DataSource = secciones;
            }

         

        }
        private void comboBoxSecciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seccionSeleccionada = cmbSecciones.SelectedItem.ToString();
            var filtradas = List_preguntas.Where(p => p.Seccion == seccionSeleccionada).ToList();
            MostrarEnTabla(filtradas);
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

    }
}
