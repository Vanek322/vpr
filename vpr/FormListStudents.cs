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
    }
}
