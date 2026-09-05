using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyDatSan
{
    /// <summary>
    /// CÂU 1: Màn hình Xem thông tin sân.
    /// - Khi mở form: hiển thị tất cả sân lên DataGridView + gọi Function
    ///   ThongKeSan để hiển thị số sân trống / đã thuê.
    /// - Khi chọn CheckBox loại sân: gọi Procedure HienThiSan(tenLoaiSan)
    ///   để lọc dữ liệu, và gọi lại Function ThongKeSan tương ứng.
    /// </summary>
    public partial class Frm_XemThongTinSan : Form
    {
        public Frm_XemThongTinSan()
        {
            InitializeComponent();
        }

        private void Frm_XemThongTinSan_Load(object sender, EventArgs e)
        {
            LoadDataSan(null);
            LoadThongKeSan(null);
        }

        /// <summary>Gọi Procedure HienThiSan để nạp dữ liệu sân lên DataGridView.</summary>
        private void LoadDataSan(string tenLoaiSan)
        {
            try
            {
                SqlParameter p = new SqlParameter("@TenLoaiSan", (object)tenLoaiSan ?? DBNull.Value);
                DataTable dt = DBConnection.ExecuteStoredProcedure("HienThiSan", p);
                dgvSan.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sân: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Gọi Function ThongKeSan để hiển thị số sân trống / đã thuê.</summary>
        private void LoadThongKeSan(string tenLoaiSan)
        {
            try
            {
                string sql = "SELECT * FROM dbo.ThongKeSan(@TenLoaiSan)";
                SqlParameter p = new SqlParameter("@TenLoaiSan", (object)tenLoaiSan ?? DBNull.Value);
                DataTable dt = DBConnection.ExecuteQuery(sql, p);

                if (dt.Rows.Count > 0)
                {
                    object soTrong = dt.Rows[0]["SoSanTrong"];
                    object soDaThue = dt.Rows[0]["SoSanDaThue"];
                    txtSanTrong.Text = (soTrong == DBNull.Value) ? "0" : soTrong.ToString();
                    txtSanDaThue.Text = (soDaThue == DBNull.Value) ? "0" : soDaThue.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thống kê sân: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý khi người dùng chọn 1 CheckBox loại sân.
        /// Chỉ cho phép chọn 1 loại tại một thời điểm (giống radio button)
        /// vì Procedure HienThiSan chỉ nhận 1 tên loại sân làm tham số.
        /// </summary>
        private void ChonLoaiSan_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkChon = sender as CheckBox;
            if (chkChon == null || !chkChon.Checked) return;

            // Bỏ chọn các checkbox còn lại (tránh gọi lại sự kiện nhiều lần)
            foreach (Control c in grpChonLoaiSan.Controls)
            {
                if (c is CheckBox cb && cb != chkChon)
                {
                    cb.CheckedChanged -= ChonLoaiSan_CheckedChanged;
                    cb.Checked = false;
                    cb.CheckedChanged += ChonLoaiSan_CheckedChanged;
                }
            }

            // "Tất cả" có Tag = null => hiển thị hết
            string tenLoaiSan = chkChon.Tag as string;

            LoadDataSan(tenLoaiSan);
            LoadThongKeSan(tenLoaiSan);
        }

        private void btnDatSan_Click(object sender, EventArgs e)
        {
            using (Frm_DatSan frm = new Frm_DatSan())
            {
                frm.ShowDialog();
            }

            // Nạp lại dữ liệu sau khi đặt sân (vì trạng thái sân có thể thay đổi)
            LoadDataSan(null);
            LoadThongKeSan(null);
            foreach (Control c in grpChonLoaiSan.Controls)
            {
                if (c is CheckBox cb) cb.Checked = (cb == chkTatCa);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
