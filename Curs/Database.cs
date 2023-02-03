using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Curs
{
    public static class Database
    {
        private static readonly string dataSource = "Data Source=Graf.db;Version=3;";
       
        public static bool InitializeDatabase()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dataSource))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string sql_command = "DROP TABLE IF EXISTS users;"
                        + "CREATE TABLE users("
                        + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "login TEXT, "
                        + "password TEXT" +
                        "  );";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql_command, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        sql_command = "DROP TABLE IF EXISTS graf; "
                        + "CREATE TABLE graf("
                        + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "user_id INTEGER, " +
                        "name_graf TEXT, "
                        + "N INTEGER," +
                        "way TEXT, " +
                        "way_len INTEGER," +
                        "matrix TEXT " +
                        ");";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql_command, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool createUser(string username, string password)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dataSource))
                {
                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {
                        string comand = $"SELECT login FROM users WHERE login = '{username}'; ";
                        using (SQLiteCommand com = new SQLiteCommand(comand, conn))
                        {
                            using (SQLiteDataReader reader = com.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    string Pas = User.Get_Hesh(password);
                                    string cmm = $"INSERT INTO users (login, password) VALUES ('{username}', '{Pas}')";
                                    using (SQLiteCommand cm = new SQLiteCommand(cmm, conn))
                                    {
                                        cm.ExecuteNonQuery();
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
            
        }

        public static bool CheckUser(string username, string password)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dataSource))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string comand = $"SELECT password, id FROM users WHERE login = '{username}'; ";
                        using (SQLiteCommand com = new SQLiteCommand(comand, conn))
                        {
                            using (SQLiteDataReader reader = com.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    string Pas = User.Get_Hesh(password);
                                    string pas_db = reader.GetValue(0).ToString();
                                    int id = Convert.ToInt32(reader.GetValue(1));
                                    if (Pas.Equals(pas_db))
                                    {
                                        User.id = id;
                                        return true;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static bool Save_Graf()
        {
            if (User.graf_now == null)
            {
                MessageBox.Show($"Граф не посчитан",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information,
                                  MessageBoxDefaultButton.Button1);
                return false;
            }

            string way = "", matrix = "";
            for (int i = 0; i < User.graf_now.N; i++)
                way += User.graf_now.way[i].ToString() + ";";
            way += User.graf_now.way[0].ToString();

            for (int i = 0; i < User.graf_now.N; i++)
                for (int j = 0; j < User.graf_now.N; j++)
                    matrix += User.graf_now.matrix[j, i].ToString() + ";";
            matrix = matrix.Remove(matrix.Length - 1);

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dataSource))
                {

                    conn.Open();

                    if (User.graf_now.id == 0)
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            string comand = $"INSERT INTO graf (user_id, name_graf, N, way, way_len, matrix) VALUES " +
                                    $"({User.id}, '{User.graf_now.name}', {User.graf_now.N}, '{way}', {User.graf_now.len}, '{matrix}')";
                            using (SQLiteCommand cmd = new SQLiteCommand(comand, conn))
                            {
                                cmd.ExecuteNonQuery();
                            }

                            comand = $"SELECT id FROM graf WHERE name_graf = '{User.graf_now.name}'; ";
                            using (SQLiteCommand com = new SQLiteCommand(comand, conn))
                            {
                                using (SQLiteDataReader reader = com.ExecuteReader())
                                {
                                    while (reader.Read())
                                        User.graf_now.id = Convert.ToInt32(reader.GetValue(0));
                                    return true;
                                }
                            }
                        }
                        return false;
                    }

                    if (User.graf_now.id > 0)
                    {
                        DialogResult result = MessageBox.Show($"Желаете перезаписать граф?\n (Нет - Сохранить)",
                                          "",
                                          MessageBoxButtons.YesNoCancel,
                                          MessageBoxIcon.Question,
                                          MessageBoxDefaultButton.Button1);


                        if (result == DialogResult.No)
                        {
                            if (conn.State == ConnectionState.Open)
                            {
                                string comand = $"INSERT INTO graf (user_id, name_graf, N, way, way_len, matrix) VALUES " +
                                        $"({User.id}, '{User.graf_now.name}', {User.graf_now.N}, '{way}', {User.graf_now.len}, '{matrix}')";
                                using (SQLiteCommand cmd = new SQLiteCommand(comand, conn))
                                {
                                    cmd.ExecuteNonQuery();
                                }

                                comand = $"SELECT id FROM graf WHERE name_graf = '{User.graf_now.name}'; ";
                                using (SQLiteCommand com = new SQLiteCommand(comand, conn))
                                {
                                    using (SQLiteDataReader reader = com.ExecuteReader())
                                    {
                                        while (reader.Read())
                                            User.graf_now.id = Convert.ToInt32(reader.GetValue(0));
                                        return true;
                                    }
                                }
                            }
                            return false;
                        }

                        if (result == DialogResult.Yes)
                        {
                            if (conn.State == ConnectionState.Open)
                            {
                                string comand = $"UPDATE graf SET name_graf = '{User.graf_now.name}', " +
                                $"N = {User.graf_now.N}, way = '{way}', way_len = {User.graf_now.len}, matrix = '{matrix}' WHERE id = {User.graf_now.id}";
                                using (SQLiteCommand cmd = new SQLiteCommand(comand, conn))
                                {
                                    cmd.ExecuteNonQuery();
                                }

                                return true;
                            }
                            return false;

                        }

                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static bool Update()
        {
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string comand = $"SELECT id, name_graf, N, way, way_len, matrix FROM graf WHERE user_id = {User.id}; ";
                    using (SQLiteCommand com = new SQLiteCommand(comand, conn))
                    {
                        using (SQLiteDataReader reader = com.ExecuteReader())
                        {
                            if(User.graf_List != null)
                                User.graf_List.Clear();
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    Graf G = new Graf();
                                    G.id = Convert.ToInt32(reader.GetValue(0));
                                    G.name = reader.GetValue(1).ToString();
                                    G.N = Convert.ToInt32(reader.GetValue(2));
                                    G.len = Convert.ToInt32(reader.GetValue(4));
                                    G.way = reader.GetValue(3).ToString().Split(';').Select(x => int.Parse(x)).ToArray();
                                    G.first_point = G.way[0]-1;
                                    int[] M = reader.GetValue(5).ToString().Split(';').Select(x => int.Parse(x)).ToArray();
                                    int[,] Matrix = new int[G.N, G.N];
                                    for (int i = 0; i < G.N; i++)
                                        for (int j = 0; j < G.N; j++)
                                            Matrix[j, i] = M[i * G.N + j];
                                    G.matrix = Matrix;
                                    User.graf_List.Add(G);
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        public static void Remove_graf(int ID)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string comand = $"DELETE FROM graf WHERE id = {ID};";
                    using (SQLiteCommand com = new SQLiteCommand(comand, conn))
                    {
                        com.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}