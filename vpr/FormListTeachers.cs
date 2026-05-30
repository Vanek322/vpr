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
    }
}
