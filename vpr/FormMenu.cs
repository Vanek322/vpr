namespace vpr
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void btnTeachers_Click(object sender, EventArgs e)
        {
            FormListTeachers formListTeachers = new FormListTeachers();
            formListTeachers.Show();
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            FormListStudents formListStudents = new FormListStudents();
            formListStudents.Show();
        }

        private void btnProtocols_Click(object sender, EventArgs e)
        {
            FormListProtocols formListProtocols = new FormListProtocols();
            formListProtocols.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
