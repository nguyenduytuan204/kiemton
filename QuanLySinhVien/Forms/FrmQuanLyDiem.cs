using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Helpers;

namespace QuanLySinhVien.Forms
{
    public class FrmQuanLyDiem : Form
    {
        private Panel pnlInput = null!;
        private Panel pnlAction = null!;
        private DataGridView dgvDiem = null!;

        // ComboBoxes
        private ComboBox cbLopFilter = null!;
        private ComboBox cbSinhVien = null!;
        private ComboBox cbMonHoc = null!;

        // TextBoxes
        private TextBox txtDiemCC = null!;
        private TextBox txtDiemGK = null!;
        private TextBox txtDiemCK = null!;
        private TextBox txtDiemTB = null!;
        private TextBox txtXepLoai = null!;
        private TextBox txtSearch = null!;

        // Buttons
        private Button btnLuuDiem = null!;
        private Button btnXoaDiem = null!;
        private Button btnLamMoi = null!;
        private Button btnTimKiem = null!;
        private Button btnExcel = null!;
        private Button btnPdf = null!;

        public FrmQuanLyDiem()
        {
            InitializeComponent();
            LoadComboBoxLop();
            LoadComboBoxMonHoc();
            LoadDataDiem();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.Padding = new Padding(15);

            // 1. Input Panel
            pnlInput = new Panel
            {
                Dock = DockStyle.Top,
                Height = 180,
                BackColor = Color.White,
                Padding = new Padding(15)
            };
            pnlInput.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, pnlInput.ClientRectangle, Color.FromArgb(226, 232, 240), ButtonBorderStyle.Solid);
            };
            this.Controls.Add(pnlInput);

            // Row 1: Dropdowns
            cbLopFilter = new ComboBox { Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            cbLopFilter.SelectedIndexChanged += CbLopFilter_SelectedIndexChanged;
            AddInputGroup(pnlInput, "Chọn Lớp Học:", cbLopFilter, 15, 15);

            cbSinhVien = new ComboBox { Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            AddInputGroup(pnlInput, "Chọn Sinh Viên:", cbSinhVien, 250, 15);

            cbMonHoc = new ComboBox { Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            cbMonHoc.SelectedIndexChanged += CbMonHoc_SelectedIndexChanged;
            AddInputGroup(pnlInput, "Chọn Môn Học:", cbMonHoc, 545, 15);

            // Row 2: Scores & Calculated Results
            txtDiemCC = new TextBox { Width = 110, Text = "10" };
            txtDiemCC.TextChanged += CalculateAverageGrade;
            AddInputGroup(pnlInput, "Chuyên Cần (10%):", txtDiemCC, 15, 90);

            txtDiemGK = new TextBox { Width = 110, Text = "8.0" };
            txtDiemGK.TextChanged += CalculateAverageGrade;
            AddInputGroup(pnlInput, "Giữa Kỳ (30%):", txtDiemGK, 140, 90);

            txtDiemCK = new TextBox { Width = 110, Text = "8.5" };
            txtDiemCK.TextChanged += CalculateAverageGrade;
            AddInputGroup(pnlInput, "Cuối Kỳ (60%):", txtDiemCK, 265, 90);

            txtDiemTB = new TextBox { Width = 130, ReadOnly = true, BackColor = Color.FromArgb(243, 244, 246), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            AddInputGroup(pnlInput, "Điểm TB (Hệ 10):", txtDiemTB, 400, 90);

            txtXepLoai = new TextBox { Width = 150, ReadOnly = true, BackColor = Color.FromArgb(243, 244, 246), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            AddInputGroup(pnlInput, "Xếp Loại Học Tập:", txtXepLoai, 545, 90);

            // 2. Action Panel
            pnlAction = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent
            };
            this.Controls.Add(pnlAction);

            btnLuuDiem = CreateButton("💾 Lưu Điểm", Color.FromArgb(37, 99, 235), Color.White, 0, 12);
            btnLuuDiem.Width = 110;
            btnLuuDiem.Click += BtnLuuDiem_Click;
            pnlAction.Controls.Add(btnLuuDiem);

            btnXoaDiem = CreateButton("❌ Xóa Điểm", Color.FromArgb(239, 68, 68), Color.White, 120, 12);
            btnXoaDiem.Width = 100;
            btnXoaDiem.Click += BtnXoaDiem_Click;
            pnlAction.Controls.Add(btnXoaDiem);

            btnLamMoi = CreateButton("Làm Mới", Color.FromArgb(100, 116, 139), Color.White, 230, 12);
            btnLamMoi.Click += (s, e) => ClearInputs();
            pnlAction.Controls.Add(btnLamMoi);

            // Search
            Label lblSearch = new Label
            {
                Text = "Tìm kiếm:",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(340, 20),
                AutoSize = true
            };
            pnlAction.Controls.Add(lblSearch);

            txtSearch = new TextBox { Width = 170, Location = new Point(415, 17), Font = new Font("Segoe UI", 10) };
            pnlAction.Controls.Add(txtSearch);

            btnTimKiem = CreateButton("Tìm", Color.FromArgb(16, 185, 129), Color.White, 595, 12);
            btnTimKiem.Size = new Size(60, 35);
            btnTimKiem.Click += (s, e) => LoadDataDiem(txtSearch.Text.Trim(), cbLopFilter.SelectedValue?.ToString(), cbMonHoc.SelectedValue?.ToString());
            pnlAction.Controls.Add(btnTimKiem);

            btnExcel = CreateButton(" 📊 Excel", Color.FromArgb(16, 124, 65), Color.White, 665, 12);
            btnExcel.Size = new Size(90, 35);
            btnExcel.Click += (s, e) => ExcelExporter.ExportToExcel(dgvDiem, "BẢNG ĐIỂM THÀNH PHẦN SINH VIÊN");
            pnlAction.Controls.Add(btnExcel);

            btnPdf = CreateButton(" 📄 PDF", Color.FromArgb(220, 38, 38), Color.White, 765, 12);
            btnPdf.Size = new Size(80, 35);
            btnPdf.Click += (s, e) => PdfExporter.ExportToPdf(dgvDiem, "BẢNG ĐIỂM THÀNH PHẦN SINH VIÊN");
            pnlAction.Controls.Add(btnPdf);

            // 3. DataGridView
            dgvDiem = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };
            dgvDiem.CellClick += DgvDiem_CellClick;
            this.Controls.Add(dgvDiem);
            dgvDiem.BringToFront();

            // Initial calculation
            CalculateAverageGrade(null, EventArgs.Empty);
        }

        private void AddInputGroup(Control parent, string labelText, Control inputControl, int x, int y)
        {
            Label lbl = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(x, y),
                AutoSize = true
            };
            parent.Controls.Add(lbl);

            inputControl.Font = new Font("Segoe UI", 10);
            inputControl.Location = new Point(x, y + 24);
            parent.Controls.Add(inputControl);
        }

        private Button CreateButton(string text, Color backColor, Color foreColor, int x, int y)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(90, 35),
                Location = new Point(x, y),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private bool _dangNapDuLieuCombo = false;

        private void LoadComboBoxLop()
        {
            string query = "SELECT MaLop, TenLop FROM LopHoc";
            SqlParameter[]? parameters = null;

            if (!UserSession.IsAdmin)
            {
                query += " WHERE MaGV = @MaGV";
                parameters = new SqlParameter[] { new SqlParameter("@MaGV", UserSession.MaGV) };
            }

            query += " ORDER BY TenLop ASC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            DataRow rowAll = dt.NewRow();
            rowAll["MaLop"] = DBNull.Value;
            rowAll["TenLop"] = "-- Tất cả các lớp --";
            dt.Rows.InsertAt(rowAll, 0);

            _dangNapDuLieuCombo = true;
            cbLopFilter.DataSource = dt;
            cbLopFilter.DisplayMember = "TenLop";
            cbLopFilter.ValueMember = "MaLop";
            cbLopFilter.SelectedIndex = 0;
            _dangNapDuLieuCombo = false;

            LoadComboBoxSinhVien(null);
        }

        private void CbLopFilter_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieuCombo) return;

            string? maLop = cbLopFilter.SelectedValue?.ToString();
            LoadComboBoxSinhVien(string.IsNullOrEmpty(maLop) ? null : maLop);
            LoadDataDiem(txtSearch.Text.Trim(), maLop, cbMonHoc?.SelectedValue?.ToString());
        }

        private void CbMonHoc_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieuCombo) return;

            string? maMon = cbMonHoc.SelectedValue?.ToString();
            LoadDataDiem(txtSearch.Text.Trim(), cbLopFilter?.SelectedValue?.ToString(), maMon);
        }

        private void LoadComboBoxSinhVien(string? maLop)
        {
            string query = "SELECT MaSV, (MaSV + ' - ' + TenSV) AS DisplayName FROM SinhVien WHERE 1=1";
            var paramList = new System.Collections.Generic.List<SqlParameter>();

            if (!string.IsNullOrEmpty(maLop))
            {
                query += " AND MaLop = @MaLop";
                paramList.Add(new SqlParameter("@MaLop", maLop));
            }

            query += " ORDER BY MaSV ASC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query, paramList.ToArray());
            cbSinhVien.DataSource = dt;
            cbSinhVien.DisplayMember = "DisplayName";
            cbSinhVien.ValueMember = "MaSV";
        }

        private void LoadComboBoxMonHoc()
        {
            string query = "SELECT MaMon, TenMon FROM MonHoc ORDER BY TenMon ASC";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            DataRow rowAll = dt.NewRow();
            rowAll["MaMon"] = DBNull.Value;
            rowAll["TenMon"] = "-- Tất cả các môn --";
            dt.Rows.InsertAt(rowAll, 0);

            _dangNapDuLieuCombo = true;
            cbMonHoc.DataSource = dt;
            cbMonHoc.DisplayMember = "TenMon";
            cbMonHoc.ValueMember = "MaMon";
            cbMonHoc.SelectedIndex = 0;
            _dangNapDuLieuCombo = false;
        }

        private void CalculateAverageGrade(object? sender, EventArgs e)
        {
            if (double.TryParse(txtDiemCC.Text.Trim(), out double cc) &&
                double.TryParse(txtDiemGK.Text.Trim(), out double gk) &&
                double.TryParse(txtDiemCK.Text.Trim(), out double ck))
            {
                if (cc >= 0 && cc <= 10 && gk >= 0 && gk <= 10 && ck >= 0 && ck <= 10)
                {
                    double tb = Math.Round((cc * 0.1) + (gk * 0.3) + (ck * 0.6), 2);
                    txtDiemTB.Text = tb.ToString("0.00");

                    if (tb >= 8.5) txtXepLoai.Text = "Giỏi";
                    else if (tb >= 7.0) txtXepLoai.Text = "Khá";
                    else if (tb >= 5.5) txtXepLoai.Text = "Trung bình";
                    else txtXepLoai.Text = "Yếu";
                    return;
                }
            }
            txtDiemTB.Text = "";
            txtXepLoai.Text = "";
        }

        private void LoadDataDiem(string searchKey = "", string? maLopFilter = null, string? maMonFilter = null)
        {
            string query = @"
                SELECT V.MaSV, V.TenSV, V.TenLop, V.MaMon, V.TenMon, V.DiemCC, V.DiemGK, V.DiemCK, V.DiemTB, V.XepLoai
                FROM v_BangDiemTongHop V
                INNER JOIN SinhVien SV ON V.MaSV = SV.MaSV
                INNER JOIN LopHoc L ON SV.MaLop = L.MaLop
                WHERE 1=1";

            var paramList = new System.Collections.Generic.List<SqlParameter>();

            if (!UserSession.IsAdmin)
            {
                query += " AND L.MaGV = @MaGV";
                paramList.Add(new SqlParameter("@MaGV", UserSession.MaGV));
            }

            if (!string.IsNullOrEmpty(maLopFilter))
            {
                query += " AND L.MaLop = @MaLop";
                paramList.Add(new SqlParameter("@MaLop", maLopFilter));
            }

            if (!string.IsNullOrEmpty(maMonFilter))
            {
                query += " AND V.MaMon = @MaMon";
                paramList.Add(new SqlParameter("@MaMon", maMonFilter));
            }

            if (!string.IsNullOrEmpty(searchKey))
            {
                query += " AND (V.MaSV LIKE @Search OR V.TenSV LIKE @Search OR V.TenMon LIKE @Search OR V.XepLoai LIKE @Search)";
                paramList.Add(new SqlParameter("@Search", "%" + searchKey + "%"));
            }

            query += " ORDER BY V.MaSV ASC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query, paramList.ToArray());
            dgvDiem.DataSource = dt;

            if (dgvDiem.Columns["MaSV"] != null) dgvDiem.Columns["MaSV"].HeaderText = "Mã SV";
            if (dgvDiem.Columns["TenSV"] != null) dgvDiem.Columns["TenSV"].HeaderText = "Họ và Tên";
            if (dgvDiem.Columns["TenLop"] != null) dgvDiem.Columns["TenLop"].HeaderText = "Lớp";
            if (dgvDiem.Columns["TenMon"] != null) dgvDiem.Columns["TenMon"].HeaderText = "Môn Học";
            if (dgvDiem.Columns["MaMon"] != null) dgvDiem.Columns["MaMon"].Visible = false;
            if (dgvDiem.Columns["DiemCC"] != null) dgvDiem.Columns["DiemCC"].HeaderText = "ĐCC (10%)";
            if (dgvDiem.Columns["DiemGK"] != null) dgvDiem.Columns["DiemGK"].HeaderText = "ĐGK (30%)";
            if (dgvDiem.Columns["DiemCK"] != null) dgvDiem.Columns["DiemCK"].HeaderText = "ĐCK (60%)";
            if (dgvDiem.Columns["DiemTB"] != null) dgvDiem.Columns["DiemTB"].HeaderText = "Điểm TB";
            if (dgvDiem.Columns["XepLoai"] != null) dgvDiem.Columns["XepLoai"].HeaderText = "Xếp Loại";
        }

        private void DgvDiem_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvDiem.Rows.Count)
            {
                DataGridViewRow row = dgvDiem.Rows[e.RowIndex];
                cbSinhVien.SelectedValue = row.Cells["MaSV"].Value?.ToString() ?? "";
                cbMonHoc.SelectedValue = row.Cells["MaMon"].Value?.ToString() ?? "";
                txtDiemCC.Text = row.Cells["DiemCC"].Value?.ToString() ?? "0";
                txtDiemGK.Text = row.Cells["DiemGK"].Value?.ToString() ?? "0";
                txtDiemCK.Text = row.Cells["DiemCK"].Value?.ToString() ?? "0";
                txtDiemTB.Text = row.Cells["DiemTB"].Value?.ToString() ?? "0";
                txtXepLoai.Text = row.Cells["XepLoai"].Value?.ToString() ?? "";
            }
        }

        private void BtnLuuDiem_Click(object? sender, EventArgs e)
        {
            if (cbSinhVien.SelectedValue == null || cbMonHoc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Sinh viên và Môn học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtDiemCC.Text.Trim(), out double cc) || cc < 0 || cc > 10 ||
                !double.TryParse(txtDiemGK.Text.Trim(), out double gk) || gk < 0 || gk > 10 ||
                !double.TryParse(txtDiemCK.Text.Trim(), out double ck) || ck < 0 || ck > 10)
            {
                MessageBox.Show("Điểm thành phần phải nằm trong khoảng từ 0.0 đến 10.0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSV = cbSinhVien.SelectedValue.ToString() ?? "";
            string maMon = cbMonHoc.SelectedValue.ToString() ?? "";

            try
            {
                // Lưu 3 điểm thành phần
                SaveSingleComponentScore(maSV, maMon, "Chuyên cần", cc);
                SaveSingleComponentScore(maSV, maMon, "Giữa kỳ", gk);
                SaveSingleComponentScore(maSV, maMon, "Cuối kỳ", ck);

                MessageBox.Show("Cập nhật toàn bộ điểm thành phần sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataDiem();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật điểm thành phần: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveSingleComponentScore(string maSV, string maMon, string loaiDiem, double diem)
        {
            string checkQuery = "SELECT MaDiem FROM DiemThanhPhan WHERE MaSV = @MaSV AND MaMon = @MaMon AND LoaiDiem = @LoaiDiem";
            SqlParameter[] checkParams = new SqlParameter[]
            {
                new SqlParameter("@MaSV", maSV),
                new SqlParameter("@MaMon", maMon),
                new SqlParameter("@LoaiDiem", loaiDiem)
            };

            object? existingIdObj = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

            string query;
            SqlParameter[] parameters;

            if (existingIdObj != null && existingIdObj != DBNull.Value)
            {
                query = "UPDATE DiemThanhPhan SET Diem = @Diem WHERE MaSV = @MaSV AND MaMon = @MaMon AND LoaiDiem = @LoaiDiem";
            }
            else
            {
                query = "INSERT INTO DiemThanhPhan (MaSV, MaMon, LoaiDiem, Diem) VALUES (@MaSV, @MaMon, @LoaiDiem, @Diem)";
            }

            parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSV", maSV),
                new SqlParameter("@MaMon", maMon),
                new SqlParameter("@LoaiDiem", loaiDiem),
                new SqlParameter("@Diem", diem)
            };

            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        private void BtnXoaDiem_Click(object? sender, EventArgs e)
        {
            if (cbSinhVien.SelectedValue == null || cbMonHoc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên và môn học cần xóa điểm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa tất cả điểm thành phần môn này của sinh viên?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string maSV = cbSinhVien.SelectedValue.ToString() ?? "";
                string maMon = cbMonHoc.SelectedValue.ToString() ?? "";

                string query = "DELETE FROM DiemThanhPhan WHERE MaSV = @MaSV AND MaMon = @MaMon";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSV", maSV),
                    new SqlParameter("@MaMon", maMon)
                };

                try
                {
                    int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa điểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataDiem();
                        ClearInputs();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa điểm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearInputs()
        {
            txtDiemCC.Text = "10";
            txtDiemGK.Text = "8.0";
            txtDiemCK.Text = "8.5";
            txtSearch.Clear();
            LoadDataDiem();
        }
    }
}