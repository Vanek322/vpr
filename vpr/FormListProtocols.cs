using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using vpr.Models;

namespace vpr
{
    public partial class FormListProtocols : Form
    {
        public FormListProtocols()
        {
            InitializeComponent();

            var colId = new DataGridViewTextBoxColumn();
            colId.Name = "№";
            colId.FillWeight = 6;
            colId.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            var colStudents = new DataGridViewTextBoxColumn();
            colStudents.Name = "Ученик";
            colStudents.FillWeight = 40;
            colStudents.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colClass = new DataGridViewTextBoxColumn();
            colClass.Name = "Класс";
            colClass.FillWeight = 7;
            colClass.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colSubject = new DataGridViewTextBoxColumn();
            colSubject.Name = "Предмет";
            colSubject.FillWeight = 13;
            colSubject.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colVariant = new DataGridViewTextBoxColumn();
            colVariant.Name = "Вариант";
            colVariant.FillWeight = 8;
            colVariant.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colPreviousGrade = new DataGridViewTextBoxColumn();
            colPreviousGrade.Name = "Прошлогодняя оценка";
            colPreviousGrade.FillWeight = 16;
            colPreviousGrade.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colTotalScore = new DataGridViewTextBoxColumn();
            colTotalScore.Name = "Всего баллов";
            colTotalScore.FillWeight = 10;
            colTotalScore.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProtocols.Columns.AddRange(colId, colStudents, colClass, colSubject, colVariant, colPreviousGrade, colTotalScore);

            dgvProtocols.Rows.Clear();

            LoadProtocols();
        }

        private void LoadProtocols()
        {
            dgvProtocols.Rows.Clear();
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
                        row.Cells["Класс"].Value = protocol.TeacherAssignment.Class.ClassLevel.Number;
                        row.Cells["Предмет"].Value = protocol.TeacherAssignment.Subject.Name;
                        row.Cells["Вариант"].Value = protocol.Variant;
                        row.Cells["Прошлогодняя оценка"].Value = protocol.PreviousGrade;
                        row.Cells["Всего баллов"].Value = protocol.TotalScore;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
                            int importedCount = 0;
                            int skippedCount = 0;
                            var errors = new List<string>();

                            for (int i = 0; i < lines.Length; i++)
                            {
                                var line = lines[i];

                                if (string.IsNullOrWhiteSpace(line)) continue;

                                var parts = line.Split(';');

                                // Формат: ID_ученика;ID_предмета;ID_назначения_учителя;Вариант;Прошлогодняя оценка;Всего баллов
                                // Пример: 1;5;3;1;4;15
                                if (parts.Length < 6) { skippedCount++; continue; }

                                // Парсим ID
                                if (!int.TryParse(parts[0].Trim(), out int studentId)) { skippedCount++; continue; }
                                if (!int.TryParse(parts[1].Trim(), out int teacherAssignmentId)) { skippedCount++; continue; }
                                if (!int.TryParse(parts[2].Trim(), out int subjectId)) { skippedCount++; continue; }
                                if (!int.TryParse(parts[3].Trim(), out int variant)) { skippedCount++; continue; }

                                int? previousGrade = null;
                                if (!string.IsNullOrWhiteSpace(parts[4].Trim()))
                                {
                                    if (int.TryParse(parts[4].Trim(), out int pg))
                                        previousGrade = pg;
                                }

                                if (!int.TryParse(parts[5].Trim(), out int totalScore)) { skippedCount++; continue; }

                                // Проверяем, что ученик существует
                                if (!db.Students.Any(s => s.Id == studentId))
                                {
                                    errors.Add($"Ученик с ID={studentId} не найден");
                                    skippedCount++;
                                    continue;
                                }

                                // Проверяем, что предмет существует
                                if (!db.Subjects.Any(s => s.Id == subjectId))
                                {
                                    errors.Add($"Предмет с ID={subjectId} не найден");
                                    skippedCount++;
                                    continue;
                                }

                                // Проверяем, что назначение учителя существует
                                if (!db.TeacherAssignments.Any(ta => ta.Id == teacherAssignmentId))
                                {
                                    errors.Add($"Назначение учителя с ID={teacherAssignmentId} не найдено");
                                    skippedCount++;
                                    continue;
                                }

                                // Проверяем дубликат протокола
                                var existingProtocol = db.Protocols.FirstOrDefault(p =>
                                    p.IdStudent == studentId &&
                                    p.IdTeacherAssignment == teacherAssignmentId &&
                                    p.Variant == variant);

                                if (existingProtocol != null)
                                {
                                    skippedCount++;
                                    continue;
                                }

                                var protocol = new Protocol
                                {
                                    IdStudent = studentId,
                                    IdTeacherAssignment = teacherAssignmentId,
                                    Variant = variant,
                                    PreviousGrade = previousGrade,
                                    TotalScore = totalScore
                                };

                                db.Protocols.Add(protocol);
                                importedCount++;
                            }

                            if (importedCount > 0)
                            {
                                db.SaveChanges();
                            }

                            LoadProtocols();

                            string errorMessage = errors.Count > 0 ? "\n\nОшибки:\n" + string.Join("\n", errors.Take(5)) : "";
                            if (errors.Count > 5) errorMessage += $"\n... и еще {errors.Count - 5} ошибок";

                            MessageBox.Show($"Импорт протоколов завершен!\n\nДобавлено: {importedCount}\nПропущено: {skippedCount}{errorMessage}",
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
                                .Include(p => p.TeacherAssignment)
                                .ThenInclude(ta => ta.Class)
                                .ThenInclude(c => c.ClassLevel)
                                .Include(p => p.TeacherAssignment)
                                .ThenInclude(ta => ta.Subject)
                                .OrderBy(p => p.Id)
                                .ToList();

                            if (protocols.Count == 0)
                            {
                                MessageBox.Show("Нет данных для экспорта!", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            var encoding = new UTF8Encoding(true);

                            using (var writer = new StreamWriter(saveFileDialog.FileName, false, encoding))
                            {
                                // Заголовок
                                writer.WriteLine("ID;ФИО ученика;Класс;Предмет;Вариант;Прошлогодняя оценка;Всего баллов");

                                // Данные
                                foreach (var protocol in protocols)
                                {
                                    string classInfo = $"{protocol.TeacherAssignment.Class.ClassLevel.Number}{protocol.TeacherAssignment.Class.SymbolOfClass}";
                                    string previousGrade = protocol.PreviousGrade.HasValue ? protocol.PreviousGrade.Value.ToString() : "";

                                    writer.WriteLine($"{protocol.Id};{protocol.Student.FullName};{classInfo};{protocol.TeacherAssignment.Subject.Name};{protocol.Variant};{previousGrade};{protocol.TotalScore}");
                                }
                            }

                            MessageBox.Show($"Экспорт завершен!\nЭкспортировано протоколов: {protocols.Count}",
                                "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
