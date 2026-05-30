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
            dgvProtocols = new DataGridView();
            panelBot = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvProtocols).BeginInit();
            panelBot.SuspendLayout();
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
            dgvProtocols.Location = new Point(0, 0);
            dgvProtocols.MultiSelect = false;
            dgvProtocols.Name = "dgvProtocols";
            dgvProtocols.ReadOnly = true;
            dgvProtocols.RowHeadersVisible = false;
            dgvProtocols.Size = new Size(1084, 517);
            dgvProtocols.TabIndex = 4;
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
            // FormListProtocols
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1084, 561);
            Controls.Add(dgvProtocols);
            Controls.Add(panelBot);
            Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(4);
            Name = "FormListProtocols";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Результаты ВПР";
            ((System.ComponentModel.ISupportInitialize)dgvProtocols).EndInit();
            panelBot.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnExit;
        private DataGridView dgvProtocols;
        private Panel panelBot;
    }
}