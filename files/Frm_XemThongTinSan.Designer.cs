using System.Windows.Forms;
using System.Drawing;

namespace QuanLyDatSan
{
    partial class Frm_XemThongTinSan
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private GroupBox grpChonLoaiSan;
        private CheckBox chkBongDa;
        private CheckBox chkCauLong;
        private CheckBox chkBongRo;
        private CheckBox chkQuanVot;
        private CheckBox chkPickleball;
        private CheckBox chkTatCa;
        private Label lblSanTrong;
        private TextBox txtSanTrong;
        private Label lblSanDaThue;
        private TextBox txtSanDaThue;
        private Button btnDatSan;
        private Button btnThoat;
        private GroupBox grpThongTinSan;
        private DataGridView dgvSan;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.grpChonLoaiSan = new GroupBox();
            this.chkBongDa = new CheckBox();
            this.chkCauLong = new CheckBox();
            this.chkBongRo = new CheckBox();
            this.chkQuanVot = new CheckBox();
            this.chkPickleball = new CheckBox();
            this.chkTatCa = new CheckBox();
            this.lblSanTrong = new Label();
            this.txtSanTrong = new TextBox();
            this.lblSanDaThue = new Label();
            this.txtSanDaThue = new TextBox();
            this.btnDatSan = new Button();
            this.btnThoat = new Button();
            this.grpThongTinSan = new GroupBox();
            this.dgvSan = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.dgvSan)).BeginInit();
            this.grpChonLoaiSan.SuspendLayout();
            this.grpThongTinSan.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Text = "XEM THÔNG TIN SÂN";
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.Red;
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTitle.Location = new Point(12, 12);
            this.lblTitle.Size = new Size(760, 40);

            // grpChonLoaiSan
            this.grpChonLoaiSan.Text = "Chọn loại sân";
            this.grpChonLoaiSan.Location = new Point(20, 65);
            this.grpChonLoaiSan.Size = new Size(400, 130);
            this.grpChonLoaiSan.Controls.Add(this.chkBongDa);
            this.grpChonLoaiSan.Controls.Add(this.chkCauLong);
            this.grpChonLoaiSan.Controls.Add(this.chkBongRo);
            this.grpChonLoaiSan.Controls.Add(this.chkQuanVot);
            this.grpChonLoaiSan.Controls.Add(this.chkPickleball);
            this.grpChonLoaiSan.Controls.Add(this.chkTatCa);

            // chkBongDa
            this.chkBongDa.Text = "Bóng đá";
            this.chkBongDa.Tag = "Bóng đá";
            this.chkBongDa.Location = new Point(25, 30);
            this.chkBongDa.Size = new Size(150, 24);
            this.chkBongDa.CheckedChanged += new System.EventHandler(this.ChonLoaiSan_CheckedChanged);

            // chkCauLong
            this.chkCauLong.Text = "Cầu lông";
            this.chkCauLong.Tag = "Cầu lông";
            this.chkCauLong.Location = new Point(25, 60);
            this.chkCauLong.Size = new Size(150, 24);
            this.chkCauLong.CheckedChanged += new System.EventHandler(this.ChonLoaiSan_CheckedChanged);

            // chkBongRo
            this.chkBongRo.Text = "Bóng rổ";
            this.chkBongRo.Tag = "Bóng rổ";
            this.chkBongRo.Location = new Point(25, 90);
            this.chkBongRo.Size = new Size(150, 24);
            this.chkBongRo.CheckedChanged += new System.EventHandler(this.ChonLoaiSan_CheckedChanged);

            // chkQuanVot
            this.chkQuanVot.Text = "Quần vợt";
            this.chkQuanVot.Tag = "Quần vợt";
            this.chkQuanVot.Location = new Point(200, 30);
            this.chkQuanVot.Size = new Size(150, 24);
            this.chkQuanVot.CheckedChanged += new System.EventHandler(this.ChonLoaiSan_CheckedChanged);

            // chkPickleball
            this.chkPickleball.Text = "PickleBall";
            this.chkPickleball.Tag = "Pickleball";
            this.chkPickleball.Location = new Point(200, 60);
            this.chkPickleball.Size = new Size(150, 24);
            this.chkPickleball.CheckedChanged += new System.EventHandler(this.ChonLoaiSan_CheckedChanged);

            // chkTatCa
            this.chkTatCa.Text = "Tất cả";
            this.chkTatCa.Tag = null;
            this.chkTatCa.Location = new Point(200, 90);
            this.chkTatCa.Size = new Size(150, 24);
            this.chkTatCa.Checked = true;
            this.chkTatCa.CheckedChanged += new System.EventHandler(this.ChonLoaiSan_CheckedChanged);

            // lblSanTrong
            this.lblSanTrong.Text = "Sân trống:";
            this.lblSanTrong.Location = new Point(450, 80);
            this.lblSanTrong.Size = new Size(100, 24);

            // txtSanTrong
            this.txtSanTrong.Location = new Point(560, 77);
            this.txtSanTrong.Size = new Size(200, 24);
            this.txtSanTrong.ReadOnly = true;

            // lblSanDaThue
            this.lblSanDaThue.Text = "Sân đã thuê:";
            this.lblSanDaThue.Location = new Point(450, 115);
            this.lblSanDaThue.Size = new Size(100, 24);

            // txtSanDaThue
            this.txtSanDaThue.Location = new Point(560, 112);
            this.txtSanDaThue.Size = new Size(200, 24);
            this.txtSanDaThue.ReadOnly = true;

            // btnDatSan
            this.btnDatSan.Text = "Đặt sân";
            this.btnDatSan.Location = new Point(450, 160);
            this.btnDatSan.Size = new Size(140, 32);
            this.btnDatSan.Click += new System.EventHandler(this.btnDatSan_Click);

            // btnThoat
            this.btnThoat.Text = "Thoát";
            this.btnThoat.Location = new Point(620, 160);
            this.btnThoat.Size = new Size(140, 32);
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            // grpThongTinSan
            this.grpThongTinSan.Text = "Thông tin sân";
            this.grpThongTinSan.Location = new Point(20, 205);
            this.grpThongTinSan.Size = new Size(760, 380);
            this.grpThongTinSan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.grpThongTinSan.Controls.Add(this.dgvSan);

            // dgvSan
            this.dgvSan.Location = new Point(15, 25);
            this.dgvSan.Size = new Size(730, 340);
            this.dgvSan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvSan.AllowUserToAddRows = false;
            this.dgvSan.AllowUserToDeleteRows = false;
            this.dgvSan.ReadOnly = true;
            this.dgvSan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Frm_XemThongTinSan
            this.ClientSize = new Size(806, 605);
            this.Text = "Frm_XemThongTinSan";
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.grpChonLoaiSan);
            this.Controls.Add(this.lblSanTrong);
            this.Controls.Add(this.txtSanTrong);
            this.Controls.Add(this.lblSanDaThue);
            this.Controls.Add(this.txtSanDaThue);
            this.Controls.Add(this.btnDatSan);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.grpThongTinSan);
            this.MinimumSize = new Size(650, 500);
            this.Load += new System.EventHandler(this.Frm_XemThongTinSan_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvSan)).EndInit();
            this.grpChonLoaiSan.ResumeLayout(false);
            this.grpThongTinSan.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
