using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Curs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (Database.CheckUser(textBox1.Text, textBox2.Text))
            {
                User.authorize = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Проверьте логин и пароль");
            }

        }

        private void reg_Click_1(object sender, EventArgs e)
        {
            this.Visible= false;
            Form Reg = new Form2();
            Reg.ShowDialog();
            this.Visible = true;
            textBox1.Text = null;
            textBox2.Text = null;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var tb = textBox1;
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

        private void textBox2_TextChanged(object sender, EventArgs e)
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
    }
}
