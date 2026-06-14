namespace vpr
{
    partial class FormListProtocols
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
            dgvProtocols = new DataGridView();
            panelBot.SuspendLayout();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProtocols).BeginInit();
            SuspendLayout();
            // 
            // btnExit
            // 
            btnExit.Dock = DockStyle.Right;
            btnExit.Location = new Point(951, 0);
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
            panelBot.Size = new Size(1084, 44);
            panelBot.TabIndex = 3;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(btnImport);
            panelTop.Controls.Add(btnExport);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1084, 38);
            panelTop.TabIndex = 4;
            // 
            // btnImport
            // 
            btnImport.Dock = DockStyle.Right;
            btnImport.Location = new Point(870, 0);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(107, 38);
            btnImport.TabIndex = 2;
            btnImport.Text = "Импорт";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // btnExport
            // 
            btnExport.Dock = DockStyle.Right;
            btnExport.Location = new Point(977, 0);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(107, 38);
            btnExport.TabIndex = 1;
            btnExport.Text = "Экспорт";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // dgvProtocols
            // 
            dgvProtocols.AllowUserToAddRows = false;
            dgvProtocols.AllowUserToDeleteRows = false;
            dgvProtocols.AllowUserToResizeColumns = false;
            dgvProtocols.AllowUserToResizeRows = false;
            dgvProtocols.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProtocols.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvProtocols.BackgroundColor = Color.White;
            dgvProtocols.BorderStyle = BorderStyle.None;
            dgvProtocols.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProtocols.Dock = DockStyle.Fill;
            dgvProtocols.Location = new Point(0, 38);
            dgvProtocols.MultiSelect = false;
            dgvProtocols.Name = "dgvProtocols";
            dgvProtocols.ReadOnly = true;
            dgvProtocols.RowHeadersVisible = false;
            dgvProtocols.Size = new Size(1084, 479);
            dgvProtocols.TabIndex = 5;
            // 
            // FormListProtocols
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1084, 561);
            Controls.Add(dgvProtocols);
            Controls.Add(panelTop);
            Controls.Add(panelBot);
            Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(4);
            Name = "FormListProtocols";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Результаты ВПР";
            panelBot.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProtocols).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnExit;
        private Panel panelBot;
        private Panel panelTop;
        private DataGridView dgvProtocols;
        private Button btnImport;
        private Button btnExport;
    }
}