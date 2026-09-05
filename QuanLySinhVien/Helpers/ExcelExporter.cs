using System;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace QuanLySinhVien.Helpers
{
    public static class ExcelExporter
    {
        public static void ExportToExcel(DataGridView dgv, string title = "BÁO CÁO DỮ LIỆU")
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
#pragma warning disable CS0618
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
#pragma warning restore CS0618
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        ExcelWorksheet ws = package.Workbook.Worksheets.Add("BaoCao");

                        // 1. Tiêu đề Báo cáo
                        int colCount = dgv.Columns.Count;
                        int visibleColCount = 0;
                        foreach (DataGridViewColumn col in dgv.Columns)
                        {
                            if (col.Visible) visibleColCount++;
                        }

                        if (visibleColCount == 0)
                        {
                            MessageBox.Show("Không có cột dữ liệu nào để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        ws.Cells[1, 1, 1, visibleColCount].Merge = true;
                        ws.Cells[1, 1].Value = title.ToUpper();
                        ws.Cells[1, 1].Style.Font.Size = 16;
                        ws.Cells[1, 1].Style.Font.Bold = true;
                        ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // 2. Tiêu đề các Cột (Header)
                        int currentColumn = 1;
                        for (int i = 0; i < colCount; i++)
                        {
                            if (dgv.Columns[i].Visible)
                            {
                                ws.Cells[3, currentColumn].Value = dgv.Columns[i].HeaderText;
                                ws.Cells[3, currentColumn].Style.Font.Bold = true;
                                ws.Cells[3, currentColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[3, currentColumn].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                                ws.Cells[3, currentColumn].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                currentColumn++;
                            }
                        }

                        // 3. Nội dung Dữ liệu (Rows)
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (dgv.Rows[i].IsNewRow) continue;

                            currentColumn = 1;
                            for (int j = 0; j < colCount; j++)
                            {
                                if (dgv.Columns[j].Visible)
                                {
                                    object? val = dgv.Rows[i].Cells[j].Value;
                                    ws.Cells[i + 4, currentColumn].Value = val != null ? val.ToString() : "";
                                    ws.Cells[i + 4, currentColumn].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    currentColumn++;
                                }
                            }
                        }

                        ws.Cells.AutoFitColumns();
                        FileInfo fileInfo = new FileInfo(sfd.FileName);
                        package.SaveAs(fileInfo);

                        MessageBox.Show($"Xuất tệp Excel thành công!\nĐường dẫn: {sfd.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất tệp Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
