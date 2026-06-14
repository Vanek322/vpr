using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using vpr.Models;

namespace vpr
{
    public partial class FormListStudents : Form
    {
        private RoundedButton btnImportBtn;
        private RoundedButton btnExportBtn;
        private RoundedButton btnExitBtn;

        public FormListStudents()
        {
            InitializeComponent();
            SetupDesign();
            LoadStudents();
        }

        private void SetupDesign()
        {
            ThemeManager.ApplyTheme(this);
            this.Padding = new Padding(20);
            this.MinimumSize = new Size(900, 600);

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = ThemeManager.Surface,
                Padding = new Padding(20, 15, 20, 15)
            };
            this.Controls.Add(headerPanel);

            var lblTitle = new Label
            {
                Text = "🎓 Список обучающихся",
                Font = ThemeManager.FontHeading,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            headerPanel.Controls.Add(lblTitle);

            var toolPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 10, 20, 10)
            };
            this.Controls.Add(toolPanel);

            btnImportBtn = new RoundedButton
            {
                Text = "⬆ Импорт",
                Size = new Size(130, 40),
                BackColor = ThemeManager.Success,
                ForeColor = Color.White,
                BorderRadius = 12,
                Dock = DockStyle.Right,
                Margin = new Padding(5, 0, 0, 0)
            };
            btnImportBtn.Click += btnImport_Click;
            toolPanel.Controls.Add(btnImportBtn);

            btnExportBtn = new RoundedButton
            {
                Text = "⬇ Экспорт",
                Size = new Size(130, 40),
                BackColor = ThemeManager.Primary,
                ForeColor = Color.White,
                BorderRadius = 12,
                Dock = DockStyle.Right,
                Margin = new Padding(5, 0, 0, 0)
            };
            btnExportBtn.Click += btnExport_Click;
            toolPanel.Controls.Add(btnExportBtn);

            btnExitBtn = new RoundedButton
            {
                Text = " Выход",
                Size = new Size(130, 40),
                BackColor = ThemeManager.Danger,
                ForeColor = Color.White,
                BorderRadius = 12,
                Dock = DockStyle.Right,
                Margin = new Padding(5, 0, 0, 0)
            };
            btnExitBtn.Click += (s, e) => this.Close();
            toolPanel.Controls.Add(btnExitBtn);

            var tableCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.Surface,
                Padding = new Padding(15)
            };
            this.Controls.Add(tableCard);

            tableCard.Paint += (s, e) =>
            {
                using (var pen = new Pen(ThemeManager.Border, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, tableCard.Width - 1, tableCard.Height - 1);
            };

            dgvStudents.Parent = tableCard;
            dgvStudents.Dock = DockStyle.Fill;
            dgvStudents.BringToFront();
            ThemeManager.StyleDataGridView(dgvStudents);
        }

        private void LoadStudents()
        {
            dgvStudents.Rows.Clear();

            if (dgvStudents.Columns.Count == 0)
            {
                var colId = new DataGridViewTextBoxColumn { Name = "№", HeaderText = "№", FillWeight = 10, ReadOnly = true };
                var colFullName = new DataGridViewTextBoxColumn { Name = "ФИО", HeaderText = "ФИО", FillWeight = 65, ReadOnly = true };
                var colClass = new DataGridViewTextBoxColumn { Name = "Класс", HeaderText = "Класс", FillWeight = 25, ReadOnly = true };

                dgvStudents.Columns.AddRange(colId, colFullName, colClass);
            }

            try
            {
                using (var db = new VprDbContext())
                {
                    var students = db.Students
                        .Include(s => s.Class)
                        .ThenInclude(c => c.ClassLevel)
                        .OrderBy(s => s.Id)
                        .ToList();

                    foreach (var student in students)
                    {
                        int rowIndex = dgvStudents.Rows.Add();
                        var row = dgvStudents.Rows[rowIndex];

                        row.Cells["№"].Value = student.Id;
                        row.Cells["ФИО"].Value = student.FullName;
                        row.Cells["Класс"].Value = $"{student.Class.ClassLevel.Number}{student.Class.SymbolOfClass}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "csv files(*.csv)|*.csv";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var lines = File.ReadAllLines(openFileDialog.FileName, Encoding.UTF8);

                        if (lines.Length > 0 && lines[0].Contains(""))
                        {
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            lines = File.ReadAllLines(openFileDialog.FileName, Encoding.GetEncoding(1251));
                        }

                        using (var db = new VprDbContext())
                        {
                            var classes = db.Classes.Include(c => c.ClassLevel).ToList();
                            int importedCount = 0, skippedCount = 0;

                            for (int i = 0; i < lines.Length; i++)
                            {
                                var line = lines[i];
                                if (string.IsNullOrWhiteSpace(line)) continue;

                                var parts = line.Split(';');
                                if (parts.Length < 2) { skippedCount++; continue; }

                                string fullName = parts[0].Trim();
                                string classInput = parts[1].Trim();

                                if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(classInput))
                                { skippedCount++; continue; }

                                string classNumberStr = "";
                                foreach (char c in classInput)
                                {
                                    if (char.IsDigit(c)) classNumberStr += c;
                                    else break;
                                }
                                string classLetter = classInput.Substring(classNumberStr.Length).Trim();

                                if (!int.TryParse(classNumberStr, out int classNumber) || string.IsNullOrWhiteSpace(classLetter))
                                { skippedCount++; continue; }

                                var classEntity = classes.FirstOrDefault(c =>
                                    c.ClassLevel.Number == classNumber &&
                                    c.SymbolOfClass.Equals(classLetter, StringComparison.OrdinalIgnoreCase));

                                if (classEntity == null) { skippedCount++; continue; }

                                if (db.Students.Any(s => s.FullName == fullName && s.IdClass == classEntity.Id))
                                { skippedCount++; continue; }

                                db.Students.Add(new Student
                                {
                                    FullName = fullName,
                                    IdClass = classEntity.Id,
                                    ParticipantCode = null
                                });
                                importedCount++;
                            }

                            if (importedCount > 0) db.SaveChanges();
                            LoadStudents();

                            MessageBox.Show($"Импорт завершен!\n\n✓ Добавлено: {importedCount}\n⊘ Пропущено: {skippedCount}",
                                "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка импорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.DefaultExt = "csv";
                saveFileDialog.FileName = "students_export.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var db = new VprDbContext())
                        {
                            var students = db.Students
                                .Include(s => s.Class).ThenInclude(c => c.ClassLevel)
                                .OrderBy(s => s.Id).ToList();

                            if (students.Count == 0)
                            {
                                MessageBox.Show("Нет данных для экспорта!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            var encoding = new UTF8Encoding(true);
                            using (var writer = new StreamWriter(saveFileDialog.FileName, false, encoding))
                            {
                                writer.WriteLine("ID;ФИО;Класс");
                                foreach (var student in students)
                                {
                                    string classInfo = $"{student.Class.ClassLevel.Number}{student.Class.SymbolOfClass}";
                                    writer.WriteLine($"{student.Id};{student.FullName};{classInfo}");
                                }
                            }

                            MessageBox.Show($"✓ Экспорт завершен!\nЭкспортировано учеников: {students.Count}",
                                "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}