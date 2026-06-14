using Microsoft.EntityFrameworkCore;
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
    public partial class FormListStudents : Form
    {
        public FormListStudents()
        {
            InitializeComponent();

            var colId = new DataGridViewTextBoxColumn();
            colId.Name = "№";
            colId.FillWeight = 10;
            colId.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            var colStudents = new DataGridViewTextBoxColumn();
            colStudents.Name = "ФИО";
            colStudents.FillWeight = 90;
            colStudents.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvStudents.Columns.AddRange(colId, colStudents);

            dgvStudents.Rows.Clear();

            LoadStudents();
        }

        private void LoadStudents()
        {
            dgvStudents.Rows.Clear();
            try
            {
                using (var db = new VprDbContext())
                {
                    var students = db.Students
                        .OrderBy(s => s.Id)
                        .ToList();

                    foreach (var student in students)
                    {
                        int rowIndex = dgvStudents.Rows.Add();
                        var row = dgvStudents.Rows[rowIndex];

                        row.Cells["№"].Value = student.Id;
                        row.Cells["ФИО"].Value = student.FullName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void testImprort()
        {

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
                            // Загружаем классы и их уровни
                            var classes = db.Classes.Include(c => c.ClassLevel).ToList();

                            int importedCount = 0;
                            int skippedCount = 0;

                            for (int i = 0; i < lines.Length; i++)
                            {
                                var line = lines[i];

                                if (string.IsNullOrWhiteSpace(line)) continue;

                                var parts = line.Split(';');

                                // Формат: ФИО;Класс (например: "Иванов И.И.;5А" или "Иванов И.И.;10 Б")
                                if (parts.Length < 2) { skippedCount++; continue; }

                                string fullName = parts[0].Trim();
                                string classInput = parts[1].Trim(); // Например: "5А", "10Б", "5 А"

                                if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(classInput))
                                {
                                    skippedCount++;
                                    continue;
                                }

                                // Парсим класс: извлекаем цифру и букву
                                string classNumberStr = "";
                                string classLetter = "";

                                // Извлекаем цифры из начала строки
                                foreach (char c in classInput)
                                {
                                    if (char.IsDigit(c))
                                        classNumberStr += c;
                                    else
                                        break;
                                }

                                // Извлекаем букву (после цифр, пропуская пробелы)
                                classLetter = classInput.Substring(classNumberStr.Length).Trim();

                                if (!int.TryParse(classNumberStr, out int classNumber) || string.IsNullOrWhiteSpace(classLetter))
                                {
                                    skippedCount++;
                                    continue;
                                }

                                // Ищем класс: по номеру (в ClassLevel) и по букве (в Class)
                                var classEntity = classes.FirstOrDefault(c =>
                                    c.ClassLevel.Number == classNumber &&
                                    c.SymbolOfClass.Equals(classLetter, StringComparison.OrdinalIgnoreCase));

                                if (classEntity == null)
                                {
                                    MessageBox.Show($"Класс {classNumber}{classLetter} не найден! Пропущена запись: {fullName}",
                                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    skippedCount++;
                                    continue;
                                }

                                // Проверяем дубликаты
                                if (db.Students.Any(s => s.FullName == fullName && s.IdClass == classEntity.Id))
                                {
                                    skippedCount++;
                                    continue;
                                }

                                var student = new Student
                                {
                                    FullName = fullName,
                                    IdClass = classEntity.Id,
                                    ParticipantCode = null
                                };

                                db.Students.Add(student);
                                importedCount++;
                            }

                            if (importedCount > 0)
                            {
                                db.SaveChanges();
                            }

                            LoadStudents();

                            MessageBox.Show($"Импорт учеников завершен!\n\nДобавлено: {importedCount}\nПропущено: {skippedCount}",
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
                                .Include(s => s.Class)
                                .ThenInclude(c => c.ClassLevel)
                                .OrderBy(s => s.Id)
                                .ToList();

                            if (students.Count == 0)
                            {
                                MessageBox.Show("Нет данных для экспорта!", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            var encoding = new UTF8Encoding(true);

                            using (var writer = new StreamWriter(saveFileDialog.FileName, false, encoding))
                            {
                                // Заголовок
                                writer.WriteLine("ID;ФИО;Класс");

                                // Данные
                                foreach (var student in students)
                                {
                                    string classInfo = $"{student.Class.ClassLevel.Number}{student.Class.SymbolOfClass}";
                                    writer.WriteLine($"{student.Id};{student.FullName};{classInfo}");
                                }
                            }

                            MessageBox.Show($"Экспорт завершен!\nЭкспортировано учеников: {students.Count}",
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