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
        private RoundedButton btnImportBtn, btnExportBtn, btnAddBtn, btnEditBtn, btnDeleteBtn, btnExitBtn;
        private TextBox txtSearch;
        private ComboBox cmbFilterClass;
        private ComboBox cmbFilterSubject;
        private List<Protocol> allProtocols = new List<Protocol>();

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
            this.MinimumSize = new Size(1200, 700);

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
                Text = "Протоколы результатов ВПР",
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
                Size = new Size(200, 30),
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
                Location = new Point(330, 15),
                AutoSize = true
            });

            cmbFilterClass = new ComboBox
            {
                Location = new Point(390, 12),
                Size = new Size(100, 30),
                Font = ThemeManager.FontBody,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            searchPanel.Controls.Add(cmbFilterClass);

            searchPanel.Controls.Add(new Label
            {
                Text = "Предмет:",
                Font = ThemeManager.FontBody,
                ForeColor = ThemeManager.TextSecondary,
                Location = new Point(510, 15),
                AutoSize = true
            });

            cmbFilterSubject = new ComboBox
            {
                Location = new Point(580, 12),
                Size = new Size(150, 30),
                Font = ThemeManager.FontBody,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            searchPanel.Controls.Add(cmbFilterSubject);

            LoadFilters();

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
            if (dgvProtocols == null)
            {
                dgvProtocols = new DataGridView();
            }

            dgvProtocols.AllowUserToAddRows = false;
            dgvProtocols.AllowUserToDeleteRows = false;
            dgvProtocols.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProtocols.BackgroundColor = Color.White;
            dgvProtocols.BorderStyle = BorderStyle.None;
            dgvProtocols.ColumnHeadersHeight = 60;  // Увеличено с 40 до 60
            dgvProtocols.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvProtocols.ReadOnly = true;
            dgvProtocols.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProtocols.MultiSelect = false;
            dgvProtocols.RowHeadersVisible = false;

            // Карточка таблицы
            var tableCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.Surface,
                Padding = new Padding(15)
            };
            this.Controls.Add(tableCard);
            tableCard.BringToFront();

            tableCard.Controls.Add(dgvProtocols);
            dgvProtocols.Dock = DockStyle.Fill;
            dgvProtocols.BringToFront();

            ThemeManager.StyleDataGridView(dgvProtocols);

            // Создаем колонки
            if (dgvProtocols.Columns.Count == 0)
            {
                dgvProtocols.Columns.AddRange(
                    new DataGridViewTextBoxColumn { Name = "№", HeaderText = "№", FillWeight = 6, ReadOnly = true },
                    new DataGridViewTextBoxColumn { Name = "Ученик", HeaderText = "Ученик", FillWeight = 26, ReadOnly = true },
                    new DataGridViewTextBoxColumn { Name = "Класс", HeaderText = "Класс", FillWeight = 8, ReadOnly = true },
                    new DataGridViewTextBoxColumn { Name = "Предмет", HeaderText = "Предмет", FillWeight = 14, ReadOnly = true },
                    new DataGridViewTextBoxColumn { Name = "Вариант", HeaderText = "Вариант", FillWeight = 9, ReadOnly = true },
                    new DataGridViewTextBoxColumn { Name = "Прошлогодняя оценка", HeaderText = "Оценка (прош. год)", FillWeight = 14, ReadOnly = true },
                    new DataGridViewTextBoxColumn { Name = "Всего баллов", HeaderText = "Всего баллов", FillWeight = 10, ReadOnly = true }
                );
            }

            // Контекстное меню
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Редактировать", null, (s, e) => btnEdit_Click(this, EventArgs.Empty));
            contextMenu.Items.Add("️Удалить", null, (s, e) => btnDelete_Click(this, EventArgs.Empty));
            dgvProtocols.ContextMenuStrip = contextMenu;

            dgvProtocols.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0) btnEdit_Click(this, EventArgs.Empty);
            };
        }

        private void LoadFilters()
        {
            // Загружаем классы
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
                        cmbFilterClass.Items.Add($"{cls.ClassLevel.Number}{cls.SymbolOfClass}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки классов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cmbFilterClass.SelectedIndexChanged += (s, e) => ApplyFilter();
            cmbFilterClass.SelectedIndex = 0;

            // Загружаем предметы
            cmbFilterSubject.Items.Clear();
            cmbFilterSubject.Items.Add("Все предметы");

            try
            {
                using (var db = new VprDbContext())
                {
                    var subjects = db.Subjects.OrderBy(s => s.Name).ToList();
                    foreach (var subject in subjects)
                        cmbFilterSubject.Items.Add(subject.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки предметов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cmbFilterSubject.SelectedIndexChanged += (s, e) => ApplyFilter();
            cmbFilterSubject.SelectedIndex = 0;
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

        private void LoadProtocols()
        {
            dgvProtocols.Rows.Clear();

            try
            {
                using (var db = new VprDbContext())
                {
                    allProtocols = db.Protocols
                        .Include(p => p.Student)
                        .Include(p => p.TeacherAssignment).ThenInclude(ta => ta.Class).ThenInclude(c => c.ClassLevel)
                        .Include(p => p.TeacherAssignment).ThenInclude(ta => ta.Subject)
                        .OrderBy(p => p.Id)
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
            dgvProtocols.Rows.Clear();

            if (allProtocols == null || allProtocols.Count == 0)
                return;

            var searchText = txtSearch.Text.Trim().ToLower();
            string selectedClass = cmbFilterClass.SelectedItem?.ToString() ?? "Все классы";
            string selectedSubject = cmbFilterSubject.SelectedItem?.ToString() ?? "Все предметы";

            foreach (var protocol in allProtocols)
            {
                // Проверка по поиску (ФИО ученика)
                bool matchesSearch = string.IsNullOrWhiteSpace(searchText) ||
                                    protocol.Student.FullName.ToLower().Contains(searchText);

                if (!matchesSearch) continue;

                // Проверка по классу
                bool matchesClass = selectedClass == "Все классы";
                if (!matchesClass && protocol.TeacherAssignment?.Class?.ClassLevel != null)
                {
                    string protocolClass = $"{protocol.TeacherAssignment.Class.ClassLevel.Number}{protocol.TeacherAssignment.Class.SymbolOfClass}";
                    matchesClass = protocolClass == selectedClass;
                }

                if (!matchesClass) continue;

                // Проверка по предмету
                bool matchesSubject = selectedSubject == "Все предметы";
                if (!matchesSubject && protocol.TeacherAssignment?.Subject != null)
                {
                    matchesSubject = protocol.TeacherAssignment.Subject.Name == selectedSubject;
                }

                if (matchesSubject)
                {
                    int rowIndex = dgvProtocols.Rows.Add();
                    var row = dgvProtocols.Rows[rowIndex];

                    row.Cells["№"].Value = protocol.Id;
                    row.Cells["Ученик"].Value = protocol.Student.FullName;

                    string classDisplay = protocol.TeacherAssignment?.Class?.ClassLevel != null
                        ? $"{protocol.TeacherAssignment.Class.ClassLevel.Number}{protocol.TeacherAssignment.Class.SymbolOfClass}"
                        : "—";
                    row.Cells["Класс"].Value = classDisplay;

                    row.Cells["Предмет"].Value = protocol.TeacherAssignment?.Subject?.Name ?? "—";
                    row.Cells["Вариант"].Value = protocol.Variant;
                    row.Cells["Прошлогодняя оценка"].Value = protocol.PreviousGrade.HasValue ? protocol.PreviousGrade.Value.ToString() : "—";
                    row.Cells["Всего баллов"].Value = protocol.TotalScore;
                }
            }

            dgvProtocols.Refresh();
            dgvProtocols.Update();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new FormEditProtocol())
            {
                if (form.ShowDialog() == DialogResult.OK)
                    LoadProtocols();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProtocols.CurrentRow == null)
            {
                MessageBox.Show("Выберите протокол для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = Convert.ToInt32(dgvProtocols.CurrentRow.Cells["№"].Value);
            using (var form = new FormEditProtocol(id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    LoadProtocols();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProtocols.CurrentRow == null)
            {
                MessageBox.Show("Выберите протокол для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = Convert.ToInt32(dgvProtocols.CurrentRow.Cells["№"].Value);
            var studentName = dgvProtocols.CurrentRow.Cells["Ученик"].Value.ToString();

            var result = MessageBox.Show(
                $"Удалить протокол ученика '{studentName}'?\n\nЭто действие нельзя отменить.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var db = new VprDbContext())
                    {
                        var protocol = db.Protocols.Find(id);
                        if (protocol != null)
                        {
                            db.Protocols.Remove(protocol);
                            db.SaveChanges();
                            LoadProtocols();
                            MessageBox.Show("Протокол удален!", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            var errors = new List<string>();

                            foreach (var line in lines)
                            {
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

                            MessageBox.Show($"Импорт завершен!\n\n✓ Добавлено: {importedCount}\n⊘ Пропущено: {skippedCount}{errorMessage}",
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

                            MessageBox.Show($"Экспорт завершен!\nЭкспортировано протоколов: {protocols.Count}",
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