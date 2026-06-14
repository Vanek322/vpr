using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace vpr
{
    /// <summary>
    /// Менеджер тем для единого стиля приложения
    /// </summary>
    public static class ThemeManager
    {
        // Цветовая палитра
        public static readonly Color Primary = Color.FromArgb(255, 37, 99, 235);      // #2563EB
        public static readonly Color PrimaryHover = Color.FromArgb(255, 29, 78, 216); // #1D4ED8
        public static readonly Color PrimaryLight = Color.FromArgb(255, 219, 234, 254); // #DBEAFE

        public static readonly Color Secondary = Color.FromArgb(255, 100, 116, 139);  // #64748B
        public static readonly Color SecondaryHover = Color.FromArgb(255, 71, 85, 105); // #475569

        public static readonly Color Success = Color.FromArgb(255, 16, 185, 129);     // #10B981
        public static readonly Color SuccessHover = Color.FromArgb(255, 5, 150, 105); // #059669

        public static readonly Color Danger = Color.FromArgb(255, 239, 68, 68);       // #EF4444
        public static readonly Color DangerHover = Color.FromArgb(255, 220, 38, 38);  // #DC2626

        public static readonly Color Background = Color.FromArgb(255, 248, 250, 252); // #F8FAFC
        public static readonly Color Surface = Color.White;
        public static readonly Color TextPrimary = Color.FromArgb(255, 30, 41, 59);   // #1E293B
        public static readonly Color TextSecondary = Color.FromArgb(255, 100, 116, 139);
        public static readonly Color Border = Color.FromArgb(255, 226, 232, 240);     // #E2E8F0

        // Шрифты
        public static readonly Font FontHeading = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
        public static readonly Font FontTitle = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
        public static readonly Font FontBody = new Font("Segoe UI", 11F, FontStyle.Regular);
        public static readonly Font FontButton = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        public static readonly Font FontSmall = new Font("Segoe UI", 9F, FontStyle.Regular);

        /// <summary>
        /// Применяет тему к форме
        /// </summary>
        public static void ApplyTheme(Form form)
        {
            form.BackColor = Background;
            form.ForeColor = TextPrimary;
            form.Font = FontBody;
            form.Padding = new Padding(20);
        }

        /// <summary>
        /// Настраивает DataGridView в современном стиле
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Surface;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Border;

            // ВАЖНО: Показываем заголовки
            dgv.ColumnHeadersVisible = true;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = FontButton;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.ColumnHeadersHeight = 50;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.DefaultCellStyle.BackColor = Surface;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.Font = FontBody;
            dgv.DefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            dgv.DefaultCellStyle.SelectionBackColor = PrimaryLight;
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;

            dgv.RowTemplate.Height = 45;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = true;
            dgv.RowHeadersVisible = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 250, 252);
        }

        /// <summary>
        /// Настраивает кнопку в современном стиле
        /// </summary>
        public static void StyleButton(Button btn, ButtonStyle style = ButtonStyle.Primary)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Font = FontButton;
            btn.Padding = new Padding(20, 10, 20, 10);
            btn.AutoSize = true;
            btn.MinimumSize = new Size(120, 45);

            switch (style)
            {
                case ButtonStyle.Primary:
                    btn.BackColor = Primary;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = PrimaryHover;
                    break;
                case ButtonStyle.Secondary:
                    btn.BackColor = Secondary;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = SecondaryHover;
                    break;
                case ButtonStyle.Success:
                    btn.BackColor = Success;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = SuccessHover;
                    break;
                case ButtonStyle.Danger:
                    btn.BackColor = Danger;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = DangerHover;
                    break;
                case ButtonStyle.Outline:
                    btn.BackColor = Surface;
                    btn.ForeColor = Primary;
                    btn.FlatAppearance.BorderSize = 2;
                    btn.FlatAppearance.BorderColor = Primary;
                    btn.FlatAppearance.MouseOverBackColor = PrimaryLight;
                    break;
            }
        }

        /// <summary>
        /// Создает панель-карточку с тенью
        /// </summary>
        public static Panel CreateCard()
        {
            var card = new Panel
            {
                BackColor = Surface,
                Padding = new Padding(20),
                Margin = new Padding(10)
            };
            card.Paint += (s, e) => DrawShadow(e.Graphics, card.ClientRectangle);
            return card;
        }

        private static void DrawShadow(Graphics g, Rectangle rect)
        {
            // Рисуем тонкую границу вместо тени (для производительности)
            using (var pen = new Pen(Border, 1))
            {
                g.DrawRectangle(pen, 0, 0, rect.Width - 1, rect.Height - 1);
            }
        }
    }

    public enum ButtonStyle
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Outline
    }
}