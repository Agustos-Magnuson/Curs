using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Curs
{
    
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") MessageBox.Show("Введите логин");
            if (textBox2.Text == "") MessageBox.Show("Введите пароль");
            if (textBox3.Text == "") MessageBox.Show("Введите пароль повторно");
            if (textBox3.Text != textBox2.Text) MessageBox.Show("Пароли не совпадают");
            if (textBox2.Text == textBox3.Text && textBox3.Text != "" && textBox2.Text != "" && textBox1.Text != "")
            {
                if (Database.createUser(textBox1.Text, textBox2.Text))
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь с таким именем уже существует");
                }
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            var tb = textBox3;
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
