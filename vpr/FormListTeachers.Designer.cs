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
            button1 = new Button();
            dgvTeachers = new DataGridView();
            panelBot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTeachers).BeginInit();
            SuspendLayout();
            // 
            // panelBot
            // 
            panelBot.Controls.Add(button1);
            panelBot.Dock = DockStyle.Bottom;
            panelBot.Location = new Point(0, 517);
            panelBot.Name = "panelBot";
            panelBot.Size = new Size(884, 44);
            panelBot.TabIndex = 1;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.Location = new Point(751, 0);
            button1.Name = "button1";
            button1.Size = new Size(133, 44);
            button1.TabIndex = 0;
            button1.Text = "ВЫХОД";
            button1.UseVisualStyleBackColor = true;
            // 
            // dgvTeachers
            // 
            dgvTeachers.AllowUserToAddRows = false;
            dgvTeachers.AllowUserToDeleteRows = false;
            dgvTeachers.AllowUserToResizeColumns = false;
            dgvTeachers.BackgroundColor = Color.White;
            dgvTeachers.BorderStyle = BorderStyle.None;
            dgvTeachers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTeachers.Dock = DockStyle.Fill;
            dgvTeachers.Location = new Point(0, 0);
            dgvTeachers.MultiSelect = false;
            dgvTeachers.Name = "dgvTeachers";
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
        private Button button1;
        private DataGridView dgvTeachers;
    }
}