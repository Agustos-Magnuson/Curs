using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace Curs
{
   
    internal static class User
    {
        public static Graf graf_now { get; set; }
        public static List <Graf> graf_List = new List <Graf> ();
        public static bool authorize { get; set; }
        public static int id { get; set; }

        static public string Get_Hesh(string str)
        {
            byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(str);
            byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++)
                sOutput.Append(tmpHash[i].ToString("X2"));
            return sOutput.ToString();
        }
           
    }

    internal class Graf
    {
        public string name { get; set; }
        public int id { get; set; }
        public int first_point { get; set; }
        public int N { get; set; }
        public int[,] matrix { get; set; }
        public int[] way { get; set; }
        public int len { get; set; }

        public Graf(string nam, int I, int F_P, int n, int[,] m, int[] w, int L)
        {
            name = nam;
            id = I;
            first_point = F_P-1;
            N = n;
            matrix = m;
            way = w;
            len = L;
        }

        public Graf()
        {
            id = 0;
        }

        public void calculating(string nam, int F_P, int n, int[,] m)
        {
            name = nam;
            first_point = F_P - 1;
            N = n;
            matrix = m;
            way = new int[N];
            way[0] = first_point + 1;
            bool[] node = new bool[N];
            node[first_point] = true;


            int node_min_way = int.MaxValue;
            int k = 0;
            int now = 0;
            for (int i = 1; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    if (!node[j] && node_min_way > matrix[now, j])
                    {
                        k = j;
                        node_min_way = matrix[now, j];
                    }
                way[i] = k + 1;
                len += node_min_way;
                node[k] = true;
                node_min_way = int.MaxValue;
                now = k;
            }
            len += matrix[way[N - 1] - 1, first_point];
        }

    }
}
