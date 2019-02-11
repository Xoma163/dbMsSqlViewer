using System;
using System.Windows.Forms;

namespace bd_lab1
{
    public partial class FormQuery : Form
    {
        public FormQuery()
        {
            InitializeComponent();
        }

        private string query = "";
        private void button1_Click(object sender, EventArgs e)
        {
            query = textBoxQuery.Text;
            this.Close();
        }

        public string getQuery()
        {
            return query;
        }


        private void textBoxQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
