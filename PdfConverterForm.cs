using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace Cerene_App
{
    public class PdfConverterForm : Form
    {
        private readonly List<Pregunta> listaPreguntas = new();
        private readonly Button btnCargarPDF = new();
        private readonly Button btnExportExcel = new();
        private readonly DataGridView dataGridView1 = new();

        public PdfConverterForm()
        {
            Text = "Convertir PDF";
            Width = 800;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            btnCargarPDF.Text = "Cargar PDF";
            btnCargarPDF.Top = 10;
            btnCargarPDF.Left = 10;
            btnCargarPDF.Click += btnCargarPDF_Click;

            btnExportExcel.Text = "Generar Excel";
            btnExportExcel.Top = 10;
            btnExportExcel.Left = btnCargarPDF.Right + 10;
            btnExportExcel.Visible = false;
            btnExportExcel.Click += btnExportExcel_Click;

            dataGridView1.Top = btnCargarPDF.Bottom + 10;
            dataGridView1.Left = 10;
            dataGridView1.Width = ClientSize.Width - 20;
            dataGridView1.Height = ClientSize.Height - dataGridView1.Top - 20;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            Controls.Add(btnCargarPDF);
            Controls.Add(btnExportExcel);
            Controls.Add(dataGridView1);

            UIStyleHelper.ApplyTheme(this);
        }

        private void btnCargarPDF_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Filter = "PDF files (*.pdf)|*.pdf";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string texto = LeerTextoDesdePDF(ofd.FileName);
                var preguntas = DetectarPreguntasConTabla(texto);
                listaPreguntas.Clear();
                listaPreguntas.AddRange(preguntas);
                MostrarEnTabla();
                btnExportExcel.Visible = listaPreguntas.Count > 0;
            }
        }

        private static string LeerTextoDesdePDF(string rutaPDF)
        {
            string resultado = string.Empty;

            using PdfReader reader = new(rutaPDF);
            using PdfDocument pdfDoc = new(reader);
            {
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    var textoPagina = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);
                    resultado += textoPagina + "\n";
                }
            }

            return resultado;
        }

        private static List<Pregunta> DetectarPreguntasConTabla(string texto)
        {
            var preguntas = new List<Pregunta>();
            var lineas = texto.Replace("_", string.Empty).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            Pregunta? preguntaActual = null;
            int contador = 0;

            foreach (var lineaRaw in lineas)
            {
                var linea = lineaRaw.Trim();

                if (Regex.IsMatch(linea, @"^\d+[\.\)]\s"))
                {
                    if (preguntaActual != null)
                        preguntas.Add(preguntaActual);

                    contador++;
                    preguntaActual = new Pregunta
                    {
                        Numero = contador,
                        Texto = linea,
                        Tipo = TipoPregunta.Desconocido
                    };
                }
                else if (Regex.IsMatch(linea, @"^[a-dA-D][\)\.]"))
                {
                    if (preguntaActual != null)
                    {
                        preguntaActual.Opciones.Add(new OpcionRespuesta { Texto = linea });
                        preguntaActual.Tipo = TipoPregunta.OpcionMultiple;
                        preguntaActual.Multiple = true;
                    }
                }
                else if (linea.StartsWith("Respuesta:", StringComparison.OrdinalIgnoreCase))
                {
                    if (preguntaActual != null)
                        preguntaActual.RespuestaCorrecta = new OpcionRespuesta { Texto = linea.Substring(9).Trim() };
                }
                else if (preguntaActual != null && !string.IsNullOrWhiteSpace(linea))
                {
                    if (preguntaActual.Tipo == TipoPregunta.Desconocido)
                        preguntaActual.Tipo = TipoPregunta.Abierta;

                    preguntaActual.Texto += " " + linea;
                }
            }

            if (preguntaActual != null)
                preguntas.Add(preguntaActual);

            return preguntas;
        }

        private void MostrarEnTabla()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("Numero", "#");
            dataGridView1.Columns.Add("Texto", "Pregunta");
            dataGridView1.Columns.Add("Tipo", "Tipo");
            dataGridView1.Columns.Add("Opciones", "Opciones");
            dataGridView1.Columns.Add("Respuesta", "Respuesta");

            foreach (var p in listaPreguntas)
            {
                dataGridView1.Rows.Add(p.Numero.ToString(), p.Texto, p.Tipo.ToString(), p.OpcionesResumen, p.RespuestaCorrecta?.Texto);
            }
        }

        private void btnExportExcel_Click(object? sender, EventArgs e)
        {
            using SaveFileDialog sfd = new();
            sfd.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                GenerarExcel(sfd.FileName);
                MessageBox.Show("Archivo Excel generado correctamente");
            }
        }

        private void GenerarExcel(string ruta)
        {
            var wb = new XLWorkbook();
            var wsOpciones = wb.AddWorksheet("Opciones");
            wsOpciones.Cell(1, 1).Value = "Id";
            wsOpciones.Cell(1, 2).Value = "Texto";

            var wsPreguntas = wb.AddWorksheet("Preguntas");
            wsPreguntas.Cell(1, 1).Value = "Texto";
            wsPreguntas.Cell(1, 2).Value = "Tipo";
            wsPreguntas.Cell(1, 3).Value = "Seccion";
            wsPreguntas.Cell(1, 4).Value = "Multiple";
            wsPreguntas.Cell(1, 5).Value = "Opciones";
            wsPreguntas.Cell(1, 6).Value = "Respuesta";

            var catalogo = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            int nextId = 1;
            foreach (var p in listaPreguntas)
            {
                foreach (var op in p.Opciones)
                {
                    var texto = op.Texto.Trim();
                    if (!catalogo.TryGetValue(texto, out int id))
                    {
                        id = nextId++;
                        catalogo[texto] = id;
                    }
                    op.Id = id;
                }

                if (p.RespuestaCorrecta != null)
                {
                    var respTexto = p.RespuestaCorrecta.Texto.Trim();
                    if (!catalogo.TryGetValue(respTexto, out int id))
                    {
                        id = nextId++;
                        catalogo[respTexto] = id;
                    }
                    p.RespuestaCorrecta.Id = id;
                }
            }

            int fila = 2;
            foreach (var kv in catalogo.OrderBy(k => k.Value))
            {
                wsOpciones.Cell(fila, 1).Value = kv.Value;
                wsOpciones.Cell(fila, 2).Value = kv.Key;
                fila++;
            }

            fila = 2;
            foreach (var p in listaPreguntas)
            {
                wsPreguntas.Cell(fila, 1).Value = p.Texto;
                wsPreguntas.Cell(fila, 2).Value = p.Tipo.ToString();
                wsPreguntas.Cell(fila, 3).Value = p.Seccion?.Nombre ?? string.Empty;
                wsPreguntas.Cell(fila, 4).Value = p.Multiple;
                wsPreguntas.Cell(fila, 5).Value = string.Join(",", p.Opciones.Select(o => o.Id));
                wsPreguntas.Cell(fila, 6).Value = p.RespuestaCorrecta?.Id;
                fila++;
            }

            wb.SaveAs(ruta);
        }
    }
}

