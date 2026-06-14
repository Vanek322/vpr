using System.Drawing;
using System.Windows.Forms;

namespace vpr
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
            SetupDesign();
        }

        private void SetupDesign()
        {
            ThemeManager.ApplyTheme(this);
            this.Padding = new Padding(0);
            this.MinimumSize = new Size(950, 600);
            this.ClientSize = new Size(950, 600);

            // Заголовок по центру
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = ThemeManager.Surface,
                Padding = new Padding(20)
            };
            this.Controls.Add(headerPanel);

            var lblTitle = new Label
            {
                Text = "Сводка результатов ВПР",
                Font = ThemeManager.FontHeading,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            headerPanel.Controls.Add(lblTitle);

            // Панель для карточек с центрированием
            var cardsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(40, 20, 40, 40),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            cardsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            cardsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            cardsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            this.Controls.Add(cardsPanel);

            // Карточки (уменьшенный размер)
            cardsPanel.Controls.Add(CreateMenuCard("👨‍🏫", "Учителя", "Управление списком преподавателей", ThemeManager.Primary, btnTeachers_Click), 0, 0);
            cardsPanel.Controls.Add(CreateMenuCard("🎓", "Обучающиеся", "Список учеников и их классы", ThemeManager.Success, btnStudents_Click), 1, 0);
            cardsPanel.Controls.Add(CreateMenuCard("📋", "Протоколы", "Результаты ВПР и баллы", Color.FromArgb(255, 139, 92, 246), btnProtocols_Click), 2, 0);

            // Кнопка выхода
            var exitPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Color.Transparent,
                Padding = new Padding(20)
            };
            this.Controls.Add(exitPanel);

            btnExit.Size = new Size(150, 45);
            ThemeManager.StyleButton(btnExit, ButtonStyle.Danger);
            btnExit.Text = "✕ Выход";
            exitPanel.Controls.Add(btnExit);
            btnExit.Anchor = AnchorStyles.None;
            btnExit.Location = new Point((exitPanel.Width - btnExit.Width) / 2, (exitPanel.Height - btnExit.Height) / 2);
        }

        private Panel CreateMenuCard(string icon, string title, string description, Color accentColor, EventHandler onClick)
        {
            var cardContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                BackColor = Color.Transparent
            };

            var card = new Panel
            {
                Size = new Size(220, 180),
                Location = new Point((cardContainer.Width - 220) / 2, (cardContainer.Height - 180) / 2),
                Anchor = AnchorStyles.None,
                BackColor = ThemeManager.Surface,
                Cursor = Cursors.Hand
            };

            // Центрируем карточку при изменении размера контейнера
            cardContainer.Resize += (s, e) =>
            {
                card.Location = new Point((cardContainer.Width - card.Width) / 2, (cardContainer.Height - card.Height) / 2);
            };

            var accentBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 6,
                BackColor = accentColor
            };
            card.Controls.Add(accentBar);

            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 32F),
                AutoSize = false,
                Size = new Size(220, 60),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top
            };
            card.Controls.Add(lblIcon);

            var lblTitle = new Label
            {
                Text = title,
                Font = ThemeManager.FontTitle,
                ForeColor = ThemeManager.TextPrimary,
                AutoSize = false,
                Size = new Size(220, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 5, 0, 0)
            };
            card.Controls.Add(lblTitle);

            var lblDesc = new Label
            {
                Text = description,
                Font = ThemeManager.FontSmall,
                ForeColor = ThemeManager.TextSecondary,
                AutoSize = false,
                Size = new Size(220, 60),
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Top,
                Padding = new Padding(10, 10, 10, 0)
            };
            card.Controls.Add(lblDesc);

            card.MouseEnter += (s, e) =>
            {
                card.BackColor = Color.FromArgb(245, 247, 250);
                accentBar.Height = 8;
            };
            card.MouseLeave += (s, e) =>
            {
                card.BackColor = ThemeManager.Surface;
                accentBar.Height = 6;
            };
            card.Click += onClick;
            lblIcon.Click += onClick;
            lblTitle.Click += onClick;
            lblDesc.Click += onClick;

            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(ThemeManager.Border, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            cardContainer.Controls.Add(card);
            return cardContainer;
        }

        private void btnTeachers_Click(object sender, EventArgs e)
        {
            var form = new FormListTeachers();
            form.ShowDialog();
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            var form = new FormListStudents();
            form.ShowDialog();
        }

        private void btnProtocols_Click(object sender, EventArgs e)
        {
            var form = new FormListProtocols();
            form.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}