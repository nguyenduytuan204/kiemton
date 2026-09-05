/*
    QUANLYDATSAN - Stored Procedures & Functions
    Dùng cho bài thi giữa kỳ - Lập trình ứng dụng CSDL
    Chạy file này SAU KHI đã chạy file QUANLYDATSAN.sql (tạo DB + dữ liệu mẫu)
*/
USE QUANLYDATSAN
GO

-- =========================================================================
-- 1. FUNCTION: ThongKeSan
--    Thống kê số sân TRỐNG và số sân ĐÃ THUÊ (dựa vào TRANGTHAI).
--    Nếu @TenLoaiSan = NULL (hoặc rỗng) => thống kê trên TẤT CẢ các sân.
--    Nếu @TenLoaiSan có giá trị => thống kê chỉ trên loại sân đó.
--    Dùng cho Câu 1 (khi mở form và khi chọn CheckBox)
-- =========================================================================
IF OBJECT_ID('dbo.ThongKeSan', 'IF') IS NOT NULL
    DROP FUNCTION dbo.ThongKeSan
GO
CREATE FUNCTION dbo.ThongKeSan (@TenLoaiSan NVARCHAR(50) = NULL)
RETURNS TABLE
AS
RETURN
(
    SELECT
        SUM(CASE WHEN S.TRANGTHAI = 0 THEN 1 ELSE 0 END) AS SoSanTrong,
        SUM(CASE WHEN S.TRANGTHAI = 1 THEN 1 ELSE 0 END) AS SoSanDaThue
    FROM SAN S
    JOIN LOAISAN L ON S.MALS = L.MALS
    WHERE (@TenLoaiSan IS NULL OR @TenLoaiSan = N'' OR L.TENLS = @TenLoaiSan)
)
GO

-- Cách gọi:  SELECT * FROM dbo.ThongKeSan(NULL)
--            SELECT * FROM dbo.ThongKeSan(N'Cầu lông')

-- =========================================================================
-- 2. PROCEDURE: HienThiSan
--    Hiển thị thông tin sân theo Tên loại sân.
--    Nếu @TenLoaiSan = NULL / '' / N'Tất cả' => hiển thị TẤT CẢ các sân.
--    Dùng cho Câu 1 (khi mở form và khi chọn CheckBox) và Câu 2 (nạp
--    danh sách sân vào DataGridViewChonSan)
-- =========================================================================
IF OBJECT_ID('dbo.HienThiSan', 'P') IS NOT NULL
    DROP PROCEDURE dbo.HienThiSan
GO
CREATE PROCEDURE dbo.HienThiSan
    @TenLoaiSan NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @TenLoaiSan IS NULL OR @TenLoaiSan = N'' OR @TenLoaiSan = N'Tất cả'
    BEGIN
        SELECT S.MASAN, S.TENSAN, S.VITRI, S.TRANGTHAI, S.MALS, L.TENLS
        FROM SAN S
        JOIN LOAISAN L ON S.MALS = L.MALS
        ORDER BY S.MASAN;
    END
    ELSE
    BEGIN
        SELECT S.MASAN, S.TENSAN, S.VITRI, S.TRANGTHAI, S.MALS, L.TENLS
        FROM SAN S
        JOIN LOAISAN L ON S.MALS = L.MALS
        WHERE L.TENLS = @TenLoaiSan
        ORDER BY S.MASAN;
    END
END
GO

-- =========================================================================
-- 3. PROCEDURE: LuuPhieuDatSan
--    Thêm mới (hoặc cập nhật nếu đã tồn tại) 1 Phiếu đặt sân.
--    Dùng cho Câu 2 - nút "Lưu thông tin"
-- =========================================================================
IF OBJECT_ID('dbo.LuuPhieuDatSan', 'P') IS NOT NULL
    DROP PROCEDURE dbo.LuuPhieuDatSan
GO
CREATE PROCEDURE dbo.LuuPhieuDatSan
    @SoPhieu CHAR(10),
    @NgayDat DATE,
    @SoDT    CHAR(10),
    @TenKH   NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM PHIEUDATSAN WHERE SOPHIEU = @SoPhieu)
    BEGIN
        UPDATE PHIEUDATSAN
        SET NGAYDAT = @NgayDat, SODT = @SoDT, TENKH = @TenKH
        WHERE SOPHIEU = @SoPhieu;
    END
    ELSE
    BEGIN
        INSERT INTO PHIEUDATSAN (SOPHIEU, NGAYDAT, SODT, TENKH)
        VALUES (@SoPhieu, @NgayDat, @SoDT, @TenKH);
    END
END
GO

-- =========================================================================
-- 4. PROCEDURE: LuuThongTinDatSan
--    Thêm mới (hoặc cập nhật) 1 dòng chi tiết đặt sân, đồng thời cập
--    nhật TRANGTHAI của sân đó thành "Đã thuê" (1).
--    Dùng cho Câu 2 - nút "Lưu thông tin" (gọi lặp lại cho từng sân đã chọn)
-- =========================================================================
IF OBJECT_ID('dbo.LuuThongTinDatSan', 'P') IS NOT NULL
    DROP PROCEDURE dbo.LuuThongTinDatSan
GO
CREATE PROCEDURE dbo.LuuThongTinDatSan
    @SoPhieu     CHAR(10),
    @MaSan       CHAR(10),
    @GioBD       TIME,
    @GioKT       TIME,
    @DonGiaThue  INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM THONGTINDATSAN WHERE SOPHIEU = @SoPhieu AND MASAN = @MaSan)
    BEGIN
        UPDATE THONGTINDATSAN
        SET GIOBD = @GioBD, GIOKT = @GioKT, DONGIATHUE = @DonGiaThue
        WHERE SOPHIEU = @SoPhieu AND MASAN = @MaSan;
    END
    ELSE
    BEGIN
        INSERT INTO THONGTINDATSAN (SOPHIEU, MASAN, GIOBD, GIOKT, DONGIATHUE)
        VALUES (@SoPhieu, @MaSan, @GioBD, @GioKT, @DonGiaThue);
    END

    UPDATE SAN SET TRANGTHAI = 1 WHERE MASAN = @MaSan;
END
GO

-- =========================================================================
-- 5. PROCEDURE: HuyThongTinDatSan
--    Huỷ (xoá) toàn bộ thông tin đặt sân của 1 phiếu, trả các sân liên
--    quan về trạng thái "Trống" (0).
--    Dùng cho Câu 2 - nút "Huỷ"
-- =========================================================================
IF OBJECT_ID('dbo.HuyThongTinDatSan', 'P') IS NOT NULL
    DROP PROCEDURE dbo.HuyThongTinDatSan
GO
CREATE PROCEDURE dbo.HuyThongTinDatSan
    @SoPhieu CHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE SAN
    SET TRANGTHAI = 0
    WHERE MASAN IN (SELECT MASAN FROM THONGTINDATSAN WHERE SOPHIEU = @SoPhieu);

    DELETE FROM THONGTINDATSAN WHERE SOPHIEU = @SoPhieu;
    DELETE FROM PHIEUDATSAN WHERE SOPHIEU = @SoPhieu;
END
GO

-- =========================================================================
-- 6. FUNCTION: ThongKeDatSan
--    Thống kê SỐ LƯỢNG SÂN và TỔNG TIỀN (số giờ * đơn giá thuê) của
--    1 phiếu đặt sân, tham số là Số phiếu.
--    Dùng cho Câu 2 - sau khi Lưu thông tin
-- =========================================================================
IF OBJECT_ID('dbo.ThongKeDatSan', 'IF') IS NOT NULL
    DROP FUNCTION dbo.ThongKeDatSan
GO
CREATE FUNCTION dbo.ThongKeDatSan (@SoPhieu CHAR(10))
RETURNS TABLE
AS
RETURN
(
    SELECT
        COUNT(*) AS SoLuongSan,
        ISNULL(SUM(DATEDIFF(MINUTE, GIOBD, GIOKT) / 60.0 * DONGIATHUE), 0) AS TongTien
    FROM THONGTINDATSAN
    WHERE SOPHIEU = @SoPhieu
)
GO

-- Cách gọi: SELECT * FROM dbo.ThongKeDatSan('P001')

-- =========================================================================
-- KIỂM TRA NHANH
-- =========================================================================
-- EXEC HienThiSan NULL
-- EXEC HienThiSan N'Cầu lông'
-- SELECT * FROM dbo.ThongKeSan(NULL)
-- SELECT * FROM dbo.ThongKeSan(N'Bóng đá')
-- SELECT * FROM dbo.ThongKeDatSan('P001')
