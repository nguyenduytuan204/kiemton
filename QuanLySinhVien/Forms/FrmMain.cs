using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLySinhVien.Helpers;

namespace QuanLySinhVien.Forms
{
    public class FrmMain : Form
    {
        private Panel pnlSidebar = null!;
        private Panel pnlHeader = null!;
        private Panel pnlContent = null!;

        private Label lblAppTitle = null!;
        private Label lblUserInfo = null!;
        private Label lblCurrentFormTitle = null!;

        private Button btnSinhVien = null!;
        private Button btnLop = null!;
        private Button btnMonHoc = null!;
        private Button btnDiem = null!;
        private Button btnThongKe = null!;
        private Button btnLogout = null!;

        private Form? activeForm = null;

        public FrmMain()
        {
            InitializeComponent();
            LoadUserInfo();
            OpenChildForm(new FrmQuanLySinhVien(), "Quản Lý Sinh Viên", btnSinhVien);
        }

        private void InitializeComponent()
        {
            this.Text = "Hệ Thống Quản Lý Sinh Viên Nâng Cao";
            this.Size = new Size(1280, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1024, 600);
            this.BackColor = Color.FromArgb(241, 245, 249);

            // 1. Sidebar Panel (Trái)
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = Color.FromArgb(15, 23, 42) // Dark Navy
            };
            this.Controls.Add(pnlSidebar);

            // App Title in Sidebar
            lblAppTitle = new Label
            {
                Text = "QL SINH VIÊN",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(56, 189, 248), // Cyan
                Size = new Size(240, 60),
                Location = new Point(0, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlSidebar.Controls.Add(lblAppTitle);

            // Navigation Buttons
            int btnY = 90;
            btnSinhVien = CreateNavButton(" 👨‍🎓  Sinh Viên", btnY);
            btnSinhVien.Click += (s, e) => OpenChildForm(new FrmQuanLySinhVien(), "Quản Lý Sinh Viên", btnSinhVien);
            pnlSidebar.Controls.Add(btnSinhVien);

            btnY += 50;
            btnLop = CreateNavButton(" 🏫  Lớp Học", btnY);
            btnLop.Click += (s, e) => OpenChildForm(new FrmQuanLyLop(), "Quản Lý Lớp Học", btnLop);
            pnlSidebar.Controls.Add(btnLop);

            btnY += 50;
            btnMonHoc = CreateNavButton(" 📚  Môn Học", btnY);
            btnMonHoc.Click += (s, e) => OpenChildForm(new FrmQuanLyMonHoc(), "Quản Lý Môn Học", btnMonHoc);
            pnlSidebar.Controls.Add(btnMonHoc);

            btnY += 50;
            btnDiem = CreateNavButton(" 📝  Điểm Thành Phần", btnY);
            btnDiem.Click += (s, e) => OpenChildForm(new FrmQuanLyDiem(), "Quản Lý Điểm Sinh Viên", btnDiem);
            pnlSidebar.Controls.Add(btnDiem);

            btnY += 50;
            btnThongKe = CreateNavButton(" 📊  Thống Kê & Biểu Đồ", btnY);
            btnThongKe.Click += (s, e) => OpenChildForm(new FrmThongKe(), "Thống Kê & Biểu Đồ Báo Cáo", btnThongKe);
            pnlSidebar.Controls.Add(btnThongKe);

            // Logout Button at bottom of sidebar
            btnLogout = new Button
            {
                Text = " 🚪  Đăng Xuất",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(248, 113, 113),
                BackColor = Color.FromArgb(30, 41, 59),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
                Location = new Point(20, 640),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += BtnLogout_Click;
            pnlSidebar.Controls.Add(btnLogout);

            // 2. Header Panel (Trên)
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.White
            };
            pnlHeader.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, pnlHeader.ClientRectangle, Color.FromArgb(226, 232, 240), ButtonBorderStyle.Solid);
            };
            this.Controls.Add(pnlHeader);

            lblCurrentFormTitle = new Label
            {
                Text = "Quản Lý Sinh Viên",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(20, 18),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblCurrentFormTitle);

            lblUserInfo = new Label
            {
                Text = "Xin chào: ...",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Size = new Size(400, 30),
                Location = new Point(pnlHeader.Width - 420, 18),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            pnlHeader.Controls.Add(lblUserInfo);

            // 3. Content Panel (Giữa)
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(241, 245, 249)
            };
            this.Controls.Add(pnlContent);
            pnlContent.BringToFront();
        }

        private Button CreateNavButton(string text, int topLocation)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(203, 213, 225),
                BackColor = Color.FromArgb(15, 23, 42),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(240, 48),
                Location = new Point(0, topLocation),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void LoadUserInfo()
        {
            lblUserInfo.Text = $"👤 {UserSession.TenGV} ({UserSession.Quyen}) | Mã GV: {UserSession.MaGV}";
        }

        private void HighlightButton(Button activeBtn)
        {
            foreach (Control ctrl in pnlSidebar.Controls)
            {
                if (ctrl is Button btn && btn != btnLogout)
                {
                    btn.BackColor = Color.FromArgb(15, 23, 42);
                    btn.ForeColor = Color.FromArgb(203, 213, 225);
                    btn.Font = new Font("Segoe UI", 11, FontStyle.Regular);
                }
            }
            activeBtn.BackColor = Color.FromArgb(30, 41, 59);
            activeBtn.ForeColor = Color.FromArgb(56, 189, 248);
            activeBtn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        }

        private void OpenChildForm(Form childForm, string title, Button navBtn)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(childForm);
            pnlContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            lblCurrentFormTitle.Text = title;
            HighlightButton(navBtn);
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                UserSession.Clear();
                this.Hide();
                FrmLogin login = new FrmLogin();
                login.Show();
            }
        }
    }
}
