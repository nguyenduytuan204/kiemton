using System;
using System.Windows.Forms;
using QuanLySinhVien.Forms;

namespace QuanLySinhVien
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmLogin());
        }
    }
}