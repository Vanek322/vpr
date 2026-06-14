namespace vpr
{
    partial class FormMenu
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
            btnExit = new RoundedButton();
            SuspendLayout();
            // 
            // btnExit
            // 
            btnExit.BackColor = ThemeManager.Danger;
            btnExit.ForeColor = Color.White;
            btnExit.BorderRadius = 12;
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(150, 45);
            btnExit.Text = "✕ Выход";
            btnExit.Click += btnExit_Click;
            // 
            // FormMenu
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 600);
            Name = "FormMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Сводка результатов ВПР";
            ResumeLayout(false);
        }

        #endregion

        private RoundedButton btnExit;
    }
}