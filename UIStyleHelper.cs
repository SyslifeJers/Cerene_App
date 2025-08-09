using System.Drawing;
using System.Windows.Forms;

namespace Cerene_App
{
    public static class UIStyleHelper
    {
        private static readonly Font DefaultFont = new("Segoe UI", 10F);
        private static readonly Color Background = Color.WhiteSmoke;
        private static readonly Color Primary = Color.SteelBlue;
        private static readonly Color PrimaryText = Color.White;
        private static readonly Color SecondaryText = Color.DimGray;

        public static void ApplyTheme(Form form)
        {
            form.Font = DefaultFont;
            form.BackColor = Background;
            ApplyThemeToControls(form.Controls);
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                control.Font = DefaultFont;

                switch (control)
                {
                    case Button btn:
                        btn.BackColor = Primary;
                        btn.ForeColor = PrimaryText;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 0;
                        break;
                    case Label lbl:
                        lbl.ForeColor = SecondaryText;
                        break;
                    case TextBox txt:
                        txt.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case DataGridView dgv:
                        StyleGrid(dgv);
                        break;
                }

                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls);
                }
            }
        }

        private static void StyleGrid(DataGridView dgv)
        {
            dgv.BackgroundColor = Background;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = PrimaryText;
            dgv.RowHeadersVisible = false;
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
        }
    }
}
