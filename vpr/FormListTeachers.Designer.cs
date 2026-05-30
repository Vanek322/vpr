namespace vpr
{
    partial class FormListTeachers
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
            panelBot = new Panel();
            btnExit = new Button();
            dgvTeachers = new DataGridView();
            panelBot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTeachers).BeginInit();
            SuspendLayout();
            // 
            // panelBot
            // 
            panelBot.Controls.Add(btnExit);
            panelBot.Dock = DockStyle.Bottom;
            panelBot.Location = new Point(0, 517);
            panelBot.Name = "panelBot";
            panelBot.Size = new Size(884, 44);
            panelBot.TabIndex = 1;
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
            btnExit.Click += button1_Click;
            // 
            // dgvTeachers
            // 
            dgvTeachers.AllowUserToAddRows = false;
            dgvTeachers.AllowUserToDeleteRows = false;
            dgvTeachers.AllowUserToResizeColumns = false;
            dgvTeachers.AllowUserToResizeRows = false;
            dgvTeachers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTeachers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvTeachers.BackgroundColor = Color.White;
            dgvTeachers.BorderStyle = BorderStyle.None;
            dgvTeachers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTeachers.Dock = DockStyle.Fill;
            dgvTeachers.Location = new Point(0, 0);
            dgvTeachers.MultiSelect = false;
            dgvTeachers.Name = "dgvTeachers";
            dgvTeachers.ReadOnly = true;
            dgvTeachers.RowHeadersVisible = false;
            dgvTeachers.Size = new Size(884, 517);
            dgvTeachers.TabIndex = 2;
            // 
            // FormListTeachers
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(884, 561);
            Controls.Add(dgvTeachers);
            Controls.Add(panelBot);
            Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(5, 4, 5, 4);
            Name = "FormListTeachers";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Список учителей";
            panelBot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTeachers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelBot;
        private Button btnExit;
        private DataGridView dgvTeachers;
    }
}