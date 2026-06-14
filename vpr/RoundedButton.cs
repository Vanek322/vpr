using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace vpr
{
    public class RoundedButton : Button
    {
        private int borderRadius = 12;
        private int borderWidth = 0;
        private Color borderColor = Color.Transparent;
        private Color _baseColor;

        [DefaultValue(12)]
        [Category("Appearance")]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = value; UpdateRegion(); Invalidate(); }
        }

        [DefaultValue(0)]
        [Category("Appearance")]
        public int BorderWidth
        {
            get => borderWidth;
            set { borderWidth = value; Invalidate(); }
        }

        [DefaultValue(typeof(Color), "Transparent")]
        [Category("Appearance")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; Invalidate(); }
        }

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
            Font = ThemeManager.FontButton;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _baseColor = BackColor;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            if (borderRadius > 0 && Width > 0 && Height > 0)
            {
                using (var path = GetRoundedRect(new Rectangle(0, 0, Width, Height), borderRadius))
                {
                    Region = new Region(path);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var path = GetRoundedRect(rect, borderRadius))
            {
                // Фон
                using (var brush = new SolidBrush(BackColor))
                {
                    g.FillPath(brush, path);
                }

                // Граница
                if (borderWidth > 0)
                {
                    using (var pen = new Pen(borderColor, borderWidth))
                    {
                        g.DrawPath(pen, path);
                    }
                }

                // Текст
                using (var textBrush = new SolidBrush(ForeColor))
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString(Text, Font, textBrush, ClientRectangle, sf);
                }
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;

            if (rect.Width < d) d = rect.Width;
            if (rect.Height < d) d = rect.Height;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }

        // Убрали изменение цвета при наведении - кнопка теперь не меняет цвет
    }
}