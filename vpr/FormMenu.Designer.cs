namespace vpr
{
    partial class FormMenu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnTeachers = new Button();
            btnStudents = new Button();
            btnProtocols = new Button();
            btnExit = new Button();
            SuspendLayout();
            // 
            // btnTeachers
            // 
            btnTeachers.Font = new Font("Times New Roman", 15.75F);
            btnTeachers.Location = new Point(124, 40);
            btnTeachers.Name = "btnTeachers";
            btnTeachers.Size = new Size(175, 52);
            btnTeachers.TabIndex = 0;
            btnTeachers.Text = "Учителя";
            btnTeachers.UseVisualStyleBackColor = true;
            btnTeachers.Click += btnTeachers_Click;
            // 
            // btnStudents
            // 
            btnStudents.Font = new Font("Times New Roman", 15.75F);
            btnStudents.Location = new Point(124, 113);
            btnStudents.Name = "btnStudents";
            btnStudents.Size = new Size(175, 52);
            btnStudents.TabIndex = 1;
            btnStudents.Text = "Обучающиеся";
            btnStudents.UseVisualStyleBackColor = true;
            btnStudents.Click += btnStudents_Click;
            // 
            // btnProtocols
            // 
            btnProtocols.Font = new Font("Times New Roman", 15.75F);
            btnProtocols.Location = new Point(124, 186);
            btnProtocols.Name = "btnProtocols";
            btnProtocols.Size = new Size(175, 52);
            btnProtocols.TabIndex = 2;
            btnProtocols.Text = "Протоколы";
            btnProtocols.UseVisualStyleBackColor = true;
            btnProtocols.Click += btnProtocols_Click;
            // 
            // btnExit
            // 
            btnExit.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnExit.Location = new Point(299, 297);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(111, 42);
            btnExit.TabIndex = 3;
            btnExit.Text = "ВЫХОД";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // FormMenu
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(422, 351);
            Controls.Add(btnExit);
            Controls.Add(btnProtocols);
            Controls.Add(btnStudents);
            Controls.Add(btnTeachers);
            Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(4);
            Name = "FormMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Сводка результатов ВПР";
            ResumeLayout(false);
        }

        #endregion

        private Button btnTeachers;
        private Button btnStudents;
        private Button btnProtocols;
        private Button btnExit;
    }
}
