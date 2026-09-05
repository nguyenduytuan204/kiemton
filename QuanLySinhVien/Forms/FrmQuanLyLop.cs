using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Helpers;

namespace QuanLySinhVien.Forms
{
    public class FrmQuanLyLop : Form
    {
        private Panel pnlInput = null!;
        private Panel pnlAction = null!;
        private DataGridView dgvLop = null!;

        private TextBox txtMaLop = null!;
        private TextBox txtTenLop = null!;
        private TextBox txtSiSo = null!;
        private ComboBox cbGiangVien = null!;
        private TextBox txtSearch = null!;

        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;
        private Button btnTimKiem = null!;
        private Button btnExcel = null!;
        private Button btnPdf = null!;

        public FrmQuanLyLop()
        {
            InitializeComponent();
            LoadComboBoxGiangVien();
            LoadDataLop();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.Padding = new Padding(15);

            // 1. Input Panel
            pnlInput = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.White,
                Padding = new Padding(15)
            };
            pnlInput.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, pnlInput.ClientRectangle, Color.FromArgb(226, 232, 240), ButtonBorderStyle.Solid);
            };
            this.Controls.Add(pnlInput);

            AddInputGroup(pnlInput, "Mã Lớp:", txtMaLop = new TextBox { Width = 150 }, 15, 15);
            AddInputGroup(pnlInput, "Tên Lớp Học:", txtTenLop = new TextBox { Width = 260 }, 185, 15);
            
            txtSiSo = new TextBox { Width = 120, ReadOnly = true, Text = "0", BackColor = Color.FromArgb(243, 244, 246) };
            AddInputGroup(pnlInput, "Sĩ Số (Tự động):", txtSiSo, 465, 15);

            cbGiangVien = new ComboBox { Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            AddInputGroup(pnlInput, "Giảng Viên Cố Vấn:", cbGiangVien, 605, 15);

            // 2. Action Panel
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

            // Search
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
            btnTimKiem.Click += (s, e) => LoadDataLop(txtSearch.Text.Trim());
            pnlAction.Controls.Add(btnTimKiem);

            btnExcel = CreateButton(" 📊 Excel", Color.FromArgb(16, 124, 65), Color.White, 775, 12);
            btnExcel.Size = new Size(90, 35);
            btnExcel.Click += (s, e) => ExcelExporter.ExportToExcel(dgvLop, "DANH SÁCH LỚP HỌC");
            pnlAction.Controls.Add(btnExcel);

            btnPdf = CreateButton(" 📄 PDF", Color.FromArgb(220, 38, 38), Color.White, 875, 12);
            btnPdf.Size = new Size(80, 35);
            btnPdf.Click += (s, e) => PdfExporter.ExportToPdf(dgvLop, "DANH SÁCH LỚP HỌC");
            pnlAction.Controls.Add(btnPdf);

            // 3. DataGridView
            dgvLop = new DataGridView
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
            dgvLop.CellClick += DgvLop_CellClick;
            this.Controls.Add(dgvLop);
            dgvLop.BringToFront();
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

        private void LoadComboBoxGiangVien()
        {
            string query = "SELECT MaGV, TenGV FROM GiangVien";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            cbGiangVien.DataSource = dt;
            cbGiangVien.DisplayMember = "TenGV";
            cbGiangVien.ValueMember = "MaGV";
        }

        private void LoadDataLop(string searchKey = "")
        {
            string query = @"
                SELECT L.MaLop, L.TenLop, L.SiSo, L.MaGV, GV.TenGV 
                FROM LopHoc L
                LEFT JOIN GiangVien GV ON L.MaGV = GV.MaGV
                WHERE 1=1";

            var paramList = new System.Collections.Generic.List<SqlParameter>();

            if (!UserSession.IsAdmin)
            {
                query += " AND L.MaGV = @MaGV";
                paramList.Add(new SqlParameter("@MaGV", UserSession.MaGV));
            }

            if (!string.IsNullOrEmpty(searchKey))
            {
                query += " AND (L.MaLop LIKE @Search OR L.TenLop LIKE @Search OR GV.TenGV LIKE @Search)";
                paramList.Add(new SqlParameter("@Search", "%" + searchKey + "%"));
            }

            query += " ORDER BY L.MaLop ASC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query, paramList.ToArray());
            dgvLop.DataSource = dt;

            if (dgvLop.Columns["MaLop"] != null) dgvLop.Columns["MaLop"].HeaderText = "Mã Lớp";
            if (dgvLop.Columns["TenLop"] != null) dgvLop.Columns["TenLop"].HeaderText = "Tên Lớp Học";
            if (dgvLop.Columns["SiSo"] != null) dgvLop.Columns["SiSo"].HeaderText = "Sĩ Số";
            if (dgvLop.Columns["MaGV"] != null) dgvLop.Columns["MaGV"].Visible = false;
            if (dgvLop.Columns["TenGV"] != null) dgvLop.Columns["TenGV"].HeaderText = "Giảng Viên Cố Vấn";
        }

        private void DgvLop_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvLop.Rows.Count)
            {
                DataGridViewRow row = dgvLop.Rows[e.RowIndex];
                txtMaLop.Text = row.Cells["MaLop"].Value?.ToString() ?? "";
                txtTenLop.Text = row.Cells["TenLop"].Value?.ToString() ?? "";
                txtSiSo.Text = row.Cells["SiSo"].Value?.ToString() ?? "0";
                cbGiangVien.SelectedValue = row.Cells["MaGV"].Value?.ToString() ?? "";
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text.Trim()) || string.IsNullOrEmpty(txtTenLop.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã Lớp và Tên Lớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO LopHoc (MaLop, TenLop, MaGV) VALUES (@MaLop, @TenLop, @MaGV)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaLop", txtMaLop.Text.Trim()),
                new SqlParameter("@TenLop", txtTenLop.Text.Trim()),
                new SqlParameter("@MaGV", cbGiangVien.SelectedValue?.ToString() ?? (object)DBNull.Value)
            };

            try
            {
                int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (rows > 0)
                {
                    MessageBox.Show("Thêm lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataLop();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm lớp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn lớp học cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE LopHoc SET TenLop=@TenLop, MaGV=@MaGV WHERE MaLop=@MaLop";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaLop", txtMaLop.Text.Trim()),
                new SqlParameter("@TenLop", txtTenLop.Text.Trim()),
                new SqlParameter("@MaGV", cbGiangVien.SelectedValue?.ToString() ?? (object)DBNull.Value)
            };

            try
            {
                int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật thông tin lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataLop();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật lớp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn lớp học cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa lớp {txtMaLop.Text}? Việc này sẽ xóa toàn bộ sinh viên và điểm số thuộc lớp này!", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                string query = "DELETE FROM LopHoc WHERE MaLop = @MaLop";
                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaLop", txtMaLop.Text.Trim()) };

                try
                {
                    int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataLop();
                        ClearInputs();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa lớp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearInputs()
        {
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtSiSo.Text = "0";
            txtSearch.Clear();
            if (cbGiangVien.Items.Count > 0) cbGiangVien.SelectedIndex = 0;
            LoadDataLop();
        }
    }
}
