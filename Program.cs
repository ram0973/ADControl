using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices;

namespace ADcontrol
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.SetCompatibleTextRenderingDefault(false);
            AppMenus.InitAppMenus();
            Application.EnableVisualStyles();
            Application.Run(new Form1());
            
        }
    }
}
