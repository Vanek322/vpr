namespace vpr
{
    partial class FormListStudents
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnExit = new Button();
            panelBot = new Panel();
            panelTop = new Panel();
            btnImport = new Button();
            btnExport = new Button();
            dgvStudents = new DataGridView();
            panelBot.SuspendLayout();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
            SuspendLayout();
            // 
            // btnExit
            // 
            btnExit.Dock = DockStyle.Right;
            btnExit.Location = new Point(751, 0);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(133, 44);
            btnExit.TabIndex = 0;
            btnExit.Text = "ВЫХОД";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // panelBot
            // 
            panelBot.Controls.Add(btnExit);
            panelBot.Dock = DockStyle.Bottom;
            panelBot.Location = new Point(0, 517);
            panelBot.Name = "panelBot";
            panelBot.Size = new Size(884, 44);
            panelBot.TabIndex = 3;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(btnImport);
            panelTop.Controls.Add(btnExport);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(884, 38);
            panelTop.TabIndex = 5;
            // 
            // btnImport
            // 
            btnImport.Dock = DockStyle.Right;
            btnImport.Location = new Point(670, 0);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(107, 38);
            btnImport.TabIndex = 3;
            btnImport.Text = "Импорт";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // btnExport
            // 
            btnExport.Dock = DockStyle.Right;
            btnExport.Location = new Point(777, 0);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(107, 38);
            btnExport.TabIndex = 2;
            btnExport.Text = "Экспорт";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // dgvStudents
            // 
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.AllowUserToDeleteRows = false;
            dgvStudents.AllowUserToResizeColumns = false;
            dgvStudents.AllowUserToResizeRows = false;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvStudents.BackgroundColor = Color.White;
            dgvStudents.BorderStyle = BorderStyle.None;
            dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStudents.Dock = DockStyle.Fill;
            dgvStudents.Location = new Point(0, 38);
            dgvStudents.MultiSelect = false;
            dgvStudents.Name = "dgvStudents";
            dgvStudents.ReadOnly = true;
            dgvStudents.RowHeadersVisible = false;
            dgvStudents.Size = new Size(884, 479);
            dgvStudents.TabIndex = 6;
            // 
            // FormListStudents
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(884, 561);
            Controls.Add(dgvStudents);
            Controls.Add(panelTop);
            Controls.Add(panelBot);
            Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(4);
            Name = "FormListStudents";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Список студентов";
            panelBot.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnExit;
        private Panel panelBot;
        private Panel panelTop;
        private DataGridView dgvStudents;
        private Button btnImport;
        private Button btnExport;
    }
}