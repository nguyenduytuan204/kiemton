USE QuanLySinhVienDB;
GO

/* ==============================
1. MON HOC
============================== */
IF NOT EXISTS (SELECT 1 FROM MonHoc WHERE MaMon = 'BUS102')
INSERT INTO MonHoc (MaMon, TenMon, SoTinChi)
VALUES ('BUS102', N'Quản trị sự thay đổi', 3);
GO

/* ==============================
2. GIANG VIEN
============================== */
IF NOT EXISTS (SELECT 1 FROM GiangVien WHERE MaGV = '61198')
INSERT INTO GiangVien (MaGV, TenGV, Username, Password, Quyen)
VALUES ('61198', N'Đinh Thị Mừng', 'gv61198', '123456', N'GiangVien');
GO

/* ==============================
3. LOP HOC
============================== */
IF NOT EXISTS (SELECT 1 FROM LopHoc WHERE MaLop = 'BUS102_L01')
INSERT INTO LopHoc (MaLop, TenLop, SiSo, MaGV)
VALUES ('BUS102_L01', N'BUS102 - Quản trị sự thay đổi (Lớp 01)', 150, '61198');
GO

/* ==============================
4. TAO BANG TAM
============================== */
IF OBJECT_ID('tempdb..#SV_Moi') IS NOT NULL DROP TABLE #SV_Moi;

CREATE TABLE #SV_Moi
(
    MaSV VARCHAR(20),
    TenSV NVARCHAR(100),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    Email VARCHAR(100),
    SoDienThoai VARCHAR(20),
    MaLop VARCHAR(20)
);
GO