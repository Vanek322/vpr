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
    public partial class FormListProtocols : Form
    {
        private RoundedButton btnImportBtn;
        private RoundedButton btnExportBtn;
        private RoundedButton btnExitBtn;

        public FormListProtocols()
        {
            InitializeComponent();
            SetupDesign();
            LoadProtocols();
        }

        private void SetupDesign()
        {
            ThemeManager.ApplyTheme(this);
            this.Padding = new Padding(20);
            this.MinimumSize = new Size(1100, 650);

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
                Text = " Протоколы результатов ВПР",
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
                Text = "✕ Выход",
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

            dgvProtocols.Parent = tableCard;
            dgvProtocols.Dock = DockStyle.Fill;
            dgvProtocols.BringToFront();
            ThemeManager.StyleDataGridView(dgvProtocols);
        }

        private void LoadProtocols()
        {
            dgvProtocols.Rows.Clear();

            if (dgvProtocols.Columns.Count == 0)
            {
                var colId = new DataGridViewTextBoxColumn { Name = "№", HeaderText = "№", FillWeight = 6, ReadOnly = true };
                var colStudent = new DataGridViewTextBoxColumn { Name = "Ученик", HeaderText = "Ученик", FillWeight = 35, ReadOnly = true };
                var colClass = new DataGridViewTextBoxColumn { Name = "Класс", HeaderText = "Класс", FillWeight = 8, ReadOnly = true };
                var colSubject = new DataGridViewTextBoxColumn { Name = "Предмет", HeaderText = "Предмет", FillWeight = 18, ReadOnly = true };
                var colVariant = new DataGridViewTextBoxColumn { Name = "Вариант", HeaderText = "Вариант", FillWeight = 9, ReadOnly = true };
                var colPrevGrade = new DataGridViewTextBoxColumn { Name = "Прошлогодняя оценка", HeaderText = "Оценка (прош. год)", FillWeight = 14, ReadOnly = true };
                var colTotalScore = new DataGridViewTextBoxColumn { Name = "Всего баллов", HeaderText = "Всего баллов", FillWeight = 10, ReadOnly = true };

                dgvProtocols.Columns.AddRange(colId, colStudent, colClass, colSubject, colVariant, colPrevGrade, colTotalScore);
            }

            try
            {
                using (var db = new VprDbContext())
                {
                    var protocols = db.Protocols
                        .Include(p => p.TeacherAssignment.Class.ClassLevel)
                        .Include(p => p.TeacherAssignment.Subject)
                        .Include(p => p.Student)
                        .OrderBy(p => p.Id)
                        .ToList();

                    foreach (var protocol in protocols)
                    {
                        int rowIndex = dgvProtocols.Rows.Add();
                        var row = dgvProtocols.Rows[rowIndex];

                        row.Cells["№"].Value = protocol.Id;
                        row.Cells["Ученик"].Value = protocol.Student.FullName;
                        row.Cells["Класс"].Value = $"{protocol.TeacherAssignment.Class.ClassLevel.Number}{protocol.TeacherAssignment.Class.SymbolOfClass}";
                        row.Cells["Предмет"].Value = protocol.TeacherAssignment.Subject.Name;
                        row.Cells["Вариант"].Value = protocol.Variant;
                        row.Cells["Прошлогодняя оценка"].Value = protocol.PreviousGrade.HasValue ? protocol.PreviousGrade.Value.ToString() : "—";
                        row.Cells["Всего баллов"].Value = protocol.TotalScore;
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
                            int importedCount = 0, skippedCount = 0;
                            var errors = new List<string>();

                            for (int i = 0; i < lines.Length; i++)
                            {
                                var line = lines[i];
                                if (string.IsNullOrWhiteSpace(line)) continue;

                                var parts = line.Split(';');
                                if (parts.Length < 6) { skippedCount++; continue; }

                                if (!int.TryParse(parts[0].Trim(), out int studentId)) { skippedCount++; continue; }
                                if (!int.TryParse(parts[1].Trim(), out int teacherAssignmentId)) { skippedCount++; continue; }
                                if (!int.TryParse(parts[2].Trim(), out int subjectId)) { skippedCount++; continue; }
                                if (!int.TryParse(parts[3].Trim(), out int variant)) { skippedCount++; continue; }

                                int? previousGrade = null;
                                if (!string.IsNullOrWhiteSpace(parts[4].Trim()) && int.TryParse(parts[4].Trim(), out int pg))
                                    previousGrade = pg;

                                if (!int.TryParse(parts[5].Trim(), out int totalScore)) { skippedCount++; continue; }

                                if (!db.Students.Any(s => s.Id == studentId)) { errors.Add($"Ученик ID={studentId} не найден"); skippedCount++; continue; }
                                if (!db.Subjects.Any(s => s.Id == subjectId)) { errors.Add($"Предмет ID={subjectId} не найден"); skippedCount++; continue; }
                                if (!db.TeacherAssignments.Any(ta => ta.Id == teacherAssignmentId)) { errors.Add($"Назначение ID={teacherAssignmentId} не найдено"); skippedCount++; continue; }

                                if (db.Protocols.Any(p => p.IdStudent == studentId && p.IdTeacherAssignment == teacherAssignmentId && p.Variant == variant))
                                { skippedCount++; continue; }

                                db.Protocols.Add(new Protocol
                                {
                                    IdStudent = studentId,
                                    IdTeacherAssignment = teacherAssignmentId,
                                    Variant = variant,
                                    PreviousGrade = previousGrade,
                                    TotalScore = totalScore
                                });
                                importedCount++;
                            }

                            if (importedCount > 0) db.SaveChanges();
                            LoadProtocols();

                            string errorMessage = errors.Count > 0 ? "\n\nОшибки:\n" + string.Join("\n", errors.Take(5)) : "";
                            if (errors.Count > 5) errorMessage += $"\n... и еще {errors.Count - 5} ошибок";

                            MessageBox.Show($"Импорт завершен!\n\n✓ Добавлено: {importedCount}\n Пропущено: {skippedCount}{errorMessage}",
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
                saveFileDialog.FileName = "protocols_export.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var db = new VprDbContext())
                        {
                            var protocols = db.Protocols
                                .Include(p => p.Student)
                                .Include(p => p.TeacherAssignment).ThenInclude(ta => ta.Class).ThenInclude(c => c.ClassLevel)
                                .Include(p => p.TeacherAssignment).ThenInclude(ta => ta.Subject)
                                .OrderBy(p => p.Id).ToList();

                            if (protocols.Count == 0)
                            {
                                MessageBox.Show("Нет данных для экспорта!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            var encoding = new UTF8Encoding(true);
                            using (var writer = new StreamWriter(saveFileDialog.FileName, false, encoding))
                            {
                                writer.WriteLine("ID;ФИО ученика;Класс;Предмет;Вариант;Прошлогодняя оценка;Всего баллов");

                                foreach (var protocol in protocols)
                                {
                                    string classInfo = $"{protocol.TeacherAssignment.Class.ClassLevel.Number}{protocol.TeacherAssignment.Class.SymbolOfClass}";
                                    string previousGrade = protocol.PreviousGrade.HasValue ? protocol.PreviousGrade.Value.ToString() : "";
                                    writer.WriteLine($"{protocol.Id};{protocol.Student.FullName};{classInfo};{protocol.TeacherAssignment.Subject.Name};{protocol.Variant};{previousGrade};{protocol.TotalScore}");
                                }
                            }

                            MessageBox.Show($"✓ Экспорт завершен!\nЭкспортировано протоколов: {protocols.Count}",
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