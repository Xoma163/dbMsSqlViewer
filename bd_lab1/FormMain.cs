using bd_lab1.db;
using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace bd_lab1
{
    public partial class FormMain : Form
    {
        #region init
        private dbWorker db;
        private List<string> tableList = new List<string>();
        public FormMain()
        {
            InitializeComponent();

            dbInit();
            dbTablesInit();
        }
        //Инициализация Баз данных
        private void dbInit()
        {
            db = new dbWorker("master");
            List<string> databases = db.getDatabases();
            comboBoxDatabases.Items.Clear();
            for (int i = 0; i < databases.Count; i++)
            {
                comboBoxDatabases.Items.Add(databases[i]);
            }
            comboBoxDatabases.SelectedValue = comboBoxDatabases.Items[0];
            comboBoxDatabases.SelectedIndex = 0;

        }
        //При переопределении БД и инициализации
        private void dbTablesInit()
        {
            db = new dbWorker(comboBoxDatabases.Text);
            tableList.Clear();
            tableList = db.getTables();
            comboBoxTables.Items.Clear();
            for (int i = 0; i < tableList.Count; i++)
            {
                comboBoxTables.Items.Add(tableList[i]);
            }
            comboBoxTables.SelectedValue = comboBoxTables.Items[0];
            comboBoxTables.SelectedIndex = 0;

            printTable(db.Select(tableList[comboBoxTables.SelectedIndex]), db.getFields());

        }
        #endregion

        #region buttons
        private void buttonSelect_Click(object sender, EventArgs e)
        {
            FormDeleteAndSelect fs = new FormDeleteAndSelect(db.getFields(), tableList[comboBoxTables.SelectedIndex],"Select");
            fs.StartPosition = FormStartPosition.CenterParent;
            fs.ShowDialog();

            string result = "";
            string query = fs.getQuery();
            List<List<string>> table = new List<List<string>>();

            if (fs.getQuery() != "")
            {
                table = db.Select(tableList[comboBoxTables.SelectedIndex], query);
            }
            fs.Dispose();

            printTable(table, db.getFields());

        }
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            FormUpdate fu = new FormUpdate(db.getFields(), tableList[comboBoxTables.SelectedIndex]);
            openFormDoTheQuerySelect(fu);
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            FormDeleteAndSelect fd = new FormDeleteAndSelect(db.getFields(), tableList[comboBoxTables.SelectedIndex],"Delete");
            openFormDoTheQuerySelect(fd);
        }
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            FormInsert fi = new FormInsert(db.getFields(), tableList[comboBoxTables.SelectedIndex]);
            openFormDoTheQuerySelect(fi);
        }
        private void buttonView_Click(object sender, EventArgs e)
        {
            List<List<string>> table = new List<List<string>>();
            table = db.Select(tableList[comboBoxTables.SelectedIndex], "Select * from myView");
            printTable(table, db.getFields());

        }
        private void buttonQuery_Click(object sender, EventArgs e)
        {
            FormQuery fq = new FormQuery();
            fq.StartPosition = FormStartPosition.CenterParent;
            fq.ShowDialog();

            string result = "";
            string query = fq.getQuery();
            if (query.IndexOf("SELECT") == 0)
            {
                List<List<string>> table = new List<List<string>>();
                table = db.Select(tableList[comboBoxTables.SelectedIndex], query);
                printTable(table, db.getFields());
            }
            else 
            if (query != "")
            {
                result = db.doQuery(query);
                if (result != "ok")
                    MessageBox.Show(result);
                printTable(db.Select(tableList[comboBoxTables.SelectedIndex]), db.getFields());
            }
            fq.Dispose();

        }

        private void openFormDoTheQuerySelect(Form form)
        {
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();

            string result = "";
            //Рефлексия, по другому не знаю как
            string query = form.GetType().GetMethod("getQuery").Invoke(form, null) + "";
            if (query != "")
            {
                result = db.doQuery(query);
                if (result != "ok")
                    MessageBox.Show(result);
                else
                {
                    printTable(db.Select(tableList[comboBoxTables.SelectedIndex]), db.getFields());
                }
            }
            form.Dispose();

        }
        #endregion

        #region comboBox
        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Баг: если закрываешь комбобокс любым способом, но не нормальным, вылетает -1. Фикс.
            if (comboBoxTables.SelectedIndex > comboBoxTables.Items.Count || comboBoxTables.SelectedIndex < 0)
            {
                comboBoxTables.SelectedIndex = 0;
            }
            printTable(db.Select(tableList[comboBoxTables.SelectedIndex]), db.getFields());
        }
        private void comboBoxDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Баг: если закрываешь комбобокс любым способом, но не нормальным, вылетает -1. Фикс.
            if (comboBoxDatabases.SelectedIndex <= comboBoxDatabases.Items.Count && comboBoxDatabases.SelectedIndex >= 0)
            {
                dbTablesInit();
                printTable(db.Select(tableList[comboBoxTables.SelectedIndex]), db.getFields());
            }

        }
        #endregion

        //Метод по выводу таблиц в dgv
        private void printTable(List<List<string>> table, List<string> fields)
        {
            if (table.Count > 0)
            {
                dgv.RowCount = table.Count;
                dgv.ColumnCount = table[0].Count;
            }
            else
            {
                dgv.RowCount = 1;
                dgv.ColumnCount = 1;
            }

            for (int i = 0; i < dgv.ColumnCount; i++)
                dgv.Columns[i].Name = fields[i];

            for (int i = 0; i < table.Count; i++)
            {
                List<string> row = table[i];
                for (int j = 0; j < row.Count; j++)
                {
                    dgv.Rows[i].Cells[j].Value = row[j];
                }
            }
        }
    }
}