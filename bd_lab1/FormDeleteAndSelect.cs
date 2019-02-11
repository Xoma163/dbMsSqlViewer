using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace bd_lab1
{
    public partial class FormDeleteAndSelect : Form
    {
        private int left = 20;
        private int top = 20;
        private List<string> operations;

        //Контроллы для сохранения
        List<TextBox> textBoxes;
        List<Label> labels;
        List<ComboBox> comboBoxes;

        //Данные которые беру из конструктора
        private string tableName;
        private List<string> fields;
        private string operation;

        public FormDeleteAndSelect(List<string> fields, string tableName, string operation)
        {
            InitializeComponent();

            this.tableName = tableName;
            this.fields = fields;
            this.operation = operation;
            this.Text = operation;
            buttonDelete.Text = operation;

            textBoxes = new List<TextBox>();
            labels = new List<Label>();
            comboBoxes = new List<ComboBox>();
            operations = new List<string> { "", "=", ">", ">=", "<", "<=", "<>", "like","in","is null" };

            for (int i = 0; i < fields.Count; i++)
            {
                //Поле
                Label l = new Label
                {
                    Text = fields[i],
                    Location = new Point(left, top + i * 30),
                    Size = new Size(150, 20)
                };

                //Операция
                ComboBox cb = new ComboBox
                {
                    Text = "",
                    Location = new Point(left + 150, top + i * 30),
                    Size = new Size(50, 20)
                };

                //Значение
                TextBox tb = new TextBox
                {
                    Text = "",
                    Location = new Point(left + 250, top + i * 30),
                    Size = new Size(150, 20)
                };

                for (int j = 0; j < operations.Count; j++)
                {
                    cb.Items.Add(operations[j]);
                }

                this.Height += 30;

                textBoxes.Add(tb);
                labels.Add(l);
                comboBoxes.Add(cb);

                this.Controls.Add(l);
                this.Controls.Add(tb);
                this.Controls.Add(cb);
            }
        }

        private string query = "";
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (operation.Equals("Select"))
            {
                query = "SELECT * FROM " + tableName;
            }
            else if (operations.Equals("Delete")){
                query = "DELETE FROM " + tableName + " ";
            }

            bool flag = true;
            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (comboBoxes[i].Text != "")
                {
                    if (flag)
                    {
                        query += " WHERE ";
                        flag = false;
                    }
                    else
                    {
                        query += "AND ";
                    }
                    query += labels[i].Text + " " + comboBoxes[i].Text + " " + textBoxes[i].Text + " ";
                }
            }
            this.Close();
        }

        public string getQuery()
        {
            return query;
        }
    }
}