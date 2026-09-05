# Đồ án Kiểm tra giữa kỳ — Quản lý Đặt sân thể thao

Giải pháp cho đề thi "Lập trình ứng dụng CSDL (Thực hành)" — CSDL **QUANLYDATSAN**.

## 1. Cấu trúc thư mục

```
QuanLyDatSan/
├── SQL/
│   └── StoredProcedures_Functions.sql   <- Procedure + Function (chạy sau QUANLYDATSAN.sql)
├── App/
│   ├── QuanLyDatSan.csproj              <- Project WinForms (.NET Framework 4.7.2)
│   ├── App.config                       <- Chuỗi kết nối SQL Server
│   ├── Program.cs                       <- Entry point (mở Frm_XemThongTinSan)
│   ├── DBConnection.cs                  <- Lớp tiện ích kết nối/gọi CSDL
│   ├── Frm_XemThongTinSan.cs / .Designer.cs   <- CÂU 1
│   └── Frm_DatSan.cs / .Designer.cs           <- CÂU 2
└── README.md
```

## 2. Cách chạy

1. **Tạo CSDL**: chạy file `QUANLYDATSAN.sql` (bạn đã có sẵn) trên SQL Server để
   tạo database + dữ liệu mẫu.
2. **Tạo Procedure/Function**: chạy tiếp file `SQL/StoredProcedures_Functions.sql`.
3. **Mở project**: mở thư mục `App/` bằng Visual Studio (double-click
   `QuanLyDatSan.csproj`), hoặc `dotnet build`/`dotnet run` nếu có .NET SDK.
4. **Sửa chuỗi kết nối** trong `App.config` cho đúng SQL Server instance của bạn
   (mặc định `Data Source=.` dùng Windows Authentication).
5. Build & chạy — form đầu tiên hiện ra là `Frm_XemThongTinSan` (Câu 1); nút
   **"Đặt sân"** mở `Frm_DatSan` (Câu 2).

## 3. Đối chiếu yêu cầu đề bài ↔ nơi xử lý

### Câu 1 — Frm_XemThongTinSan
| Yêu cầu | Nơi xử lý |
|---|---|
| Hiển thị tất cả sân lên DataGridView khi mở form | `Frm_XemThongTinSan_Load` → `LoadDataSan(null)` gọi Procedure **HienThiSan** |
| Function `ThongKeSan` (số sân trống / đã thuê) | `SQL/StoredProcedures_Functions.sql` → `dbo.ThongKeSan(@TenLoaiSan)` |
| Chọn CheckBox → gọi Procedure `HienThiSan` | `ChonLoaiSan_CheckedChanged` |
| Chọn CheckBox → gọi Function `ThongKeSan` | `ChonLoaiSan_CheckedChanged` → `LoadThongKeSan` |
| Thoát → hộp thoại hỏi đáp | `btnThoat_Click` |

### Câu 2 — Frm_DatSan
| Yêu cầu | Nơi xử lý |
|---|---|
| Hiển thị tất cả sân lên DataGridViewChonSan | `Frm_DatSan_Load` → `LoadChonSan` (Procedure **HienThiSan**) |
| Double click sân → đưa vào DataGridViewThongTinDatSan | `dgvChonSan_CellDoubleClick` |
| Nút "Lưu thông tin" → lưu Phiếu đặt sân + Thông tin đặt sân (Procedure) | `btnLuu_Click` → Procedure **LuuPhieuDatSan**, **LuuThongTinDatSan** |
| Hàm thống kê số lượng sân + tổng tiền theo Số phiếu | `dbo.ThongKeDatSan(@SoPhieu)`, gọi trong `ThongKeTheoPhieu` |
| Nút "Huỷ" → huỷ thông tin đặt sân | `btnHuy_Click` → Procedure **HuyThongTinDatSan** |
| Nút "Thoát" → hộp thoại hỏi đáp | `btnThoat_Click` |

## 4. Ghi chú thiết kế

- **Ô "Giờ bắt đầu / Giờ kết thúc / Đơn giá thuê"** trong `DataGridViewThongTinDatSan`
  được để **cho phép sửa trực tiếp trên lưới** (ô Số phiếu / Mã sân / Tên sân là
  read-only) vì bảng `SAN` không có sẵn giờ thuê hay đơn giá — người dùng nhập
  sau khi double-click chọn sân. Giá trị mặc định là `08:00 - 09:00`, đơn giá `0`.
- Khi lưu thành công 1 dòng `THONGTINDATSAN`, Procedure **LuuThongTinDatSan**
  tự động cập nhật `TRANGTHAI` của sân đó thành **Đã thuê (1)**.
- Khi bấm **Huỷ**, Procedure **HuyThongTinDatSan** trả các sân liên quan về
  **Trống (0)** trước khi xoá phiếu.
- Trong `HienThiSan` và `ThongKeSan`, tham số `@TenLoaiSan = NULL` (hoặc rỗng,
  hoặc `N'Tất cả'`) nghĩa là **không lọc** — trả về toàn bộ dữ liệu.
- Các CheckBox loại sân trong Câu 1 hoạt động theo kiểu chọn 1 lựa chọn tại một
  thời điểm (giống radio button) vì `HienThiSan` chỉ nhận 1 tên loại sân.
