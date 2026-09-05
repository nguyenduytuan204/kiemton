using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyDatSan
{
    /// <summary>
    /// CÂU 2: Màn hình Đặt sân.
    /// - Khi mở form: nạp toàn bộ sân lên DataGridViewChonSan.
    /// - Double-click 1 sân => thêm dòng vào DataGridViewThongTinDatSan
    ///   (SoPhieu, MaSan, GioBD, GioKT, DonGiaThue) - có thể chỉnh sửa giờ/giá.
    /// - Nút "Lưu thông tin" => gọi Procedure lưu Phiếu đặt sân + Thông tin
    ///   đặt sân, sau đó gọi Function ThongKeDatSan(SoPhieu) để hiển thị
    ///   tổng số lượng sân và tổng tiền.
    /// - Nút "Huỷ" => huỷ thông tin đặt sân hiện tại.
    /// - Nút "Thoát" => hỏi xác nhận rồi đóng form.
    /// </summary>
    public partial class Frm_DatSan : Form
    {
        private DataTable dtThongTinDatSan;

        public Frm_DatSan()
        {
            InitializeComponent();
        }

        private void Frm_DatSan_Load(object sender, EventArgs e)
        {
            LoadChonSan();
            KhoiTaoBangThongTinDatSan();
            dtpNgayDat.Value = DateTime.Today;
            txtSoPhieu.Text = TaoSoPhieuMoi();
        }

        /// <summary>Gọi Procedure HienThiSan (không lọc) để nạp DataGridViewChonSan.</summary>
        private void LoadChonSan()
        {
            try
            {
                SqlParameter p = new SqlParameter("@TenLoaiSan", DBNull.Value);
                DataTable dt = DBConnection.ExecuteStoredProcedure("HienThiSan", p);
                dgvChonSan.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sân: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Sinh số phiếu mới tự động (Pxxx) dựa trên số phiếu lớn nhất hiện có.</summary>
        private string TaoSoPhieuMoi()
        {
            try
            {
                string sql = "SELECT ISNULL(MAX(CAST(SUBSTRING(SOPHIEU, 2, 9) AS INT)), 0) + 1 AS NextId " +
                             "FROM PHIEUDATSAN WHERE ISNUMERIC(SUBSTRING(SOPHIEU, 2, 9)) = 1";
                DataTable dt = DBConnection.ExecuteQuery(sql);
                int nextId = Convert.ToInt32(dt.Rows[0]["NextId"]);
                return "P" + nextId.ToString("D3");
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>Tạo bảng dữ liệu tạm (in-memory) chứa các dòng chi tiết đặt sân đang nhập.</summary>
        private void KhoiTaoBangThongTinDatSan()
        {
            dtThongTinDatSan = new DataTable();
            dtThongTinDatSan.Columns.Add("SOPHIEU", typeof(string));
            dtThongTinDatSan.Columns.Add("MASAN", typeof(string));
            dtThongTinDatSan.Columns.Add("TENSAN", typeof(string));
            dtThongTinDatSan.Columns.Add("GIOBD", typeof(string));
            dtThongTinDatSan.Columns.Add("GIOKT", typeof(string));
            dtThongTinDatSan.Columns.Add("DONGIATHUE", typeof(int));

            dgvThongTinDatSan.DataSource = dtThongTinDatSan;
            dgvThongTinDatSan.Columns["SOPHIEU"].ReadOnly = true;
            dgvThongTinDatSan.Columns["MASAN"].ReadOnly = true;
            dgvThongTinDatSan.Columns["TENSAN"].ReadOnly = true;

            TinhTongTruoc();
        }

        /// <summary>Double click 1 sân trên DataGridViewChonSan => thêm vào DataGridViewThongTinDatSan.</summary>
        private void dgvChonSan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (string.IsNullOrWhiteSpace(txtSoPhieu.Text))
            {
                MessageBox.Show("Vui lòng nhập Số phiếu trước khi chọn sân.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvChonSan.Rows[e.RowIndex];
            string maSan = row.Cells["MASAN"].Value.ToString();
            string tenSan = row.Cells["TENSAN"].Value.ToString();

            foreach (DataRow r in dtThongTinDatSan.Rows)
            {
                if (r["MASAN"].ToString() == maSan)
                {
                    MessageBox.Show("Sân này đã được chọn.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            DataRow newRow = dtThongTinDatSan.NewRow();
            newRow["SOPHIEU"] = txtSoPhieu.Text.Trim();
            newRow["MASAN"] = maSan;
            newRow["TENSAN"] = tenSan;
            newRow["GIOBD"] = "08:00";
            newRow["GIOKT"] = "09:00";
            newRow["DONGIATHUE"] = 0;
            dtThongTinDatSan.Rows.Add(newRow);

            TinhTongTruoc();
        }

        private void dgvThongTinDatSan_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            TinhTongTruoc();
        }

        /// <summary>Tính tạm tổng số lượng sân / tổng tiền dựa trên dữ liệu đang nhập (chưa lưu).</summary>
        private void TinhTongTruoc()
        {
            txtTongSoLuong.Text = dtThongTinDatSan.Rows.Count.ToString();

            decimal tongTien = 0;
            foreach (DataRow r in dtThongTinDatSan.Rows)
            {
                if (TimeSpan.TryParse(r["GIOBD"].ToString(), out TimeSpan gioBD) &&
                    TimeSpan.TryParse(r["GIOKT"].ToString(), out TimeSpan gioKT) &&
                    int.TryParse(r["DONGIATHUE"].ToString(), out int donGia))
                {
                    double soGio = (gioKT - gioBD).TotalHours;
                    if (soGio > 0)
                        tongTien += (decimal)soGio * donGia;
                }
            }
            txtTongTien.Text = tongTien.ToString("N0");
        }

        /// <summary>
        /// Nút "Lưu thông tin": gọi Procedure LuuPhieuDatSan + LuuThongTinDatSan,
        /// sau đó gọi Function ThongKeDatSan(SoPhieu) để hiển thị tổng số lượng
        /// sân và tổng tiền chính thức lấy từ CSDL.
        /// </summary>
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoPhieu.Text) || string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin phiếu đặt sân.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtThongTinDatSan.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sân (double-click trên danh sách Chọn sân).",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DBConnection.ExecuteNonQueryProcedure("LuuPhieuDatSan",
                    new SqlParameter("@SoPhieu", txtSoPhieu.Text.Trim()),
                    new SqlParameter("@NgayDat", dtpNgayDat.Value.Date),
                    new SqlParameter("@SoDT", (object)txtSDT.Text.Trim() ?? DBNull.Value),
                    new SqlParameter("@TenKH", txtTenKH.Text.Trim()));

                foreach (DataRow r in dtThongTinDatSan.Rows)
                {
                    if (!TimeSpan.TryParse(r["GIOBD"].ToString(), out TimeSpan gioBD) ||
                        !TimeSpan.TryParse(r["GIOKT"].ToString(), out TimeSpan gioKT))
                    {
                        MessageBox.Show("Giờ bắt đầu / giờ kết thúc không hợp lệ cho sân " + r["MASAN"],
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DBConnection.ExecuteNonQueryProcedure("LuuThongTinDatSan",
                        new SqlParameter("@SoPhieu", r["SOPHIEU"].ToString()),
                        new SqlParameter("@MaSan", r["MASAN"].ToString()),
                        new SqlParameter("@GioBD", gioBD),
                        new SqlParameter("@GioKT", gioKT),
                        new SqlParameter("@DonGiaThue", Convert.ToInt32(r["DONGIATHUE"])));
                }

                ThongKeTheoPhieu(txtSoPhieu.Text.Trim());

                MessageBox.Show("Lưu thông tin đặt sân thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadChonSan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu thông tin: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Gọi Function ThongKeDatSan(SoPhieu) và hiển thị lên txtTongSoLuong / txtTongTien.</summary>
        private void ThongKeTheoPhieu(string soPhieu)
        {
            string sql = "SELECT * FROM dbo.ThongKeDatSan(@SoPhieu)";
            SqlParameter p = new SqlParameter("@SoPhieu", soPhieu);
            DataTable dt = DBConnection.ExecuteQuery(sql, p);

            if (dt.Rows.Count > 0)
            {
                txtTongSoLuong.Text = dt.Rows[0]["SoLuongSan"].ToString();
                txtTongTien.Text = Convert.ToDecimal(dt.Rows[0]["TongTien"]).ToString("N0");
            }
        }

        /// <summary>Nút "Huỷ": gọi Procedure HuyThongTinDatSan và làm sạch màn hình.</summary>
        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn huỷ thông tin đặt sân này không?",
                "Xác nhận huỷ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                if (!string.IsNullOrWhiteSpace(txtSoPhieu.Text))
                {
                    DBConnection.ExecuteNonQueryProcedure("HuyThongTinDatSan",
                        new SqlParameter("@SoPhieu", txtSoPhieu.Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi huỷ thông tin: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dtThongTinDatSan.Rows.Clear();
            txtSDT.Clear();
            txtTenKH.Clear();
            dtpNgayDat.Value = DateTime.Today;
            txtSoPhieu.Text = TaoSoPhieuMoi();
            TinhTongTruoc();
            LoadChonSan();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đóng màn hình này không?",
                "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
