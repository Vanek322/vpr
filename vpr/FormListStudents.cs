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
        private RoundedButton btnImportBtn, btnExportBtn, btnAddBtn, btnEditBtn, btnDeleteBtn, btnExitBtn;
        private TextBox txtSearch;
        private ComboBox cmbFilterClass;
        private List<Student> allStudents = new List<Student>();

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
            this.MinimumSize = new Size(1000, 650);

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
                Text = "Список обучающихся",
                Font = ThemeManager.FontHeading,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            });

            // Панель поиска и фильтра
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
                Size = new Size(250, 30),
                Font = ThemeManager.FontBody,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            searchPanel.Controls.Add(txtSearch);

            searchPanel.Controls.Add(new Label
            {
                Text = "Класс:",
                Font = ThemeManager.FontBody,
                ForeColor = ThemeManager.TextSecondary,
                Location = new Point(380, 15),
                AutoSize = true
            });

            cmbFilterClass = new ComboBox
            {
                Location = new Point(440, 12),
                Size = new Size(120, 30),
                Font = ThemeManager.FontBody,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            searchPanel.Controls.Add(cmbFilterClass);

            LoadClassFilter();

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

            // Нижняя панель с кнопкой Выход
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

            // DataGridView
            if (dgvStudents == null)
            {
                dgvStudents = new DataGridView();
            }

            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.AllowUserToDeleteRows = false;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.BackgroundColor = Color.White;
            dgvStudents.BorderStyle = BorderStyle.None;
            dgvStudents.ColumnHeadersHeight = 40;
            dgvStudents.ReadOnly = true;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.MultiSelect = false;
            dgvStudents.RowHeadersVisible = false;

            // Карточка таблицы
            var tableCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.Surface,
                Padding = new Padding(15)
            };
            this.Controls.Add(tableCard);
            tableCard.BringToFront();

            tableCard.Controls.Add(dgvStudents);
            dgvStudents.Dock = DockStyle.Fill;
            dgvStudents.BringToFront();

            ThemeManager.StyleDataGridView(dgvStudents);

            // Создаем колонки
            if (dgvStudents.Columns.Count == 0)
            {
                dgvStudents.Columns.AddRange(
                    new DataGridViewTextBoxColumn { Name = "№", HeaderText = "№", FillWeight = 10, ReadOnly = true },
                    new DataGridViewTextBoxColumn { Name = "ФИО", HeaderText = "ФИО", FillWeight = 65, ReadOnly = true },
                    new DataGridViewTextBoxColumn { Name = "Класс", HeaderText = "Класс", FillWeight = 25, ReadOnly = true }
                );
            }

            // Контекстное меню
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Редактировать", null, (s, e) => btnEdit_Click(this, EventArgs.Empty));
            contextMenu.Items.Add("Удалить", null, (s, e) => btnDelete_Click(this, EventArgs.Empty));
            dgvStudents.ContextMenuStrip = contextMenu;

            dgvStudents.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0) btnEdit_Click(this, EventArgs.Empty);
            };
        }

        private void LoadClassFilter()
        {
            cmbFilterClass.Items.Clear();
            cmbFilterClass.Items.Add("Все классы");

            try
            {
                using (var db = new VprDbContext())
                {
                    var classes = db.Classes
                        .Include(c => c.ClassLevel)
                        .OrderBy(c => c.ClassLevel.Number)
                        .ThenBy(c => c.SymbolOfClass)
                        .ToList();

                    foreach (var cls in classes)
                    {
                        string className = $"{cls.ClassLevel.Number}{cls.SymbolOfClass}";
                        cmbFilterClass.Items.Add(className);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки классов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cmbFilterClass.SelectedIndexChanged += CmbFilterClass_SelectedIndexChanged;
            cmbFilterClass.SelectedIndex = 0;
        }

        private void CmbFilterClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
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

        private void LoadStudents()
        {
            dgvStudents.Rows.Clear();

            try
            {
                using (var db = new VprDbContext())
                {
                    allStudents = db.Students
                        .Include(s => s.Class).ThenInclude(c => c.ClassLevel)
                        .OrderBy(s => s.Id)
                        .ToList();
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
            dgvStudents.Rows.Clear();

            if (allStudents == null || allStudents.Count == 0)
                return;

            var searchText = txtSearch.Text.Trim().ToLower();
            string selectedClass = cmbFilterClass.SelectedItem?.ToString() ?? "Все классы";

            foreach (var student in allStudents)
            {
                bool matchesSearch = string.IsNullOrWhiteSpace(searchText) ||
                                    student.FullName.ToLower().Contains(searchText);

                if (!matchesSearch) continue;

                bool matchesClass = false;

                if (selectedClass == "Все классы")
                {
                    matchesClass = true;
                }
                else if (student.Class != null && student.Class.ClassLevel != null)
                {
                    string studentClass = $"{student.Class.ClassLevel.Number}{student.Class.SymbolOfClass}";
                    matchesClass = studentClass == selectedClass;
                }

                if (matchesClass)
                {
                    int rowIndex = dgvStudents.Rows.Add();
                    var row = dgvStudents.Rows[rowIndex];

                    row.Cells["№"].Value = student.Id;
                    row.Cells["ФИО"].Value = student.FullName;

                    string classDisplay = student.Class?.ClassLevel != null
                        ? $"{student.Class.ClassLevel.Number}{student.Class.SymbolOfClass}"
                        : "—";
                    row.Cells["Класс"].Value = classDisplay;
                }
            }

            dgvStudents.Refresh();
            dgvStudents.Update();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new FormEditStudent())
            {
                if (form.ShowDialog() == DialogResult.OK)
                    LoadStudents();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Выберите ученика для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = Convert.ToInt32(dgvStudents.CurrentRow.Cells["№"].Value);
            using (var form = new FormEditStudent(id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    LoadStudents();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Выберите ученика для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = Convert.ToInt32(dgvStudents.CurrentRow.Cells["№"].Value);
            var fullName = dgvStudents.CurrentRow.Cells["ФИО"].Value.ToString();

            var result = MessageBox.Show(
                $"Удалить ученика '{fullName}'?\n\nЭто действие нельзя отменить.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var db = new VprDbContext())
                    {
                        var student = db.Students.Find(id);
                        if (student != null)
                        {
                            db.Students.Remove(student);
                            db.SaveChanges();
                            LoadStudents();
                            MessageBox.Show("Ученик удален!", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}\n\nВозможно, ученик связан с протоколами.",
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
                            var classes = db.Classes.Include(c => c.ClassLevel).ToList();
                            int importedCount = 0, skippedCount = 0;

                            foreach (var line in lines)
                            {
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

                                db.Students.Add(new Student { FullName = fullName, IdClass = classEntity.Id });
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
                                MessageBox.Show("Нет данных для экспорта!", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                            MessageBox.Show($"Экспорт завершен!\nЭкспортировано учеников: {students.Count}",
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