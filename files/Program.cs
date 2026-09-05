using System;
using System.Windows.Forms;

namespace QuanLyDatSan
{
    internal static class Program
    {
        /// <summary>
        /// Điểm bắt đầu (entry point) của ứng dụng.
        /// Mở màn hình "Xem thông tin sân" (Câu 1) đầu tiên.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Frm_XemThongTinSan());
        }
    }
}
