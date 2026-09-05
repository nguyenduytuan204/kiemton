using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Helpers;

namespace QuanLySinhVien.Forms
{
    public class FrmThongKe : Form
    {
        private Panel pnlMetrics = null!;
        private Panel pnlCharts = null!;

        private Label lblTotalStudents = null!;
        private Label lblTotalClasses = null!;
        private Label lblAvgScore = null!;
        private Label lblPassRate = null!;

        private Chart chartXepLoai = null!;
        private Chart chartDiemLop = null!;

        public FrmThongKe()
        {
            InitializeComponent();
            LoadMetrics();
            LoadChartXepLoai();
            LoadChartDiemLop();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.Padding = new Padding(15);

            // 1. Metric Cards Panel (Top)
            pnlMetrics = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = Color.Transparent
            };
            this.Controls.Add(pnlMetrics);

            // Create 4 Summary Cards
            pnlMetrics.Controls.Add(CreateMetricCard(" TỔNG SINH VIÊN", lblTotalStudents = new Label(), Color.FromArgb(37, 99, 235), 0));
            pnlMetrics.Controls.Add(CreateMetricCard(" TỔNG LỚP HỌC", lblTotalClasses = new Label(), Color.FromArgb(16, 185, 129), 230));
            pnlMetrics.Controls.Add(CreateMetricCard(" ĐIỂM TB TOÀN TRƯỜNG", lblAvgScore = new Label(), Color.FromArgb(245, 158, 11), 460));
            pnlMetrics.Controls.Add(CreateMetricCard(" TỶ LỆ ĐẠT (≥5.5)", lblPassRate = new Label(), Color.FromArgb(139, 92, 246), 690));

            // 2. Charts Container Panel (Fill)
            pnlCharts = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 15, 0, 0)
            };
            this.Controls.Add(pnlCharts);
            pnlCharts.BringToFront();

            // Chart 1: Pie Chart (Biểu đồ hình quạt Xếp Loại)
            chartXepLoai = new Chart
            {
                Dock = DockStyle.Left,
                Width = 460,
                BackColor = Color.White
            };
            ChartArea ca1 = new ChartArea("MainArea");
            ca1.BackColor = Color.White;
            chartXepLoai.ChartAreas.Add(ca1);

            Legend leg1 = new Legend("Legend1")
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                Font = new Font("Segoe UI", 9.5f)
            };
            chartXepLoai.Legends.Add(leg1);

            Title title1 = new Title("TỶ LỆ PHÂN BỐ XẾP LOẠI HỌC TẬP", Docking.Top, new Font("Segoe UI", 12, FontStyle.Bold), Color.FromArgb(15, 23, 42));
            chartXepLoai.Titles.Add(title1);
            pnlCharts.Controls.Add(chartXepLoai);

            // Chart 2: Column Chart (Biểu đồ cột Điểm TB theo Lớp)
            chartDiemLop = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            ChartArea ca2 = new ChartArea("MainArea");
            ca2.BackColor = Color.White;
            ca2.AxisX.MajorGrid.LineColor = Color.FromArgb(241, 245, 249);
            ca2.AxisY.MajorGrid.LineColor = Color.FromArgb(241, 245, 249);
            ca2.AxisY.Maximum = 10;
            ca2.AxisY.Minimum = 0;
            chartDiemLop.ChartAreas.Add(ca2);

            Title title2 = new Title("ĐIỂM TRUNG BÌNH CÁC LỚP HỌC", Docking.Top, new Font("Segoe UI", 12, FontStyle.Bold), Color.FromArgb(15, 23, 42));
            chartDiemLop.Titles.Add(title2);
            if (pnlCharts.Width == 0)
            {
                pnlCharts.Width = this.Width;
            }

            pnlCharts.Controls.Add(chartDiemLop);
            pnlCharts.Controls.Add(chartDiemLop);

           // chartDiemLop.BringToFront();
        }

        private Panel CreateMetricCard(string title, Label valueLabel, Color accentColor, int xLocation)
        {
            Panel card = new Panel
            {
                Size = new Size(215, 95),
                Location = new Point(xLocation, 0),
                BackColor = Color.White
            };
            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle, Color.FromArgb(226, 232, 240), ButtonBorderStyle.Solid);
                using (SolidBrush b = new SolidBrush(accentColor))
                {
                    e.Graphics.FillRectangle(b, 0, 0, 6, card.Height);
                }
            };

            Label lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(15, 12),
                Size = new Size(185, 20)
            };
            card.Controls.Add(lblT);

            valueLabel.Text = "0";
            valueLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            valueLabel.ForeColor = Color.FromArgb(15, 23, 42);
            valueLabel.Location = new Point(15, 38);
            valueLabel.Size = new Size(185, 40);
            card.Controls.Add(valueLabel);

            return card;
        }

        private void LoadMetrics()
        {
            try
            {
                string svScope = UserSession.IsAdmin ? "" : $" WHERE MaLop IN (SELECT MaLop FROM LopHoc WHERE MaGV = '{UserSession.MaGV}')";
                string lopScope = UserSession.IsAdmin ? "" : $" WHERE MaGV = '{UserSession.MaGV}'";
                string viewScope = UserSession.IsAdmin ? "" : $" WHERE MaSV IN (SELECT MaSV FROM SinhVien WHERE MaLop IN (SELECT MaLop FROM LopHoc WHERE MaGV = '{UserSession.MaGV}'))";

                // 1. Tổng Sinh Viên
                object? totalSV = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM SinhVien" + svScope);
                lblTotalStudents.Text = totalSV?.ToString() ?? "0";

                // 2. Tổng Lớp
                object? totalLop = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM LopHoc" + lopScope);
                lblTotalClasses.Text = totalLop?.ToString() ?? "0";

                // 3. Điểm TB
                object? avgScore = DatabaseHelper.ExecuteScalar("SELECT AVG(DiemTB) FROM v_BangDiemTongHop" + viewScope);
                if (avgScore != null && double.TryParse(avgScore.ToString(), out double avg))
                {
                    lblAvgScore.Text = Math.Round(avg, 2).ToString("0.00");
                }
                else
                {
                    lblAvgScore.Text = "0.00";
                }

                // 4. Tỷ lệ đạt
                object? totalDiemCount = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM v_BangDiemTongHop" + viewScope);
                object? passDiemCount = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM v_BangDiemTongHop " + (string.IsNullOrEmpty(viewScope) ? "WHERE DiemTB >= 5.5" : viewScope + " AND DiemTB >= 5.5"));

                int totalCount = Convert.ToInt32(totalDiemCount ?? 0);
                int passCount = Convert.ToInt32(passDiemCount ?? 0);

                if (totalCount > 0)
                {
                    double passRate = Math.Round(((double)passCount / totalCount) * 100, 1);
                    lblPassRate.Text = $"{passRate}%";
                }
                else
                {
                    lblPassRate.Text = "N/A";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error metrics: {ex.Message}");
            }
        }

        private void LoadChartXepLoai()
        {
            chartXepLoai.Series.Clear();

            Series series = new Series("XepLoaiSeries")
            {
                ChartType = SeriesChartType.Doughnut,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            string scope = UserSession.IsAdmin ? "" : $" WHERE MaSV IN (SELECT MaSV FROM SinhVien WHERE MaLop IN (SELECT MaLop FROM LopHoc WHERE MaGV = '{UserSession.MaGV}'))";
            string query = $@"
                SELECT XepLoai, COUNT(*) AS SoLuong 
                FROM v_BangDiemTongHop
                {scope}
                GROUP BY XepLoai";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                string xepLoai = row["XepLoai"].ToString() ?? "Chưa rõ";
                int count = Convert.ToInt32(row["SoLuong"]);

                int pointIdx = series.Points.AddXY(xepLoai, count);
                DataPoint point = series.Points[pointIdx];
                point.Label = $"{xepLoai}: {count}";

                if (xepLoai == "Giỏi") point.Color = Color.FromArgb(16, 185, 129);
                else if (xepLoai == "Khá") point.Color = Color.FromArgb(37, 99, 235);
                else if (xepLoai == "Trung bình") point.Color = Color.FromArgb(245, 158, 11);
                else point.Color = Color.FromArgb(239, 68, 68);
            }

            chartXepLoai.Series.Add(series);
        }

        private void LoadChartDiemLop()
        {
            chartDiemLop.Series.Clear();

            Series series = new Series("DiemLopSeries")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(37, 99, 235),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                IsValueShownAsLabel = true
            };

            string scope = UserSession.IsAdmin ? "" : $" WHERE L.MaGV = '{UserSession.MaGV}'";
            string query = $@"
                SELECT V.TenLop, AVG(V.DiemTB) AS DiemTBTB
                FROM v_BangDiemTongHop V
                INNER JOIN SinhVien SV ON V.MaSV = SV.MaSV
                INNER JOIN LopHoc L ON SV.MaLop = L.MaLop
                {scope}
                GROUP BY V.TenLop";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                string tenLop = row["TenLop"].ToString() ?? "";
                double diemTB = Convert.ToDouble(row["DiemTBTB"]);
                diemTB = Math.Round(diemTB, 2);

                int idx = series.Points.AddXY(tenLop, diemTB);
                series.Points[idx].Label = diemTB.ToString("0.00");
            }

            chartDiemLop.Series.Add(series);
        }
    }
}
