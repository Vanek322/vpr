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

            LoadTeachers();
        }

        private void LoadTeachers()
        {
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
    }
}
