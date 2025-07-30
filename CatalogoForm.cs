using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cerene_App
{
    public class CatalogoForm : Form
    {
        public CatalogoForm(List<OpcionRespuesta> catalogo, Action<OpcionRespuesta> onSelect)
        {
            Text = "Catálogo de Opciones";
            Width = 400;
            Height = 300;

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            grid.Columns.Add("Id", "Id");
            grid.Columns.Add("Texto", "Texto");

            foreach (var o in catalogo)
            {
                grid.Rows.Add(o.Id, o.Texto);
            }

            grid.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < catalogo.Count)
                {
                    onSelect?.Invoke(catalogo[e.RowIndex]);
                    Close();
                }
            };

            Controls.Add(grid);
        }
    }
}
