using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Curs
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            User.authorize = false;
            string currentDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);
            if (!File.Exists($"{currentDirectoryPath}\\Graf.db"))
                Database.InitializeDatabase();
            
            Form Main = new Form3();
            try
            {
                Application.Run(Main);
            }
            catch(System.ObjectDisposedException)
            {
                Application.Exit();
            }
        }
    }
}
