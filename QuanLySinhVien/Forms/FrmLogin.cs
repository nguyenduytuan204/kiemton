using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Helpers;

namespace QuanLySinhVien.Forms
{
    public class FrmLogin : Form
    {
        private Panel pnlCard = null!;
        private Label lblTitle = null!;
        private Label lblSubtitle = null!;
        private Label lblUsername = null!;
        private TextBox txtUsername = null!;
        private Label lblPassword = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Button btnExit = null!;
        private CheckBox chkShowPassword = null!;

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Hệ Thống Quản Lý Sinh Viên - Đăng Nhập";
            this.Size = new Size(450, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 244, 248);

            // Container Card
            pnlCard = new Panel
            {
                Size = new Size(380, 400),
                Location = new Point(30, 20),
                BackColor = Color.White
            };
            pnlCard.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, pnlCard.ClientRectangle, Color.FromArgb(220, 224, 230), ButtonBorderStyle.Solid);
            };
            this.Controls.Add(pnlCard);

            // Title
            lblTitle = new Label
            {
                Text = "ĐĂNG NHẬP",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Size = new Size(340, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlCard.Controls.Add(lblTitle);

            lblSubtitle = new Label
            {
                Text = "Cổng thông tin Giảng viên & Quản trị",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 116, 139),
                Size = new Size(340, 25),
                Location = new Point(20, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlCard.Controls.Add(lblSubtitle);

            // Username
            lblUsername = new Label
            {
                Text = "Tên đăng nhập / Username:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(30, 95),
                Size = new Size(320, 22),
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            pnlCard.Controls.Add(lblUsername);

            txtUsername = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(30, 120),
                Size = new Size(320, 30),
                Text = "admin"
            };
            pnlCard.Controls.Add(txtUsername);

            // Password
            lblPassword = new Label
            {
                Text = "Mật khẩu:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(30, 160),
                Size = new Size(320, 22),
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            pnlCard.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(30, 185),
                Size = new Size(320, 30),
                PasswordChar = '●',
                Text = "123456"
            };
            pnlCard.Controls.Add(txtPassword);

            // Show password checkbox
            chkShowPassword = new CheckBox
            {
                Text = "Hiển thị mật khẩu",
                Font = new Font("Segoe UI", 9),
                Location = new Point(30, 225),
                Size = new Size(160, 25),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '●';
            };
            pnlCard.Controls.Add(chkShowPassword);

            // Buttons
            btnLogin = new Button
            {
                Text = "ĐĂNG NHẬP",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(30, 270),
                Size = new Size(320, 42),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            pnlCard.Controls.Add(btnLogin);

            btnExit = new Button
            {
                Text = "Thoát",
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(226, 232, 240),
                ForeColor = Color.FromArgb(51, 65, 85),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(30, 325),
                Size = new Size(320, 35),
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Click += (s, e) => Application.Exit();
            pnlCard.Controls.Add(btnExit);

            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "SELECT MaGV, TenGV, Username, Quyen FROM GiangVien WHERE Username = @Username AND Password = @Password";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", password)
                };

                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    UserSession.MaGV = row["MaGV"].ToString() ?? "";
                    UserSession.TenGV = row["TenGV"].ToString() ?? "";
                    UserSession.Username = row["Username"].ToString() ?? "";
                    UserSession.Quyen = row["Quyen"].ToString() ?? "";

                    MessageBox.Show($"Đăng nhập thành công!\nXin chào {UserSession.TenGV} ({UserSession.Quyen})", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    FrmMain mainForm = new FrmMain();
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối CSDL!\nChi tiết: {ex.Message}\n\nHướng dẫn: Hãy chạy tệp DatabaseSetup.sql trên SQL Server và kiểm tra chuỗi kết nối trong DatabaseHelper.cs.", "Lỗi kết nối CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
