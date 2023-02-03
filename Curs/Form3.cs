using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Curs
{
    public partial class Form3 : Form
    {
        int number { get; set; }
        public Form3()
        {
            InitializeComponent();
            new Form1().ShowDialog();
            if (!User.authorize)
            {
                this.Close();
            }
            if (!Database.Update())
                MessageBox.Show("Ошибка");
            list.DataSource = User.graf_List;
            list.DisplayMember = "name";
            list.Update();
            list.SelectedIndexChanged += list_SelectedIndexChanged;
            New_Matrix(4);
            number = 4;
        }

        private void List_Update()
        {
            list.SelectedIndexChanged -= list_SelectedIndexChanged;
            list.DataSource = null;
            Database.Update();
            list.DataSource = User.graf_List;
            list.DisplayMember = "name";
            list.Update();
            list.SelectedIndexChanged += list_SelectedIndexChanged;
        }
        private void New_Matrix(int N)
        {
            dataGridView1.Rows.Clear();  // удаление всех строк
            int count = dataGridView1.Columns.Count;
            for (int i = 0; i < count; i++)     // цикл удаления всех столбцов
            {
                dataGridView1.Columns.RemoveAt(0);
            }
            DataGridViewTextBoxColumn q = new DataGridViewTextBoxColumn();
            q.Name = "#";
            q.HeaderText = "#";
            q.SortMode = DataGridViewColumnSortMode.NotSortable;
            q.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns.Add(q);
            for (int i = 1; i <= N; i++)
            {
                DataGridViewTextBoxColumn w = new DataGridViewTextBoxColumn();
                w.Name = i.ToString();
                w.HeaderText = i.ToString();
                w.SortMode = DataGridViewColumnSortMode.NotSortable;
                w.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns.Add(w);
            }

            DataGridViewRow row1 = new DataGridViewRow();
            DataGridViewCell S1 = new DataGridViewTextBoxCell();
            DataGridViewCellStyle m = new DataGridViewCellStyle();
            m.DataSourceNullValue = 0;
            m.Font = new Font("Microsoft Sans Serif", 11);
            S1.Value = "";
            row1.Cells.Add(S1);
            row1.Cells[0].ReadOnly = true;
            row1.Cells[0].Style = m;
            for (int j = 1; j <= N; j++)
            {
                DataGridViewCell d = new DataGridViewTextBoxCell();
                d.Value = j.ToString();
                row1.Cells.Add(d);
                row1.Cells[j].ReadOnly = true;
                row1.Cells[j].Style = m;
            }
            dataGridView1.Rows.Add(row1);

            for (int i = 1; i <= N; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewCell S = new DataGridViewTextBoxCell();
                S.Value = i.ToString();
                row.Cells.Add(S);
                row.Cells[0].ReadOnly = true;
                row.Cells[0].Style = m;
                for (int j = 1; j <= N; j++)
                {
                    DataGridViewCell d = new DataGridViewTextBoxCell();

                    if (i == j)
                    {
                        d.Value = Convert.ToString(0);
                        row.Cells.Add(d);
                        row.Cells[j].ReadOnly = true;
                        row.Cells[j].Style = m;
                    }
                    else
                    {
                        d.Value = (i*j).ToString();
                        row.Cells.Add(d);
                        row.Cells[j].Style = m;
                    }
                }
                dataGridView1.Rows.Add(row);
            }
        }

        private void DataGridView1_Cell(object sender, DataGridViewCellValidatingEventArgs e)
        {
            const string disallowed = @"[^0-9]";
            var newText = Regex.Replace(e.FormattedValue.ToString(), disallowed, string.Empty);
            dataGridView1.Rows[e.RowIndex].ErrorText = "";
            if (dataGridView1.Rows[e.RowIndex].IsNewRow) return;
            if (string.CompareOrdinal(e.FormattedValue.ToString(), newText) == 0) return;
            e.Cancel = true;
            dataGridView1.Rows[e.RowIndex].ErrorText = "Некорректный символ!";
        }

        private void way_Click(object sender, EventArgs e)
        {
            if (textBox7.Text == "")
            {
                MessageBox.Show("Введите начальную вершину",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1);
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("Введите название графа",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1);
                return;
            }
            int N = number;
            int F_P = Convert.ToInt32(textBox7.Text);

            if (F_P > N || F_P < 1)
            {
                MessageBox.Show("Начальная вершина введена некорректно",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1);
                return;
            }

            string name = textBox4.Text.ToString();
            int[,] matrix = new int[N, N];
            for (int i = 1; i < N+1; i++)
                for (int j = 1; j < N + 1; j++)
                {
                    string s = dataGridView1[i, j].Value.ToString();
                    if (s == "")
                    {
                        MessageBox.Show("Путь не задан",
                                        "Ошибка",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1);
                        return;
                    }
                    try
                    {
                        matrix[j-1, i-1] = Convert.ToInt32(s);
                    }
                    catch 
                    {
                        MessageBox.Show($"Путь слишком большой,\nзначение не больше {int.MaxValue}",
                                                "Ошибка",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information,
                                                MessageBoxDefaultButton.Button1);
                        return;
                    }
                }
            if (User.graf_now == null || User.graf_now.id == 0)
            {
                User.graf_now = new Graf();
                User.graf_now.calculating(name, F_P, N, matrix);
            }
            if (User.graf_now.id >= 0) 
                User.graf_now.calculating(name, F_P, N, matrix);

            string st = "Длинна: " + User.graf_now.len + "; Путь: ";
            for (int i = 0; i < N; i++)
                st += User.graf_now.way[i].ToString() + "-";
            st += User.graf_now.way[0].ToString() + ";";
            WayV.Text = st;
        }
        
        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Выбрать элемент (Да)\nУдалить элемент (Нет)",
                                                "Ошибка",
                                                MessageBoxButtons.YesNoCancel,
                                                MessageBoxIcon.Question,
                                                MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                User.graf_now = (Graf)list.SelectedItem;

                textBox4.Text = User.graf_now.name;
                textBox2.Text = User.graf_now.N.ToString();
                textBox7.Text = (User.graf_now.first_point + 1).ToString();
                string st = "Длинна: " + User.graf_now.len + "; Путь: ";
                for (int i = 0; i < User.graf_now.N; i++)
                    st += User.graf_now.way[i].ToString() + "-";
                st += User.graf_now.way[0].ToString() + ";";
                WayV.Text = st;
                number = User.graf_now.N;
                dataGridView1.Rows.Clear();  // удаление всех строк
                int count = dataGridView1.Columns.Count;
                for (int i = 0; i < count; i++)     // цикл удаления всех столбцов
                {
                    dataGridView1.Columns.RemoveAt(0);
                }
                DataGridViewTextBoxColumn q = new DataGridViewTextBoxColumn();
                q.Name = "#";
                q.HeaderText = "#";
                q.SortMode = DataGridViewColumnSortMode.NotSortable;
                q.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns.Add(q);
                for (int i = 1; i <= User.graf_now.N; i++)
                {
                    DataGridViewTextBoxColumn w = new DataGridViewTextBoxColumn();
                    w.Name = i.ToString();
                    w.HeaderText = i.ToString();
                    w.SortMode = DataGridViewColumnSortMode.NotSortable;
                    w.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns.Add(w);
                }

                DataGridViewRow row1 = new DataGridViewRow();
                DataGridViewCell S1 = new DataGridViewTextBoxCell();
                DataGridViewCellStyle m = new DataGridViewCellStyle();
                m.DataSourceNullValue = 0;
                m.Font = new Font("Microsoft Sans Serif", 11);
                S1.Value = "";
                row1.Cells.Add(S1);
                row1.Cells[0].ReadOnly = true;
                row1.Cells[0].Style = m;
                for (int j = 1; j <= User.graf_now.N; j++)
                {
                    DataGridViewCell d = new DataGridViewTextBoxCell();
                    d.Value = j.ToString();
                    row1.Cells.Add(d);
                    row1.Cells[j].ReadOnly = true;
                    row1.Cells[j].Style = m;
                }
                dataGridView1.Rows.Add(row1);

                for (int i = 1; i <= User.graf_now.N; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    DataGridViewCell S = new DataGridViewTextBoxCell();
                    S.Value = i.ToString();
                    row.Cells.Add(S);
                    row.Cells[0].ReadOnly = true;
                    row.Cells[0].Style = m;
                    for (int j = 1; j <= User.graf_now.N; j++)
                    {
                        DataGridViewCell d = new DataGridViewTextBoxCell();

                        if (i == j)
                        {
                            d.Value = Convert.ToString(0);
                            row.Cells.Add(d);
                            row.Cells[j].ReadOnly = true;
                            row.Cells[j].Style = m;
                        }
                        else
                        {
                            d.Value = User.graf_now.matrix[j-1,i-1].ToString();
                            row.Cells.Add(d);
                            row.Cells[j].Style = m;
                        }
                    }
                    dataGridView1.Rows.Add(row);
                }
            }
            if (result == DialogResult.No)
            {
                Graf G = (Graf)list.SelectedItem;
                Database.Remove_graf(G.id);
                List_Update();
            }
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            var tb = textBox2;
            if (tb == null) return;
            var actual = tb.Text;
            var disallowed = @"[^0-9]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(tb.Text, newText) != 0)
            {
                var sstart = tb.SelectionStart;
                tb.Text = newText;
                tb.SelectionStart = sstart - 1;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int N = Convert.ToInt32(textBox2.Text);
            if (N >= 4 && N <= 20)
            {
                New_Matrix(N);
                number = N;
            }
            else
            {
                MessageBox.Show("Недопустимое колличество вершин графа");
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            var tb = textBox4;
            if (tb == null) return;
            var actual = tb.Text;
            var disallowed = @"[^0-9A-Za-z-_]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(tb.Text, newText) != 0)
            {
                var sstart = tb.SelectionStart;
                tb.Text = newText;
                tb.SelectionStart = sstart - 1;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            if(User.graf_now == null)
            {
                MessageBox.Show($"Граф не посчитан",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information,
                                  MessageBoxDefaultButton.Button1);
                return;
            }
            User.graf_now.name = textBox4.Text;
            Database.Save_Graf();
            Database.Update();
            List_Update();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            var tb = textBox7;
            if (tb == null) return;
            var actual = tb.Text;
            var disallowed = @"[^0-9]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(tb.Text, newText) != 0)
            {
                var sstart = tb.SelectionStart;
                tb.Text = newText;
                tb.SelectionStart = sstart - 1;
            }
        }

    }
}
