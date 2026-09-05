using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Helpers;

namespace QuanLySinhVien.Forms
{
    public class FrmQuanLySinhVien : Form
    {
        private Panel pnlInput = null!;
        private Panel pnlAction = null!;
        private DataGridView dgvSinhVien = null!;

        // Inputs
        private TextBox txtMaSV = null!;
        private TextBox txtTenSV = null!;
        private DateTimePicker dtpNgaySinh = null!;
        private ComboBox cbGioiTinh = null!;
        private TextBox txtEmail = null!;
        private TextBox txtDienThoai = null!;
        private ComboBox cbLop = null!;
        private TextBox txtSearch = null!;

        // Buttons
        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;
        private Button btnTimKiem = null!;
        private Button btnExcel = null!;
        private Button btnPdf = null!;

        public FrmQuanLySinhVien()
        {
            InitializeComponent();
            LoadComboBoxLop();
            LoadDataSinhVien();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.Padding = new Padding(15);

            // 1. Panel Input (Trên cùng)
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

            // Row 1
            AddInputGroup(pnlInput, "Mã Sinh Viên:", txtMaSV = new TextBox { Width = 160 }, 15, 15);
            AddInputGroup(pnlInput, "Họ và Tên:", txtTenSV = new TextBox { Width = 220 }, 220, 15);
            AddInputGroup(pnlInput, "Ngày Sinh:", dtpNgaySinh = new DateTimePicker { Width = 150, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" }, 480, 15);
            
            cbGioiTinh = new ComboBox { Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cbGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
            cbGioiTinh.SelectedIndex = 0;
            AddInputGroup(pnlInput, "Giới Tính:", cbGioiTinh, 670, 15);

            // Row 2
            AddInputGroup(pnlInput, "Email:", txtEmail = new TextBox { Width = 220 }, 15, 90);
            AddInputGroup(pnlInput, "Số Điện Thoại:", txtDienThoai = new TextBox { Width = 180 }, 260, 90);
            
            cbLop = new ComboBox { Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            cbLop.SelectedIndexChanged += CbLop_SelectedIndexChanged;
            AddInputGroup(pnlInput, "Lớp Học:", cbLop, 470, 90);

            // 2. Panel Action Buttons & Search (Giữa)
            pnlAction = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent
            };
            this.Controls.Add(pnlAction);

            btnThem = CreateButton("Thêm", Color.FromArgb(37, 99, 235), Color.White, 0, 12);
            btnThem.Click += BtnThem_Click;
            pnlAction.Controls.Add(btnThem);

            btnSua = CreateButton("Sửa", Color.FromArgb(234, 179, 8), Color.Black, 100, 12);
            btnSua.Click += BtnSua_Click;
            pnlAction.Controls.Add(btnSua);

            btnXoa = CreateButton("Xóa", Color.FromArgb(239, 68, 68), Color.White, 200, 12);
            btnXoa.Click += BtnXoa_Click;
            pnlAction.Controls.Add(btnXoa);

            btnLamMoi = CreateButton("Làm Mới", Color.FromArgb(100, 116, 139), Color.White, 300, 12);
            btnLamMoi.Click += (s, e) => ClearInputs();
            pnlAction.Controls.Add(btnLamMoi);

            // Search Group (Phải)
            Label lblSearch = new Label
            {
                Text = "Tìm kiếm:",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(430, 20),
                AutoSize = true
            };
            pnlAction.Controls.Add(lblSearch);

            txtSearch = new TextBox { Width = 180, Location = new Point(505, 17), Font = new Font("Segoe UI", 10) };
            pnlAction.Controls.Add(txtSearch);

            btnTimKiem = CreateButton("Tìm", Color.FromArgb(16, 185, 129), Color.White, 695, 12);
            btnTimKiem.Size = new Size(70, 35);
            btnTimKiem.Click += (s, e) => LoadDataSinhVien(txtSearch.Text.Trim(), cbLop.SelectedValue?.ToString());
            pnlAction.Controls.Add(btnTimKiem);

            btnExcel = CreateButton(" 📊 Excel", Color.FromArgb(16, 124, 65), Color.White, 775, 12);
            btnExcel.Size = new Size(90, 35);
            btnExcel.Click += (s, e) => ExcelExporter.ExportToExcel(dgvSinhVien, "DANH SÁCH SINH VIÊN");
            pnlAction.Controls.Add(btnExcel);

            btnPdf = CreateButton(" 📄 PDF", Color.FromArgb(220, 38, 38), Color.White, 875, 12);
            btnPdf.Size = new Size(80, 35);
            btnPdf.Click += (s, e) => PdfExporter.ExportToPdf(dgvSinhVien, "DANH SÁCH SINH VIÊN");
            pnlAction.Controls.Add(btnPdf);

            // 3. DataGridView (Dưới)
            dgvSinhVien = new DataGridView
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
            dgvSinhVien.CellClick += DgvSinhVien_CellClick;
            this.Controls.Add(dgvSinhVien);
            dgvSinhVien.BringToFront();
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

        private bool _dangNapDuLieuLop = false;

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

            // Thêm dòng "-- Tất cả các lớp --" ở đầu để có thể xem toàn bộ sinh viên
            DataRow rowAll = dt.NewRow();
            rowAll["MaLop"] = DBNull.Value;
            rowAll["TenLop"] = "-- Tất cả các lớp --";
            dt.Rows.InsertAt(rowAll, 0);

            _dangNapDuLieuLop = true;
            cbLop.DataSource = dt;
            cbLop.DisplayMember = "TenLop";
            cbLop.ValueMember = "MaLop";
            cbLop.SelectedIndex = 0;
            _dangNapDuLieuLop = false;
        }

        private void CbLop_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Tránh load lại nhiều lần khi ComboBox đang được nạp dữ liệu ban đầu
            if (_dangNapDuLieuLop) return;

            string? maLop = cbLop.SelectedValue?.ToString();
            LoadDataSinhVien(txtSearch.Text.Trim(), string.IsNullOrEmpty(maLop) ? null : maLop);
        }

        private void LoadDataSinhVien(string searchKey = "", string? maLopFilter = null)
        {
            string query = @"
                SELECT SV.MaSV, SV.TenSV, SV.NgaySinh, SV.GioiTinh, SV.Email, SV.SoDienThoai, SV.MaLop, L.TenLop 
                FROM SinhVien SV
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
                query += " AND SV.MaLop = @MaLop";
                paramList.Add(new SqlParameter("@MaLop", maLopFilter));
            }

            if (!string.IsNullOrEmpty(searchKey))
            {
                query += " AND (SV.MaSV LIKE @Search OR SV.TenSV LIKE @Search OR L.TenLop LIKE @Search)";
                paramList.Add(new SqlParameter("@Search", "%" + searchKey + "%"));
            }

            query += " ORDER BY SV.MaSV ASC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query, paramList.ToArray());
            dgvSinhVien.DataSource = dt;

            if (dgvSinhVien.Columns["MaSV"] != null) dgvSinhVien.Columns["MaSV"].HeaderText = "Mã SV";
            if (dgvSinhVien.Columns["TenSV"] != null) dgvSinhVien.Columns["TenSV"].HeaderText = "Họ và Tên";
            if (dgvSinhVien.Columns["NgaySinh"] != null)
            {
                dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                dgvSinhVien.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            if (dgvSinhVien.Columns["GioiTinh"] != null) dgvSinhVien.Columns["GioiTinh"].HeaderText = "Giới Tính";
            if (dgvSinhVien.Columns["Email"] != null) dgvSinhVien.Columns["Email"].HeaderText = "Email";
            if (dgvSinhVien.Columns["SoDienThoai"] != null) dgvSinhVien.Columns["SoDienThoai"].HeaderText = "Điện Thoại";
            if (dgvSinhVien.Columns["MaLop"] != null) dgvSinhVien.Columns["MaLop"].Visible = false;
            if (dgvSinhVien.Columns["TenLop"] != null) dgvSinhVien.Columns["TenLop"].HeaderText = "Lớp Học";
        }

        private void DgvSinhVien_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSinhVien.Rows.Count)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                txtMaSV.Text = row.Cells["MaSV"].Value?.ToString() ?? "";
                txtTenSV.Text = row.Cells["TenSV"].Value?.ToString() ?? "";

                if (DateTime.TryParse(row.Cells["NgaySinh"].Value?.ToString(), out DateTime ns))
                    dtpNgaySinh.Value = ns;

                cbGioiTinh.SelectedItem = row.Cells["GioiTinh"].Value?.ToString() ?? "Nam";
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
                txtDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString() ?? "";
                cbLop.SelectedValue = row.Cells["MaLop"].Value?.ToString() ?? "";
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            // Gọi Stored Procedure sp_ThemSinhVien
            string query = "EXEC sp_ThemSinhVien @MaSV, @TenSV, @NgaySinh, @GioiTinh, @Email, @SoDienThoai, @MaLop";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSV", txtMaSV.Text.Trim()),
                new SqlParameter("@TenSV", txtTenSV.Text.Trim()),
                new SqlParameter("@NgaySinh", dtpNgaySinh.Value),
                new SqlParameter("@GioiTinh", cbGioiTinh.SelectedItem?.ToString() ?? "Nam"),
                new SqlParameter("@Email", string.IsNullOrEmpty(txtEmail.Text.Trim()) ? DBNull.Value : txtEmail.Text.Trim()),
                new SqlParameter("@SoDienThoai", string.IsNullOrEmpty(txtDienThoai.Text.Trim()) ? DBNull.Value : txtDienThoai.Text.Trim()),
                new SqlParameter("@MaLop", cbLop.SelectedValue?.ToString() ?? "")
            };

            try
            {
                int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Thêm sinh viên qua Stored Procedure thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataSinhVien();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSV.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE SinhVien SET TenSV=@TenSV, NgaySinh=@NgaySinh, GioiTinh=@GioiTinh, Email=@Email, SoDienThoai=@SoDienThoai, MaLop=@MaLop WHERE MaSV=@MaSV";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSV", txtMaSV.Text.Trim()),
                new SqlParameter("@TenSV", txtTenSV.Text.Trim()),
                new SqlParameter("@NgaySinh", dtpNgaySinh.Value),
                new SqlParameter("@GioiTinh", cbGioiTinh.SelectedItem?.ToString() ?? "Nam"),
                new SqlParameter("@Email", txtEmail.Text.Trim()),
                new SqlParameter("@SoDienThoai", txtDienThoai.Text.Trim()),
                new SqlParameter("@MaLop", cbLop.SelectedValue?.ToString() ?? "")
            };

            try
            {
                int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataSinhVien();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSV.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên {txtMaSV.Text} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string query = "DELETE FROM SinhVien WHERE MaSV = @MaSV";
                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaSV", txtMaSV.Text.Trim()) };

                try
                {
                    int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataSinhVien();
                        ClearInputs();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtMaSV.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập Mã Sinh Viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSV.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtTenSV.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập Họ và Tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSV.Focus();
                return false;
            }
            if (cbLop.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Lớp Học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ClearInputs()
        {
            txtMaSV.Clear();
            txtTenSV.Clear();
            txtEmail.Clear();
            txtDienThoai.Clear();
            txtSearch.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            cbGioiTinh.SelectedIndex = 0;
            if (cbLop.Items.Count > 0) cbLop.SelectedIndex = 0;
            LoadDataSinhVien();
        }
    }
}