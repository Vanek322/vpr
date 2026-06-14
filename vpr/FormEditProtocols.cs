using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using vpr.Models;

namespace vpr
{
    public partial class FormEditProtocol : Form
    {
        private int? protocolId;
        private bool isNew;
        private ComboBox cmbStudent;
        private ComboBox cmbSubject;
        private ComboBox cmbTeacherAssignment;
        private NumericUpDown numVariant;
        private NumericUpDown numPreviousGrade;
        private NumericUpDown numTotalScore;
        private RoundedButton btnSave;
        private RoundedButton btnCancel;

        public FormEditProtocol(int? id = null)
        {
            this.protocolId = id;
            this.isNew = !id.HasValue;
            InitializeComponent();
            LoadComboBoxes();
            LoadData();
        }

        private void InitializeComponent()
        {
            cmbStudent = new ComboBox();
            cmbSubject = new ComboBox();
            cmbTeacherAssignment = new ComboBox();
            numVariant = new NumericUpDown();
            numPreviousGrade = new NumericUpDown();
            numTotalScore = new NumericUpDown();
            btnSave = new RoundedButton();
            btnCancel = new RoundedButton();
            SuspendLayout();

            this.BackColor = ThemeManager.Background;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(600, 500);
            this.Text = isNew ? "Добавить протокол" : "Редактировать протокол";

            var lblTitle = new Label
            {
                Text = isNew ? "Новый протокол" : "Редактирование протокола",
                Font = ThemeManager.FontTitle,
                ForeColor = ThemeManager.TextPrimary,
                Location = new Point(20, 20),
                AutoSize = true
            };

            int yPos = 70;
            int labelX = 20;
            int controlX = 200;
            int controlWidth = 360;

            // Ученик
            var lblStudent = new Label { Text = "Ученик:", Font = ThemeManager.FontBody, Location = new Point(labelX, yPos), AutoSize = true };
            cmbStudent.Location = new Point(controlX, yPos - 3);
            cmbStudent.Size = new Size(controlWidth, 30);
            cmbStudent.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStudent.Font = ThemeManager.FontBody;
            cmbStudent.DisplayMember = "Display";
            cmbStudent.ValueMember = "Id";
            Controls.Add(lblStudent);
            Controls.Add(cmbStudent);

            yPos += 45;

            // Предмет
            var lblSubject = new Label { Text = "Предмет:", Font = ThemeManager.FontBody, Location = new Point(labelX, yPos), AutoSize = true };
            cmbSubject.Location = new Point(controlX, yPos - 3);
            cmbSubject.Size = new Size(controlWidth, 30);
            cmbSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSubject.Font = ThemeManager.FontBody;
            cmbSubject.DisplayMember = "Name";
            cmbSubject.ValueMember = "Id";
            cmbSubject.SelectedIndexChanged += CmbSubject_SelectedIndexChanged;
            Controls.Add(lblSubject);
            Controls.Add(cmbSubject);

            yPos += 45;

            // Назначение учителя
            var lblTeacher = new Label { Text = "Учитель:", Font = ThemeManager.FontBody, Location = new Point(labelX, yPos), AutoSize = true };
            cmbTeacherAssignment.Location = new Point(controlX, yPos - 3);
            cmbTeacherAssignment.Size = new Size(controlWidth, 30);
            cmbTeacherAssignment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTeacherAssignment.Font = ThemeManager.FontBody;
            cmbTeacherAssignment.DisplayMember = "Display";
            cmbTeacherAssignment.ValueMember = "Id";
            Controls.Add(lblTeacher);
            Controls.Add(cmbTeacherAssignment);

            yPos += 45;

            // Вариант
            var lblVariant = new Label { Text = "Вариант:", Font = ThemeManager.FontBody, Location = new Point(labelX, yPos), AutoSize = true };
            numVariant.Location = new Point(controlX, yPos - 3);
            numVariant.Size = new Size(100, 30);
            numVariant.Minimum = 1;
            numVariant.Maximum = 99;
            numVariant.Value = 1;
            numVariant.Font = ThemeManager.FontBody;
            Controls.Add(lblVariant);
            Controls.Add(numVariant);

            yPos += 45;

            // Прошлогодняя оценка
            var lblGrade = new Label { Text = "Оценка (прош. год):", Font = ThemeManager.FontBody, Location = new Point(labelX, yPos), AutoSize = true };
            numPreviousGrade.Location = new Point(controlX, yPos - 3);
            numPreviousGrade.Size = new Size(100, 30);
            numPreviousGrade.Minimum = 0;
            numPreviousGrade.Maximum = 5;
            numPreviousGrade.Font = ThemeManager.FontBody;
            Controls.Add(lblGrade);
            Controls.Add(numPreviousGrade);

            yPos += 45;

            // Всего баллов
            var lblScore = new Label { Text = "Всего баллов:", Font = ThemeManager.FontBody, Location = new Point(labelX, yPos), AutoSize = true };
            numTotalScore.Location = new Point(controlX, yPos - 3);
            numTotalScore.Size = new Size(100, 30);
            numTotalScore.Minimum = 0;
            numTotalScore.Maximum = 999;
            numTotalScore.Font = ThemeManager.FontBody;
            Controls.Add(lblScore);
            Controls.Add(numTotalScore);

            yPos += 60;

            // Кнопки
            btnSave.Text = "Сохранить";
            btnSave.Size = new Size(150, 40);
            btnSave.BackColor = ThemeManager.Success;
            btnSave.ForeColor = Color.White;
            btnSave.BorderRadius = 10;
            btnSave.Location = new Point(200, yPos);
            btnSave.Click += btnSave_Click;
            Controls.Add(btnSave);

            btnCancel.Text = "Отмена";
            btnCancel.Size = new Size(150, 40);
            btnCancel.BackColor = ThemeManager.Secondary;
            btnCancel.ForeColor = Color.White;
            btnCancel.BorderRadius = 10;
            btnCancel.Location = new Point(360, yPos);
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancel);

            ResumeLayout(false);
        }

        private void LoadComboBoxes()
        {
            using (var db = new VprDbContext())
            {
                // Загружаем учеников
                var students = db.Students
                    .Include(s => s.Class).ThenInclude(c => c.ClassLevel)
                    .OrderBy(s => s.FullName)
                    .ToList();

                cmbStudent.Items.Clear();
                foreach (var student in students)
                {
                    string className = student.Class?.ClassLevel != null
                        ? $"{student.Class.ClassLevel.Number}{student.Class.SymbolOfClass}"
                        : "—";
                    cmbStudent.Items.Add(new StudentItem
                    {
                        Id = student.Id,
                        Display = $"{student.FullName} ({className})"
                    });
                }

                // Загружаем предметы
                var subjects = db.Subjects.OrderBy(s => s.Name).ToList();
                cmbSubject.DataSource = subjects;

                // Назначения учителей загрузятся при выборе предмета
            }
        }

        private void CmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubject.SelectedItem == null) return;

            var subjectId = ((Subject)cmbSubject.SelectedItem).Id;

            using (var db = new VprDbContext())
            {
                var assignments = db.TeacherAssignments
                    .Include(ta => ta.Teacher)
                    .Include(ta => ta.Class).ThenInclude(c => c.ClassLevel)
                    .Where(ta => ta.IdSubject == subjectId)
                    .OrderBy(ta => ta.Class.ClassLevel.Number)
                    .ThenBy(ta => ta.Class.SymbolOfClass)
                    .ToList();

                cmbTeacherAssignment.Items.Clear();
                foreach (var assignment in assignments)
                {
                    string className = $"{assignment.Class.ClassLevel.Number}{assignment.Class.SymbolOfClass}";
                    cmbTeacherAssignment.Items.Add(new TeacherAssignmentItem
                    {
                        Id = assignment.Id,
                        Display = $"{assignment.Teacher.FullName} - {className}"
                    });
                }
            }
        }

        private void LoadData()
        {
            if (!isNew && protocolId.HasValue)
            {
                using (var db = new VprDbContext())
                {
                    var protocol = db.Protocols.Find(protocolId.Value);
                    if (protocol != null)
                    {
                        // Устанавливаем предмет первым (это вызовет загрузку назначений)
                        cmbSubject.SelectedValue = protocol.IdTeacherAssignment; // Временно

                        var assignment = db.TeacherAssignments
                            .Include(ta => ta.Subject)
                            .FirstOrDefault(ta => ta.Id == protocol.IdTeacherAssignment);

                        if (assignment != null)
                        {
                            cmbSubject.SelectedValue = assignment.IdSubject;
                            cmbTeacherAssignment.SelectedValue = protocol.IdTeacherAssignment;
                        }

                        cmbStudent.SelectedValue = protocol.IdStudent;
                        numVariant.Value = protocol.Variant;
                        numPreviousGrade.Value = protocol.PreviousGrade ?? 0;
                        numTotalScore.Value = protocol.TotalScore;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedItem == null)
            {
                MessageBox.Show("Выберите ученика!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbTeacherAssignment.SelectedItem == null)
            {
                MessageBox.Show("Выберите назначение учителя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var studentId = ((StudentItem)cmbStudent.SelectedItem).Id;
            var teacherAssignmentId = ((TeacherAssignmentItem)cmbTeacherAssignment.SelectedItem).Id;
            var variant = (int)numVariant.Value;
            var previousGrade = numPreviousGrade.Value > 0 ? (int?)numPreviousGrade.Value : null;
            var totalScore = (int)numTotalScore.Value;

            try
            {
                using (var db = new VprDbContext())
                {
                    // Проверяем дубликат
                    var existing = db.Protocols.FirstOrDefault(p =>
                        p.IdStudent == studentId &&
                        p.IdTeacherAssignment == teacherAssignmentId &&
                        p.Variant == variant &&
                        (!isNew || p.Id != protocolId.Value));

                    if (existing != null)
                    {
                        MessageBox.Show("Такой протокол уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (isNew)
                    {
                        db.Protocols.Add(new Protocol
                        {
                            IdStudent = studentId,
                            IdTeacherAssignment = teacherAssignmentId,
                            Variant = variant,
                            PreviousGrade = previousGrade,
                            TotalScore = totalScore
                        });
                    }
                    else
                    {
                        var protocol = db.Protocols.Find(protocolId.Value);
                        if (protocol != null)
                        {
                            protocol.IdStudent = studentId;
                            protocol.IdTeacherAssignment = teacherAssignmentId;
                            protocol.Variant = variant;
                            protocol.PreviousGrade = previousGrade;
                            protocol.TotalScore = totalScore;
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

        private class StudentItem
        {
            public int Id { get; set; }
            public string Display { get; set; }
        }

        private class TeacherAssignmentItem
        {
            public int Id { get; set; }
            public string Display { get; set; }
        }
    }
}