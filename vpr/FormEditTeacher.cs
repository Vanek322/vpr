using System.Drawing;
using System.Windows.Forms;
using vpr.Models;

namespace vpr
{
    public partial class FormEditTeacher : Form
    {
        private int? teacherId;
        private bool isNew;
        private TextBox txtFullName;
        private RoundedButton btnSave;
        private RoundedButton btnCancel;

        public FormEditTeacher(int? id = null)
        {
            this.teacherId = id;
            this.isNew = !id.HasValue;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            txtFullName = new TextBox();
            btnSave = new RoundedButton();
            btnCancel = new RoundedButton();
            SuspendLayout();

            this.BackColor = ThemeManager.Background;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(500, 250);
            this.Text = isNew ? "Добавить учителя" : "Редактировать учителя";

            var lblTitle = new Label
            {
                Text = isNew ? "Новый учитель" : "Редактирование",
                Font = ThemeManager.FontTitle,
                ForeColor = ThemeManager.TextPrimary,
                Location = new Point(20, 20),
                AutoSize = true
            };

            var lblName = new Label
            {
                Text = "ФИО:",
                Font = ThemeManager.FontBody,
                ForeColor = ThemeManager.TextPrimary,
                Location = new Point(20, 70),
                AutoSize = true
            };

            txtFullName.Location = new Point(100, 67);
            txtFullName.Size = new Size(360, 30);
            txtFullName.Font = ThemeManager.FontBody;
            txtFullName.BorderStyle = BorderStyle.FixedSingle;

            btnSave.Text = "Сохранить";
            btnSave.Size = new Size(150, 40);
            btnSave.BackColor = ThemeManager.Success;
            btnSave.ForeColor = Color.White;
            btnSave.BorderRadius = 10;
            btnSave.Location = new Point(160, 130);
            btnSave.Click += btnSave_Click;

            btnCancel.Text = "Отмена";
            btnCancel.Size = new Size(150, 40);
            btnCancel.BackColor = ThemeManager.Secondary;
            btnCancel.ForeColor = Color.White;
            btnCancel.BorderRadius = 10;
            btnCancel.Location = new Point(320, 130);
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lblTitle, lblName, txtFullName, btnSave, btnCancel });
            ResumeLayout(false);
        }

        private void LoadData()
        {
            if (!isNew && teacherId.HasValue)
            {
                using (var db = new VprDbContext())
                {
                    var teacher = db.Teachers.Find(teacherId.Value);
                    if (teacher != null)
                        txtFullName.Text = teacher.FullName;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var fullName = txtFullName.Text.Trim();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Введите ФИО учителя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new VprDbContext())
                {
                    if (isNew)
                    {
                        db.Teachers.Add(new Teacher { FullName = fullName });
                    }
                    else
                    {
                        var teacher = db.Teachers.Find(teacherId.Value);
                        if (teacher != null)
                            teacher.FullName = fullName;
                    }
                    db.SaveChanges();
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}