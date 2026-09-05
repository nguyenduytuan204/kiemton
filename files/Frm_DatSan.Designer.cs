using System.Windows.Forms;
using System.Drawing;

namespace QuanLyDatSan
{
    partial class Frm_DatSan
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;

        private GroupBox grpPhieu;
        private Label lblSoPhieu;
        private TextBox txtSoPhieu;
        private Label lblNgayDat;
        private DateTimePicker dtpNgayDat;
        private Label lblSDT;
        private TextBox txtSDT;
        private Label lblTenKH;
        private TextBox txtTenKH;

        private GroupBox grpChonSan;
        private DataGridView dgvChonSan;

        private GroupBox grpThongTinDatSan;
        private DataGridView dgvThongTinDatSan;

        private Label lblTongSoLuong;
        private TextBox txtTongSoLuong;
        private Label lblTongTien;
        private TextBox txtTongTien;

        private Button btnLuu;
        private Button btnHuy;
        private Button btnThoat;

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

            this.grpPhieu = new GroupBox();
            this.lblSoPhieu = new Label();
            this.txtSoPhieu = new TextBox();
            this.lblNgayDat = new Label();
            this.dtpNgayDat = new DateTimePicker();
            this.lblSDT = new Label();
            this.txtSDT = new TextBox();
            this.lblTenKH = new Label();
            this.txtTenKH = new TextBox();

            this.grpChonSan = new GroupBox();
            this.dgvChonSan = new DataGridView();

            this.grpThongTinDatSan = new GroupBox();
            this.dgvThongTinDatSan = new DataGridView();

            this.lblTongSoLuong = new Label();
            this.txtTongSoLuong = new TextBox();
            this.lblTongTien = new Label();
            this.txtTongTien = new TextBox();

            this.btnLuu = new Button();
            this.btnHuy = new Button();
            this.btnThoat = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvChonSan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongTinDatSan)).BeginInit();
            this.grpPhieu.SuspendLayout();
            this.grpChonSan.SuspendLayout();
            this.grpThongTinDatSan.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Text = "THÔNG TIN ĐẶT SÂN";
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.Red;
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTitle.Location = new Point(12, 10);
            this.lblTitle.Size = new Size(860, 36);

            // grpPhieu
            this.grpPhieu.Text = "Phiếu đặt sân";
            this.grpPhieu.Location = new Point(20, 55);
            this.grpPhieu.Size = new Size(400, 190);
            this.grpPhieu.Controls.Add(this.lblSoPhieu);
            this.grpPhieu.Controls.Add(this.txtSoPhieu);
            this.grpPhieu.Controls.Add(this.lblNgayDat);
            this.grpPhieu.Controls.Add(this.dtpNgayDat);
            this.grpPhieu.Controls.Add(this.lblSDT);
            this.grpPhieu.Controls.Add(this.txtSDT);
            this.grpPhieu.Controls.Add(this.lblTenKH);
            this.grpPhieu.Controls.Add(this.txtTenKH);

            this.lblSoPhieu.Text = "Số phiếu:";
            this.lblSoPhieu.Location = new Point(20, 35);
            this.lblSoPhieu.Size = new Size(120, 24);
            this.txtSoPhieu.Location = new Point(160, 32);
            this.txtSoPhieu.Size = new Size(200, 24);
            this.txtSoPhieu.MaxLength = 10;

            this.lblNgayDat.Text = "Ngày đặt:";
            this.lblNgayDat.Location = new Point(20, 70);
            this.lblNgayDat.Size = new Size(120, 24);
            this.dtpNgayDat.Location = new Point(160, 67);
            this.dtpNgayDat.Size = new Size(200, 24);
            this.dtpNgayDat.Format = DateTimePickerFormat.Long;

            this.lblSDT.Text = "Số điện thoại:";
            this.lblSDT.Location = new Point(20, 105);
            this.lblSDT.Size = new Size(120, 24);
            this.txtSDT.Location = new Point(160, 102);
            this.txtSDT.Size = new Size(200, 24);
            this.txtSDT.MaxLength = 10;

            this.lblTenKH.Text = "Tên khách hàng:";
            this.lblTenKH.Location = new Point(20, 140);
            this.lblTenKH.Size = new Size(120, 24);
            this.txtTenKH.Location = new Point(160, 137);
            this.txtTenKH.Size = new Size(200, 24);
            this.txtTenKH.MaxLength = 100;

            // grpChonSan
            this.grpChonSan.Text = "Chọn sân";
            this.grpChonSan.Location = new Point(440, 55);
            this.grpChonSan.Size = new Size(440, 190);
            this.grpChonSan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.grpChonSan.Controls.Add(this.dgvChonSan);

            this.dgvChonSan.Location = new Point(12, 22);
            this.dgvChonSan.Size = new Size(416, 155);
            this.dgvChonSan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvChonSan.AllowUserToAddRows = false;
            this.dgvChonSan.AllowUserToDeleteRows = false;
            this.dgvChonSan.ReadOnly = true;
            this.dgvChonSan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChonSan.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvChonSan_CellDoubleClick);

            // grpThongTinDatSan
            this.grpThongTinDatSan.Text = "Thông tin đặt sân";
            this.grpThongTinDatSan.Location = new Point(20, 255);
            this.grpThongTinDatSan.Size = new Size(600, 230);
            this.grpThongTinDatSan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.grpThongTinDatSan.Controls.Add(this.dgvThongTinDatSan);

            this.dgvThongTinDatSan.Location = new Point(12, 22);
            this.dgvThongTinDatSan.Size = new Size(576, 195);
            this.dgvThongTinDatSan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvThongTinDatSan.AllowUserToAddRows = false;
            this.dgvThongTinDatSan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThongTinDatSan.CellValueChanged += new DataGridViewCellEventHandler(this.dgvThongTinDatSan_CellValueChanged);
            this.dgvThongTinDatSan.CellEndEdit += new DataGridViewCellEventHandler(this.dgvThongTinDatSan_CellValueChanged);

            // lblTongSoLuong / txtTongSoLuong
            this.lblTongSoLuong.Text = "Tổng số lượng sân:";
            this.lblTongSoLuong.Location = new Point(640, 270);
            this.lblTongSoLuong.Size = new Size(150, 24);
            this.lblTongSoLuong.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.txtTongSoLuong.Location = new Point(640, 295);
            this.txtTongSoLuong.Size = new Size(220, 24);
            this.txtTongSoLuong.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.txtTongSoLuong.ReadOnly = true;

            // lblTongTien / txtTongTien
            this.lblTongTien.Text = "Tổng tiền thanh toán:";
            this.lblTongTien.Location = new Point(640, 335);
            this.lblTongTien.Size = new Size(150, 24);
            this.lblTongTien.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.txtTongTien.Location = new Point(640, 360);
            this.txtTongTien.Size = new Size(220, 24);
            this.txtTongTien.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.txtTongTien.ReadOnly = true;

            // btnLuu
            this.btnLuu.Text = "Lưu thông tin";
            this.btnLuu.Location = new Point(640, 410);
            this.btnLuu.Size = new Size(120, 32);
            this.btnLuu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            // btnHuy
            this.btnHuy.Text = "Huỷ";
            this.btnHuy.Location = new Point(640, 450);
            this.btnHuy.Size = new Size(120, 32);
            this.btnHuy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            // btnThoat
            this.btnThoat.Text = "Thoát";
            this.btnThoat.Location = new Point(640, 490);
            this.btnThoat.Size = new Size(120, 32);
            this.btnThoat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            // Frm_DatSan
            this.ClientSize = new Size(896, 545);
            this.Text = "Frm_DatSan";
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.grpPhieu);
            this.Controls.Add(this.grpChonSan);
            this.Controls.Add(this.grpThongTinDatSan);
            this.Controls.Add(this.lblTongSoLuong);
            this.Controls.Add(this.txtTongSoLuong);
            this.Controls.Add(this.lblTongTien);
            this.Controls.Add(this.txtTongTien);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnThoat);
            this.MinimumSize = new Size(820, 480);
            this.Load += new System.EventHandler(this.Frm_DatSan_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvChonSan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongTinDatSan)).EndInit();
            this.grpPhieu.ResumeLayout(false);
            this.grpChonSan.ResumeLayout(false);
            this.grpThongTinDatSan.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
