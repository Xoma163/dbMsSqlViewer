using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace bd_lab1
{
    public partial class FormInsert : Form
    {
        private int left = 20;
        private int top = 20;

        //Контроллы для сохранения
        List<TextBox> textBoxes;

        private string tableName;
        private List<string> fields;

        public FormInsert(List<string> fields, string tableName)
        {
            InitializeComponent();

            this.tableName = tableName;
            this.fields = fields;

            textBoxes = new List<TextBox>();

            for (int i = 0; i < fields.Count; i++)
            {
                Label l = new Label
                {
                    Text = fields[i],
                    Location = new Point(left, top + i * 30),
                    Size = new Size(150, 20)
                };
                TextBox tb = new TextBox
                {
                    Text = "",
                    Location = new Point(left + 150, top + i * 30),
                    Size = new Size(150, 20)
                };
                this.Height += 30;

                textBoxes.Add(tb);
                this.Controls.Add(l);
                this.Controls.Add(tb);
            }
        }

        private string query = "";
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            query = "INSERT INTO " + tableName + " VALUES (";

            for (int i = 0; i < fields.Count; i++)
            {
                if (i != fields.Count - 1)
                {
                    query += "'" + textBoxes[i].Text + "'" + ",";
                }
                else
                {
                    query += "'" + textBoxes[i].Text + "'";
                }
            }
            query += ")";

            this.Close();
        }

        public string getQuery()
        {
            return query;
        }
    }
}