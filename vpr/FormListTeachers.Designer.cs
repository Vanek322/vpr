namespace vpr
{
    partial class FormListTeachers
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dgvTeachers = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvTeachers).BeginInit();
            SuspendLayout();
            // 
            // dgvTeachers
            // 
            dgvTeachers.AllowUserToAddRows = false;
            dgvTeachers.AllowUserToDeleteRows = false;
            dgvTeachers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTeachers.BackgroundColor = Color.White;
            dgvTeachers.BorderStyle = BorderStyle.None;
            dgvTeachers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTeachers.Location = new Point(0, 0);
            dgvTeachers.Name = "dgvTeachers";
            dgvTeachers.ReadOnly = true;
            dgvTeachers.Size = new Size(800, 400);
            dgvTeachers.TabIndex = 0;
            // 
            // FormListTeachers
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 650);
            Controls.Add(dgvTeachers);
            Name = "FormListTeachers";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Список учителей";
            ((System.ComponentModel.ISupportInitialize)dgvTeachers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvTeachers;
    }
}