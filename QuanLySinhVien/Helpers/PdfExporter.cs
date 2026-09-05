using System;
using System.IO;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font;

namespace QuanLySinhVien.Helpers
{
    public static class PdfExporter
    {
        public static void ExportToPdf(DataGridView dgv, string title = "BÁO CÁO DỮ LIỆU SINH VIÊN")
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (PdfWriter writer = new PdfWriter(sfd.FileName))
                    {
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            Document document = new Document(pdf);

                            // Load font Arial để hiển thị tiếng Việt chuẩn Unicode
                            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                            PdfFont font;
                            if (File.Exists(fontPath))
                            {
                                font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
                            }
                            else
                            {
                                font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);
                            }
                            document.SetFont(font);

                            // 1. Tiêu đề Báo cáo
                            Paragraph pTitle = new Paragraph(title.ToUpper())
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(16)
                                .SetMarginBottom(15);
                            document.Add(pTitle);

                            // 2. Ngày xuất báo cáo
                            Paragraph pDate = new Paragraph($"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                .SetTextAlignment(TextAlignment.RIGHT)
                                .SetFontSize(10)
                                .SetMarginBottom(10);
                            document.Add(pDate);

                            // 3. Tạo Bảng (Table)
                            int visibleColCount = 0;
                            foreach (DataGridViewColumn col in dgv.Columns)
                            {
                                if (col.Visible) visibleColCount++;
                            }

                            if (visibleColCount == 0)
                            {
                                MessageBox.Show("Không có cột nào để xuất PDF!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            Table table = new Table(visibleColCount);
                            table.SetWidth(UnitValue.CreatePercentValue(100));

                            // Add Column Headers
                            foreach (DataGridViewColumn col in dgv.Columns)
                            {
                                if (col.Visible)
                                {
                                    Cell cellHeader = new Cell()
                                        .Add(new Paragraph(col.HeaderText ?? "").SetFontSize(10))
                                        .SetTextAlignment(TextAlignment.CENTER)
                                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY);
                                    table.AddHeaderCell(cellHeader);
                                }
                            }

                            // Add Rows Data
                            for (int i = 0; i < dgv.Rows.Count; i++)
                            {
                                if (dgv.Rows[i].IsNewRow) continue;

                                foreach (DataGridViewColumn col in dgv.Columns)
                                {
                                    if (col.Visible)
                                    {
                                        object? val = dgv.Rows[i].Cells[col.Index].Value;
                                        string text = val != null ? val.ToString() ?? "" : "";

                                        Cell cellData = new Cell()
                                            .Add(new Paragraph(text).SetFontSize(9))
                                            .SetTextAlignment(TextAlignment.LEFT);
                                        table.AddCell(cellData);
                                    }
                                }
                            }

                            document.Add(table);
                            document.Close();
                        }
                    }

                    MessageBox.Show($"Xuất tệp PDF thành công!\nĐường dẫn: {sfd.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
