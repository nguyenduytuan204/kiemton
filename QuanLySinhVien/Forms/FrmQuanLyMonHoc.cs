using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Helpers;

namespace QuanLySinhVien.Forms
{
    public class FrmQuanLyMonHoc : Form
    {
        private Panel pnlInput = null!;
        private Panel pnlAction = null!;
        private DataGridView dgvMonHoc = null!;

        private TextBox txtMaMon = null!;
        private TextBox txtTenMon = null!;
        private NumericUpDown nudSoTinChi = null!;
        private TextBox txtSearch = null!;

        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;
        private Button btnTimKiem = null!;
        private Button btnExcel = null!;
        private Button btnPdf = null!;

        public FrmQuanLyMonHoc()
        {
            InitializeComponent();
            LoadDataMonHoc();
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

            AddInputGroup(pnlInput, "Mã Môn Học:", txtMaMon = new TextBox { Width = 180 }, 15, 15);
            AddInputGroup(pnlInput, "Tên Môn Học:", txtTenMon = new TextBox { Width = 320 }, 220, 15);

            nudSoTinChi = new NumericUpDown { Width = 140, Minimum = 1, Maximum = 10, Value = 3 };
            AddInputGroup(pnlInput, "Số Tín Chỉ:", nudSoTinChi, 560, 15);

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
            btnTimKiem.Click += (s, e) => LoadDataMonHoc(txtSearch.Text.Trim());
            pnlAction.Controls.Add(btnTimKiem);

            btnExcel = CreateButton(" 📊 Excel", Color.FromArgb(16, 124, 65), Color.White, 775, 12);
            btnExcel.Size = new Size(90, 35);
            btnExcel.Click += (s, e) => ExcelExporter.ExportToExcel(dgvMonHoc, "DANH SÁCH MÔN HỌC");
            pnlAction.Controls.Add(btnExcel);

            btnPdf = CreateButton(" 📄 PDF", Color.FromArgb(220, 38, 38), Color.White, 875, 12);
            btnPdf.Size = new Size(80, 35);
            btnPdf.Click += (s, e) => PdfExporter.ExportToPdf(dgvMonHoc, "DANH SÁCH MÔN HỌC");
            pnlAction.Controls.Add(btnPdf);

            // 3. DataGridView
            dgvMonHoc = new DataGridView
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
            dgvMonHoc.CellClick += DgvMonHoc_CellClick;
            this.Controls.Add(dgvMonHoc);
            dgvMonHoc.BringToFront();
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

        private void LoadDataMonHoc(string searchKey = "")
        {
            string query = "SELECT MaMon, TenMon, SoTinChi FROM MonHoc WHERE 1=1";
            var paramList = new System.Collections.Generic.List<SqlParameter>();

            if (!string.IsNullOrEmpty(searchKey))
            {
                query += " AND (MaMon LIKE @Search OR TenMon LIKE @Search)";
                paramList.Add(new SqlParameter("@Search", "%" + searchKey + "%"));
            }

            query += " ORDER BY MaMon ASC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query, paramList.ToArray());
            dgvMonHoc.DataSource = dt;

            if (dgvMonHoc.Columns["MaMon"] != null) dgvMonHoc.Columns["MaMon"].HeaderText = "Mã Môn Học";
            if (dgvMonHoc.Columns["TenMon"] != null) dgvMonHoc.Columns["TenMon"].HeaderText = "Tên Môn Học";
            if (dgvMonHoc.Columns["SoTinChi"] != null) dgvMonHoc.Columns["SoTinChi"].HeaderText = "Số Tín Chỉ";
        }

        private void DgvMonHoc_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvMonHoc.Rows.Count)
            {
                DataGridViewRow row = dgvMonHoc.Rows[e.RowIndex];
                txtMaMon.Text = row.Cells["MaMon"].Value?.ToString() ?? "";
                txtTenMon.Text = row.Cells["TenMon"].Value?.ToString() ?? "";

                if (int.TryParse(row.Cells["SoTinChi"].Value?.ToString(), out int stc))
                    nudSoTinChi.Value = stc;
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaMon.Text.Trim()) || string.IsNullOrEmpty(txtTenMon.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã và Tên Môn Học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO MonHoc (MaMon, TenMon, SoTinChi) VALUES (@MaMon, @TenMon, @SoTinChi)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaMon", txtMaMon.Text.Trim()),
                new SqlParameter("@TenMon", txtTenMon.Text.Trim()),
                new SqlParameter("@SoTinChi", (int)nudSoTinChi.Value)
            };

            try
            {
                int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (rows > 0)
                {
                    MessageBox.Show("Thêm môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataMonHoc();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm môn học: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaMon.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn môn học cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE MonHoc SET TenMon=@TenMon, SoTinChi=@SoTinChi WHERE MaMon=@MaMon";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaMon", txtMaMon.Text.Trim()),
                new SqlParameter("@TenMon", txtTenMon.Text.Trim()),
                new SqlParameter("@SoTinChi", (int)nudSoTinChi.Value)
            };

            try
            {
                int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataMonHoc();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật môn học: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaMon.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn môn học cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa môn học {txtMaMon.Text}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string query = "DELETE FROM MonHoc WHERE MaMon = @MaMon";
                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaMon", txtMaMon.Text.Trim()) };

                try
                {
                    int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataMonHoc();
                        ClearInputs();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa môn học: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearInputs()
        {
            txtMaMon.Clear();
            txtTenMon.Clear();
            txtSearch.Clear();
            nudSoTinChi.Value = 3;
            LoadDataMonHoc();
        }
    }
}
