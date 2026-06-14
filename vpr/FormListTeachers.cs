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
    public partial class FormListTeachers : Form
    {
        private RoundedButton btnImportBtn;
        private RoundedButton btnExportBtn;
        private RoundedButton btnExitBtn;

        public FormListTeachers()
        {
            InitializeComponent();
            SetupDesign();
            LoadTeachers();
        }

        private void SetupDesign()
        {
            ThemeManager.ApplyTheme(this);
            this.Padding = new Padding(20);
            this.MinimumSize = new Size(900, 600);

            // Заголовок
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
                Text = "👨‍🏫 Список учителей",
                Font = ThemeManager.FontHeading,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            headerPanel.Controls.Add(lblTitle);

            // Панель инструментов
            var toolPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 10, 20, 10)
            };
            this.Controls.Add(toolPanel);

            // Создаем кнопки программно
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

            // Карточка для таблицы
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

            // Переносим DataGridView в карточку
            dgvTeachers.Parent = tableCard;
            dgvTeachers.Dock = DockStyle.Fill;
            dgvTeachers.BringToFront();
            ThemeManager.StyleDataGridView(dgvTeachers);
        }

        private void LoadTeachers()
        {
            dgvTeachers.Rows.Clear();

            if (dgvTeachers.Columns.Count == 0)
            {
                var colId = new DataGridViewTextBoxColumn
                {
                    Name = "№",
                    HeaderText = "№",
                    FillWeight = 15,
                    ReadOnly = true
                };

                var colTeachers = new DataGridViewTextBoxColumn
                {
                    Name = "ФИО",
                    HeaderText = "ФИО",
                    FillWeight = 85,
                    ReadOnly = true
                };

                dgvTeachers.Columns.AddRange(colId, colTeachers);
            }

            try
            {
                using (var db = new VprDbContext())
                {
                    var teachers = db.Teachers.OrderBy(e => e.Id).ToList();

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

                            for (int i = 0; i < lines.Length; i++)
                            {
                                var line = lines[i];
                                if (string.IsNullOrWhiteSpace(line)) continue;

                                var parts = line.Split(';');
                                string fullName = "";

                                if (parts.Length >= 2)
                                    fullName = parts[1].Trim();
                                else if (parts.Length == 1)
                                    fullName = parts[0].Trim();
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

                                if (db.Teachers.Any(t => t.FullName == fullName))
                                {
                                    skippedCount++;
                                    continue;
                                }

                                var teacher = new Teacher { FullName = fullName };
                                db.Teachers.Add(teacher);
                                importedCount++;
                            }

                            if (importedCount > 0)
                                db.SaveChanges();

                            LoadTeachers();

                            MessageBox.Show(
                                $"Импорт завершен!\n\n✓ Добавлено: {importedCount}\n⊘ Пропущено: {skippedCount}",
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

                            var encoding = new UTF8Encoding(true);

                            using (var writer = new StreamWriter(saveFileDialog.FileName, false, encoding))
                            {
                                writer.WriteLine("ID;ФИО");

                                foreach (var teacher in teachers)
                                {
                                    writer.WriteLine($"{teacher.Id};{teacher.FullName}");
                                }
                            }

                            MessageBox.Show($"✓ Экспорт завершен!\nЭкспортировано учителей: {teachers.Count}",
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