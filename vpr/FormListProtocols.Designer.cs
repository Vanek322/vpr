namespace vpr
{
    partial class FormListProtocols
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
            dgvProtocols = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvProtocols).BeginInit();
            SuspendLayout();
            // 
            // dgvProtocols
            // 
            dgvProtocols.AllowUserToAddRows = false;
            dgvProtocols.AllowUserToDeleteRows = false;
            dgvProtocols.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProtocols.BackgroundColor = Color.White;
            dgvProtocols.BorderStyle = BorderStyle.None;
            dgvProtocols.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProtocols.Location = new Point(0, 0);
            dgvProtocols.Name = "dgvProtocols";
            dgvProtocols.ReadOnly = true;
            dgvProtocols.Size = new Size(1000, 500);
            dgvProtocols.TabIndex = 0;
            // 
            // FormListProtocols
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 700);
            Controls.Add(dgvProtocols);
            Name = "FormListProtocols";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Протоколы ВПР";
            ((System.ComponentModel.ISupportInitialize)dgvProtocols).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvProtocols;
    }
}