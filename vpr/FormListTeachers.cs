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
        private OpenFileDialog openFileDialogImport;
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

        private void btnImport_Click(object sender, EventArgs e)
        {
            openFileDialogImport = new OpenFileDialog();
            openFileDialogImport.Filter = "csv files(*.csv)|*.csv";
            openFileDialogImport.InitialDirectory = "C:\\";

            if (openFileDialogImport.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialogImport.FileName;
                string readText = File.ReadAllText(filePath);
                List<string> listStrLineElements = readText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();


                List<string> rowList = listStrLineElements.SelectMany(s => s.Split(';')).ToList();

        private void sqlCon(List<string> x)
        {
            SqlConnection myConnection = new SqlConnection("user id=postgres;" + "password=1111;server=PostgreSQL;" + "Trusted_Connection=yes;" + "database=vpr_db; " + "connection timeout=30");
            try
            {
                myConnection.Open();
                for (int i = 0; i <= x.Count - 2; i += 2)
                {
                    //Replace table_name with your table name, and Column1 with your column names (replace for all).
                    SqlCommand myCommand = new SqlCommand("INSERT INTO teachers (id, full_name) " +
                                         String.Format("Values ('{0}','{1}')", x[i], x[i + 1]), myConnection);
                    myCommand.ExecuteNonQuery();
                }

            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            try { myConnection.Close(); }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }
    }
}
