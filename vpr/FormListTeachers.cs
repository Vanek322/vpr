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
        private RoundedButton btnAddBtn;
        private RoundedButton btnEditBtn;
        private RoundedButton btnDeleteBtn;
        private RoundedButton btnExitBtn;
        private TextBox txtSearch;
        private List<Teacher> allTeachers = new List<Teacher>();

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
            this.MinimumSize = new Size(1000, 650);

            // ============================================
            // ВАЖНО: Сначала создаём DataGridView
            // ============================================
            if (dgvTeachers == null)
            {
                dgvTeachers = new DataGridView();
            }

            dgvTeachers.AllowUserToAddRows = false;
            dgvTeachers.AllowUserToDeleteRows = false;
            dgvTeachers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTeachers.BackgroundColor = Color.White;
            dgvTeachers.BorderStyle = BorderStyle.None;
            dgvTeachers.ColumnHeadersHeight = 50;
            dgvTeachers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTeachers.ReadOnly = true;
            dgvTeachers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTeachers.MultiSelect = false;
            dgvTeachers.RowHeadersVisible = false;

            // Создаем колонки
            if (dgvTeachers.Columns.Count == 0)
            {
                dgvTeachers.Columns.AddRange(
                    new DataGridViewTextBoxColumn { Name = "№", HeaderText = "№", FillWeight = 10, ReadOnly = true },
                    new DataGridViewTextBoxColumn { Name = "ФИО", HeaderText = "ФИО", FillWeight = 90, ReadOnly = true }
                );
            }

            // ============================================
            // Карточка таблицы — ДОБАВЛЯЕМ ПЕРВОЙ (Fill)
            // ============================================
            var tableCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.Surface,
                Padding = new Padding(15)
            };
            this.Controls.Add(tableCard);  // Сначала Fill элемент!

            tableCard.Controls.Add(dgvTeachers);
            dgvTeachers.Dock = DockStyle.Fill;

            ThemeManager.StyleDataGridView(dgvTeachers);

            // ============================================
            // Потом добавляем элементы с DockStyle.Top
            // ============================================

            // Заголовок
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = ThemeManager.Surface,
                Padding = new Padding(20, 15, 20, 15)
            };
            this.Controls.Add(headerPanel);
            headerPanel.Controls.Add(new Label
            {
                Text = "Список учителей",
                Font = ThemeManager.FontHeading,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            });

            // Панель поиска
            var searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 10, 20, 10)
            };
            this.Controls.Add(searchPanel);

            searchPanel.Controls.Add(new Label
            {
                Text = "Поиск:",
                Font = ThemeManager.FontBody,
                ForeColor = ThemeManager.TextSecondary,
                Location = new Point(20, 15),
                AutoSize = true
            });

            txtSearch = new TextBox
            {
                Location = new Point(110, 12),
                Size = new Size(300, 30),
                Font = ThemeManager.FontBody,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            searchPanel.Controls.Add(txtSearch);

            // Панель инструментов с центрированными кнопками
            var toolPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 10, 20, 10),
                ColumnCount = 3,
                RowCount = 1
            };
            toolPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            toolPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.Controls.Add(toolPanel);

            var buttonsContainer = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Anchor = AnchorStyles.None
            };
            toolPanel.Controls.Add(buttonsContainer, 1, 0);

            btnAddBtn = CreateToolButton("Добавить", ThemeManager.Success);
            btnAddBtn.Click += btnAdd_Click;
            buttonsContainer.Controls.Add(btnAddBtn);

            btnEditBtn = CreateToolButton("Редактировать", ThemeManager.Primary);
            btnEditBtn.Click += btnEdit_Click;
            buttonsContainer.Controls.Add(btnEditBtn);

            btnDeleteBtn = CreateToolButton("Удалить", ThemeManager.Danger);
            btnDeleteBtn.Click += btnDelete_Click;
            buttonsContainer.Controls.Add(btnDeleteBtn);

            btnImportBtn = CreateToolButton("Импорт", Color.FromArgb(255, 147, 197, 253));
            btnImportBtn.Click += btnImport_Click;
            buttonsContainer.Controls.Add(btnImportBtn);

            btnExportBtn = CreateToolButton("Экспорт", Color.FromArgb(255, 167, 139, 250));
            btnExportBtn.Click += btnExport_Click;
            buttonsContainer.Controls.Add(btnExportBtn);

            this.Resize += (s, e) =>
            {
                if (buttonsContainer.Width < toolPanel.Width)
                {
                    buttonsContainer.Location = new Point(
                        (toolPanel.Width - buttonsContainer.Width) / 2,
                        (toolPanel.Height - buttonsContainer.Height) / 2
                    );
                }
            };

            // ============================================
            // В конце добавляем DockStyle.Bottom
            // ============================================
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.Transparent,
                Padding = new Padding(20)
            };
            this.Controls.Add(bottomPanel);

            btnExitBtn = CreateToolButton("Выход", ThemeManager.Danger);
            btnExitBtn.Size = new Size(150, 45);
            btnExitBtn.Click += (s, e) => this.Close();
            bottomPanel.Controls.Add(btnExitBtn);
            btnExitBtn.Anchor = AnchorStyles.None;
            btnExitBtn.Location = new Point((bottomPanel.Width - btnExitBtn.Width) / 2,
                                             (bottomPanel.Height - btnExitBtn.Height) / 2);

            // Контекстное меню
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Редактировать", null, (s, e) => btnEdit_Click(this, EventArgs.Empty));
            contextMenu.Items.Add("Удалить", null, (s, e) => btnDelete_Click(this, EventArgs.Empty));
            dgvTeachers.ContextMenuStrip = contextMenu;

            dgvTeachers.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0) btnEdit_Click(this, EventArgs.Empty);
            };
        }

        private RoundedButton CreateToolButton(string text, Color color)
        {
            return new RoundedButton
            {
                Text = text,
                Size = new Size(150, 40),
                BackColor = color,
                ForeColor = Color.White,
                BorderRadius = 10,
                Margin = new Padding(5, 0, 5, 0),
                Font = ThemeManager.FontButton
            };
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
                    FillWeight = 10,
                    ReadOnly = true
                };
                var colTeachers = new DataGridViewTextBoxColumn
                {
                    Name = "ФИО",
                    HeaderText = "ФИО",
                    FillWeight = 90,
                    ReadOnly = true
                };
                dgvTeachers.Columns.AddRange(colId, colTeachers);
            }

            try
            {
                using (var db = new VprDbContext())
                {
                    allTeachers = db.Teachers.OrderBy(e => e.Id).ToList();
                    ApplyFilter();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter()
        {
            dgvTeachers.Rows.Clear();
            var searchText = txtSearch.Text.Trim().ToLower();

            var filtered = string.IsNullOrWhiteSpace(searchText)
                ? allTeachers
                : allTeachers.Where(t => t.FullName.ToLower().Contains(searchText)).ToList();

            foreach (var teacher in filtered)
            {
                int rowIndex = dgvTeachers.Rows.Add();
                dgvTeachers.Rows[rowIndex].Cells["№"].Value = teacher.Id;
                dgvTeachers.Rows[rowIndex].Cells["ФИО"].Value = teacher.FullName;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new FormEditTeacher())
            {
                if (form.ShowDialog() == DialogResult.OK)
                    LoadTeachers();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTeachers.CurrentRow == null)
            {
                MessageBox.Show("Выберите учителя для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = Convert.ToInt32(dgvTeachers.CurrentRow.Cells["№"].Value);
            using (var form = new FormEditTeacher(id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    LoadTeachers();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTeachers.CurrentRow == null)
            {
                MessageBox.Show("Выберите учителя для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = Convert.ToInt32(dgvTeachers.CurrentRow.Cells["№"].Value);
            var fullName = dgvTeachers.CurrentRow.Cells["ФИО"].Value.ToString();

            var result = MessageBox.Show(
                $"Удалить учителя '{fullName}'?\n\nЭто действие нельзя отменить.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var db = new VprDbContext())
                    {
                        var teacher = db.Teachers.Find(id);
                        if (teacher != null)
                        {
                            db.Teachers.Remove(teacher);
                            db.SaveChanges();
                            LoadTeachers();
                            MessageBox.Show("Учитель удален!", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}\n\nВозможно, учитель связан с другими записями.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

                            for (int i = 0; i < lines.Length; i++)
                            {
                                var line = lines[i];
                                if (string.IsNullOrWhiteSpace(line)) continue;

                                var parts = line.Split(';');
                                string fullName = "";

                                if (parts.Length >= 2) fullName = parts[1].Trim();
                                else if (parts.Length == 1) fullName = parts[0].Trim();
                                else { skippedCount++; continue; }

                                if (string.IsNullOrWhiteSpace(fullName)) { skippedCount++; continue; }
                                if (db.Teachers.Any(t => t.FullName == fullName)) { skippedCount++; continue; }

                                db.Teachers.Add(new Teacher { FullName = fullName });
                                importedCount++;
                            }

                            if (importedCount > 0) db.SaveChanges();
                            LoadTeachers();

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
                                    writer.WriteLine($"{teacher.Id};{teacher.FullName}");
                            }

                            MessageBox.Show($"Экспорт завершен!\nЭкспортировано учителей: {teachers.Count}",
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