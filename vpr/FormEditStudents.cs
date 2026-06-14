using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using vpr.Models;

namespace vpr
{
    public partial class FormEditStudent : Form
    {
        private int? studentId;
        private bool isNew;
        private TextBox txtFullName;
        private ComboBox cmbClass;
        private RoundedButton btnSave;
        private RoundedButton btnCancel;

        public FormEditStudent(int? id = null)
        {
            this.studentId = id;
            this.isNew = !id.HasValue;
            InitializeComponent();
            LoadClasses();
            LoadData();
        }

        private void InitializeComponent()
        {
            txtFullName = new TextBox();
            cmbClass = new ComboBox();
            btnSave = new RoundedButton();
            btnCancel = new RoundedButton();
            SuspendLayout();

            this.BackColor = ThemeManager.Background;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(500, 320);
            this.Text = isNew ? "Добавить ученика" : "Редактировать ученика";

            var lblTitle = new Label
            {
                Text = isNew ? "Новый ученик" : "Редактирование",
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

            txtFullName.Location = new Point(120, 67);
            txtFullName.Size = new Size(340, 30);
            txtFullName.Font = ThemeManager.FontBody;
            txtFullName.BorderStyle = BorderStyle.FixedSingle;

            var lblClass = new Label
            {
                Text = "Класс:",
                Font = ThemeManager.FontBody,
                ForeColor = ThemeManager.TextPrimary,
                Location = new Point(20, 115),
                AutoSize = true
            };

            cmbClass.Location = new Point(120, 112);
            cmbClass.Size = new Size(340, 30);
            cmbClass.Font = ThemeManager.FontBody;
            cmbClass.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClass.FlatStyle = FlatStyle.Flat;

            btnSave.Text = "Сохранить";
            btnSave.Size = new Size(150, 40);
            btnSave.BackColor = ThemeManager.Success;
            btnSave.ForeColor = Color.White;
            btnSave.BorderRadius = 10;
            btnSave.Location = new Point(160, 180);
            btnSave.Click += btnSave_Click;

            btnCancel.Text = "Отмена";
            btnCancel.Size = new Size(150, 40);
            btnCancel.BackColor = ThemeManager.Secondary;
            btnCancel.ForeColor = Color.White;
            btnCancel.BorderRadius = 10;
            btnCancel.Location = new Point(320, 180);
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lblTitle, lblName, txtFullName, lblClass, cmbClass, btnSave, btnCancel });
            ResumeLayout(false);
        }

        private void LoadClasses()
        {
            using (var db = new VprDbContext())
            {
                var classes = db.Classes
                    .Include(c => c.ClassLevel)
                    .OrderBy(c => c.ClassLevel.Number)
                    .ThenBy(c => c.SymbolOfClass)
                    .ToList();

                cmbClass.Items.Clear();
                foreach (var cls in classes)
                {
                    cmbClass.Items.Add(new ClassItem
                    {
                        Id = cls.Id,
                        Display = $"{cls.ClassLevel.Number}{cls.SymbolOfClass}"
                    });
                }
                cmbClass.DisplayMember = "Display";
                cmbClass.ValueMember = "Id";
            }
        }

        private void LoadData()
        {
            if (!isNew && studentId.HasValue)
            {
                using (var db = new VprDbContext())
                {
                    var student = db.Students.Find(studentId.Value);
                    if (student != null)
                    {
                        txtFullName.Text = student.FullName;
                        cmbClass.SelectedValue = student.IdClass;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var fullName = txtFullName.Text.Trim();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Введите ФИО ученика!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbClass.SelectedItem == null)
            {
                MessageBox.Show("Выберите класс!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var classId = ((ClassItem)cmbClass.SelectedItem).Id;

            try
            {
                using (var db = new VprDbContext())
                {
                    if (isNew)
                    {
                        db.Students.Add(new Student { FullName = fullName, IdClass = classId });
                    }
                    else
                    {
                        var student = db.Students.Find(studentId.Value);
                        if (student != null)
                        {
                            student.FullName = fullName;
                            student.IdClass = classId;
                        }
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

        private class ClassItem
        {
            public int Id { get; set; }
            public string Display { get; set; }
        }
    }
}