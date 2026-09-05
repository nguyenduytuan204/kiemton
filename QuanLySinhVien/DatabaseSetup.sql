-- ============================================================================
-- KỊCH BẢN TẠO CƠ SỞ DỮ LIỆU SQL SERVER: HỆ THỐNG QUẢN LÝ SINH VIÊN NÂNG CAO
-- Hệ quản trị CSDL: Microsoft SQL Server (2016 trở lên)
-- Tác giả: Antigravity AI
-- ============================================================================

-- 1. TẠO CƠ SỞ DỮ LIỆU (DATABASE)
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'QuanLySinhVienDB')
BEGIN
    ALTER DATABASE QuanLySinhVienDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLySinhVienDB;
END
GO

CREATE DATABASE QuanLySinhVienDB;
GO

USE QuanLySinhVienDB;
GO

-- ============================================================================
-- 2. TẠO CÁC BẢNG (TABLES) VÀ RÀNG BUỘC (CONSTRAINTS)
-- ============================================================================

-- 2.1. Bảng GiangVien (Giảng viên)
CREATE TABLE GiangVien (
    MaGV VARCHAR(20) NOT NULL,
    TenGV NVARCHAR(100) NOT NULL,
    Username VARCHAR(50) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    Quyen NVARCHAR(20) NOT NULL DEFAULT 'GiangVien', -- 'Admin' hoặc 'GiangVien'
    Email VARCHAR(100),
    DienThoai VARCHAR(20),
    CONSTRAINT PK_GiangVien PRIMARY KEY (MaGV),
    CONSTRAINT UQ_GiangVien_Username UNIQUE (Username)
);
GO

-- 2.2. Bảng LopHoc (Lớp học - Mỗi giảng viên quản lý nhiều lớp)
CREATE TABLE LopHoc (
    MaLop VARCHAR(20) NOT NULL,
    TenLop NVARCHAR(100) NOT NULL,
    SiSo INT NOT NULL DEFAULT 0 CHECK (SiSo >= 0),
    MaGV VARCHAR(20) NULL,
    CONSTRAINT PK_LopHoc PRIMARY KEY (MaLop),
    CONSTRAINT FK_LopHoc_GiangVien FOREIGN KEY (MaGV) 
        REFERENCES GiangVien(MaGV) 
        ON DELETE SET NULL 
        ON UPDATE CASCADE
);
GO

-- 2.3. Bảng SinhVien (Sinh viên - Mỗi lớp có nhiều sinh viên)
CREATE TABLE SinhVien (
    MaSV VARCHAR(20) NOT NULL,
    TenSV NVARCHAR(100) NOT NULL,
    NgaySinh DATE NOT NULL,
    GioiTinh NVARCHAR(10) NOT NULL CHECK (GioiTinh IN (N'Nam', N'Nữ')),
    Email VARCHAR(100),
    SoDienThoai VARCHAR(20),
    MaLop VARCHAR(20) NOT NULL,
    CONSTRAINT PK_SinhVien PRIMARY KEY (MaSV),
    CONSTRAINT FK_SinhVien_LopHoc FOREIGN KEY (MaLop) 
        REFERENCES LopHoc(MaLop) 
        ON DELETE CASCADE 
        ON UPDATE CASCADE
);
GO

-- 2.4. Bảng MonHoc (Môn học)
CREATE TABLE MonHoc (
    MaMon VARCHAR(20) NOT NULL,
    TenMon NVARCHAR(100) NOT NULL,
    SoTinChi INT NOT NULL CHECK (SoTinChi > 0),
    CONSTRAINT PK_MonHoc PRIMARY KEY (MaMon)
);
GO

-- 2.5. Bảng DiemThanhPhan (Điểm thành phần - Mỗi môn có nhiều điểm thành phần)
CREATE TABLE DiemThanhPhan (
    MaDiem INT IDENTITY(1,1) NOT NULL,
    MaSV VARCHAR(20) NOT NULL,
    MaMon VARCHAR(20) NOT NULL,
    LoaiDiem NVARCHAR(50) NOT NULL, -- N'Chuyên cần', N'Giữa kỳ', N'Cuối kỳ'
    Diem FLOAT NOT NULL,
    CONSTRAINT PK_DiemThanhPhan PRIMARY KEY (MaDiem),
    CONSTRAINT FK_DiemThanhPhan_SinhVien FOREIGN KEY (MaSV) 
        REFERENCES SinhVien(MaSV) 
        ON DELETE CASCADE 
        ON UPDATE CASCADE,
    CONSTRAINT FK_DiemThanhPhan_MonHoc FOREIGN KEY (MaMon) 
        REFERENCES MonHoc(MaMon) 
        ON DELETE CASCADE 
        ON UPDATE CASCADE
);
GO

-- ============================================================================
-- 3. CÁC TRIGGER BẢO VỆ DỮ LIỆU & TỰ ĐỘNG HÓA
-- ============================================================================

-- 3.1. TRIGGER: Kiểm tra điểm số nằm trong khoảng từ 0.0 đến 10.0
CREATE TRIGGER trg_KiemTraDiem
ON DiemThanhPhan
FOR INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE Diem < 0.0 OR Diem > 10.0
    )
    BEGIN
        RAISERROR (N'Lỗi: Điểm thành phần phải nằm trong khoảng từ 0.0 đến 10.0!', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO

-- 3.2. TRIGGER: Tự động cập nhật Sĩ Số lớp học khi thêm/xóa sinh viên
CREATE TRIGGER trg_CapNhatSiSoLop
ON SinhVien
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật cho các lớp có sinh viên mới thêm vào hoặc đổi lớp
    UPDATE LopHoc
    SET SiSo = (SELECT COUNT(*) FROM SinhVien WHERE SinhVien.MaLop = LopHoc.MaLop)
    WHERE MaLop IN (
        SELECT MaLop FROM inserted
        UNION
        SELECT MaLop FROM deleted
    );
END;
GO

-- ============================================================================
-- 4. STORED PROCEDURE: THÊM SINH VIÊN
-- ============================================================================

CREATE PROCEDURE sp_ThemSinhVien
    @MaSV VARCHAR(20),
    @TenSV NVARCHAR(100),
    @NgaySinh DATE,
    @GioiTinh NVARCHAR(10),
    @Email VARCHAR(100) = NULL,
    @SoDienThoai VARCHAR(20) = NULL,
    @MaLop VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Kiểm tra mã sinh viên đã tồn tại chưa
    IF EXISTS (SELECT 1 FROM SinhVien WHERE MaSV = @MaSV)
    BEGIN
        RAISERROR(N'Lỗi: Mã sinh viên %s đã tồn tại trong hệ thống!', 16, 1, @MaSV);
        RETURN -1;
    END

    -- 2. Kiểm tra mã lớp học có tồn tại không
    IF NOT EXISTS (SELECT 1 FROM LopHoc WHERE MaLop = @MaLop)
    BEGIN
        RAISERROR(N'Lỗi: Mã lớp học %s không tồn tại!', 16, 1, @MaLop);
        RETURN -2;
    END

    -- 3. Kiểm tra giới tính
    IF @GioiTinh NOT IN (N'Nam', N'Nữ')
    BEGIN
        RAISERROR(N'Lỗi: Giới tính phải là ''Nam'' hoặc ''Nữ''!', 16, 1);
        RETURN -3;
    END

    -- 4. Thêm sinh viên mới
    INSERT INTO SinhVien (MaSV, TenSV, NgaySinh, GioiTinh, Email, SoDienThoai, MaLop)
    VALUES (@MaSV, @TenSV, @NgaySinh, @GioiTinh, @Email, @SoDienThoai, @MaLop);

    PRINT N'Thêm sinh viên thành công!';
    RETURN 0;
END;
GO

-- ============================================================================
-- 5. CHÈN DỮ LIỆU MẪU (SEED DATA)
-- ============================================================================

-- 5.1. Thêm Giảng Viên (Mật khẩu mặc định: 123456)
INSERT INTO GiangVien (MaGV, TenGV, Username, Password, Quyen, Email, DienThoai) VALUES
('GV001', N'Nguyễn Văn Admin', 'admin', '123456', 'Admin', 'admin@cntt.edu.vn', '0901234567'),
('GV002', N'Trần Thị Hương', 'huongtt', '123456', 'GiangVien', 'huongtt@cntt.edu.vn', '0912345678'),
('GV003', N'Lê Hoàng Nam', 'namlh', '123456', 'GiangVien', 'namlh@cntt.edu.vn', '0923456789');
GO

-- 5.2. Thêm Lớp Học
INSERT INTO LopHoc (MaLop, TenLop, MaGV) VALUES
('CNTT01', N'Công Nghệ Thông Tin 1 K64', 'GV002'),
('CNTT02', N'Công Nghệ Thông Tin 2 K64', 'GV002'),
('KHMT01', N'Khoa Học Máy Tính K64', 'GV003');
GO

-- 5.3. Thêm Sinh Viên qua Stored Procedure sp_ThemSinhVien
EXEC sp_ThemSinhVien 'SV001', N'Nguyễn Văn An', '2003-05-15', N'Nam', 'an.nv@gmail.com', '0345678901', 'CNTT01';
EXEC sp_ThemSinhVien 'SV002', N'Lê Thị Bình', '2003-08-20', N'Nữ', 'binh.lt@gmail.com', '0345678902', 'CNTT01';
EXEC sp_ThemSinhVien 'SV003', N'Phạm Cường', '2003-01-10', N'Nam', 'cuong.p@gmail.com', '0345678903', 'CNTT01';
EXEC sp_ThemSinhVien 'SV004', N'Hoàng Thị Dung', '2003-11-25', N'Nữ', 'dung.ht@gmail.com', '0345678904', 'CNTT02';
EXEC sp_ThemSinhVien 'SV005', N'Đỗ Minh Đức', '2003-03-05', N'Nam', 'duc.dm@gmail.com', '0345678905', 'CNTT02';
EXEC sp_ThemSinhVien 'SV006', N'Vũ Thị Giang', '2003-09-12', N'Nữ', 'giang.vt@gmail.com', '0345678906', 'KHMT01';
GO

-- 5.4. Thêm Môn Học
INSERT INTO MonHoc (MaMon, TenMon, SoTinChi) VALUES
('MH001', N'Lập trình Windows Forms C#', 3),
('MH002', N'Cơ sở dữ liệu SQL Server', 3),
('MH003', N'Cấu trúc dữ liệu & Giải thuật', 4),
('MH004', N'Phát triển ứng dụng Web', 3);
GO

-- 5.5. Thêm Điểm Thành Phần (Chuyên cần, Giữa kỳ, Cuối kỳ)
INSERT INTO DiemThanhPhan (MaSV, MaMon, LoaiDiem, Diem) VALUES
-- Sinh viên SV001 - MH001
('SV001', 'MH001', N'Chuyên cần', 9.0),
('SV001', 'MH001', N'Giữa kỳ', 8.5),
('SV001', 'MH001', N'Cuối kỳ', 9.0),

-- Sinh viên SV001 - MH002
('SV001', 'MH002', N'Chuyên cần', 10.0),
('SV001', 'MH002', N'Giữa kỳ', 7.5),
('SV001', 'MH002', N'Cuối kỳ', 8.0),

-- Sinh viên SV002 - MH001
('SV002', 'MH001', N'Chuyên cần', 8.0),
('SV002', 'MH001', N'Giữa kỳ', 7.0),
('SV002', 'MH001', N'Cuối kỳ', 7.5),

-- Sinh viên SV003 - MH001
('SV003', 'MH001', N'Chuyên cần', 10.0),
('SV003', 'MH001', N'Giữa kỳ', 9.5),
('SV003', 'MH001', N'Cuối kỳ', 9.5),

-- Sinh viên SV004 - MH001
('SV004', 'MH001', N'Chuyên cần', 6.0),
('SV004', 'MH001', N'Giữa kỳ', 5.0),
('SV004', 'MH001', N'Cuối kỳ', 4.5);
GO

-- ============================================================================
-- 6. VIEW THỐNG KÊ TỔNG HỢP ĐIỂM TRUNG BÌNH & XẾP LOẠI
-- ============================================================================

CREATE VIEW v_BangDiemTongHop AS
SELECT 
    SV.MaSV,
    SV.TenSV,
    L.TenLop,
    MH.MaMon,
    MH.TenMon,
    MAX(CASE WHEN D.LoaiDiem = N'Chuyên cần' THEN D.Diem END) AS DiemCC,
    MAX(CASE WHEN D.LoaiDiem = N'Giữa kỳ' THEN D.Diem END) AS DiemGK,
    MAX(CASE WHEN D.LoaiDiem = N'Cuối kỳ' THEN D.Diem END) AS DiemCK,
    ROUND(
        (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Chuyên cần' THEN D.Diem END), 0) * 0.1) +
        (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Giữa kỳ' THEN D.Diem END), 0) * 0.3) +
        (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Cuối kỳ' THEN D.Diem END), 0) * 0.6), 2
    ) AS DiemTB,
    CASE 
        WHEN ROUND(
            (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Chuyên cần' THEN D.Diem END), 0) * 0.1) +
            (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Giữa kỳ' THEN D.Diem END), 0) * 0.3) +
            (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Cuối kỳ' THEN D.Diem END), 0) * 0.6), 2
        ) >= 8.5 THEN N'Giỏi'
        WHEN ROUND(
            (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Chuyên cần' THEN D.Diem END), 0) * 0.1) +
            (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Giữa kỳ' THEN D.Diem END), 0) * 0.3) +
            (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Cuối kỳ' THEN D.Diem END), 0) * 0.6), 2
        ) >= 7.0 THEN N'Khá'
        WHEN ROUND(
            (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Chuyên cần' THEN D.Diem END), 0) * 0.1) +
            (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Giữa kỳ' THEN D.Diem END), 0) * 0.3) +
            (ISNULL(MAX(CASE WHEN D.LoaiDiem = N'Cuối kỳ' THEN D.Diem END), 0) * 0.6), 2
        ) >= 5.5 THEN N'Trung bình'
        ELSE N'Yếu'
    END AS XepLoai
FROM SinhVien SV
INNER JOIN LopHoc L ON SV.MaLop = L.MaLop
INNER JOIN DiemThanhPhan D ON SV.MaSV = D.MaSV
INNER JOIN MonHoc MH ON D.MaMon = MH.MaMon
GROUP BY SV.MaSV, SV.TenSV, L.TenLop, MH.MaMon, MH.TenMon;
GO

-- KẾT THÚC KỊCH BẢN SQL
