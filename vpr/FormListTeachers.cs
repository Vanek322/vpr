using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using vpr.Models;
using System.Linq;
using Microsoft.Data.SqlClient;

using static System.Windows.Forms.LinkLabel;

namespace vpr
{
    public partial class FormListTeachers : Form
    {
        public FormListTeachers()
        {
            InitializeComponent();

            var colId = new DataGridViewTextBoxColumn();
            colId.Name = "№";
            colId.FillWeight = 10;
            colId.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            var colTeachers = new DataGridViewTextBoxColumn();
            colTeachers.Name = "ФИО";
            colTeachers.FillWeight = 90;
            colTeachers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvTeachers.Columns.AddRange(colId, colTeachers);

            dgvTeachers.Rows.Clear();

            LoadTeachers();
        }

        private void LoadTeachers()
        {
            dgvTeachers.Rows.Clear();

            try
            {
                using (var db = new VprDbContext())
                {
                    var teachers = db.Teachers
                        .OrderBy(e => e.Id)
                        .ToList();

                    foreach (var teacher in teachers)
                    {
                        int rowIndex = dgvTeachers.Rows.Add();
                        var row = dgvTeachers.Rows[rowIndex];

                        row.Cells["№"].Value = teacher.Id;
                        row.Cells["ФИО"].Value = teacher.FullName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
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

                        // Если кодировка неправильная, пробуем Windows-1251
                        if (lines.Length > 0 && lines[0].Contains(""))
                        {
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            lines = File.ReadAllLines(openFileDialog.FileName, Encoding.GetEncoding(1251));
                        }

                        using (var db = new VprDbContext())
                        {
                            int importedCount = 0;
                            int skippedCount = 0;

                            for (int i = 0; i < lines.Length; i++)
                            {
                                var line = lines[i];

                                // Пропускаем пустые строки и заголовки
                                if (string.IsNullOrWhiteSpace(line)) continue;

                                var parts = line.Split(';');

                                // Берем ФИО - это может быть первая или вторая колонка
                                string fullName = "";

                                if (parts.Length >= 2)
                                {
                                    // Если 2 колонки (ID;ФИО) - берем вторую
                                    fullName = parts[1].Trim();
                                }
                                else if (parts.Length == 1)
                                {
                                    // Если 1 колонка (только ФИО) - берем первую
                                    fullName = parts[0].Trim();
                                }
                                else
                                {
                                    skippedCount++;
                                    continue;
                                }

                                if (string.IsNullOrWhiteSpace(fullName))
                                {
                                    skippedCount++;
                                    continue;
                                }

                                // Проверяем, нет ли уже такого учителя (по ФИО)
                                if (db.Teachers.Any(t => t.FullName == fullName))
                                {
                                    skippedCount++;
                                    continue;
                                }

                                // ID не указываем - база сгенерирует его автоматически
                                var teacher = new Teacher
                                {
                                    FullName = fullName
                                };

                                db.Teachers.Add(teacher);
                                importedCount++;
                            }

                            if (importedCount > 0)
                            {
                                db.SaveChanges();
                            }

                            LoadTeachers();

                            MessageBox.Show($"Импорт завершен!\n\nДобавлено: {importedCount}\nПропущено (пустые/дубли): {skippedCount}",
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
                saveFileDialog.FileName = "teachers_export.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var db = new VprDbContext())
                        {
                            var teachers = db.Teachers.OrderBy(t => t.Id).ToList();

                            if (teachers.Count == 0)
                            {
                                MessageBox.Show("Нет данных для экспорта!", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            // UTF-8 с BOM для корректного отображения в Excel
                            var encoding = new UTF8Encoding(true);

                            using (var writer = new StreamWriter(saveFileDialog.FileName, false, encoding))
                            {
                                // Заголовок
                                writer.WriteLine("ID;ФИО");

                                // Данные
                                foreach (var teacher in teachers)
                                {
                                    writer.WriteLine($"{teacher.Id};{teacher.FullName}");
                                }
                            }

                            MessageBox.Show($"Экспорт завершен!\nЭкспортировано учителей: {teachers.Count}",
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
