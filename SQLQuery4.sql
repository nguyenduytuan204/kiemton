/* ============================================================
   SCRIPT: THEM 150 SINH VIEN VAO DATABASE THAT (QuanLySinhVienDB)
   Cau truc dung: SinhVien, MonHoc, LopHoc, GiangVien, DiemThanhPhan
   - Chi THEM du lieu, KHONG xoa/dung du lieu cu.
   - Neu du lieu da ton tai (trung khoa chinh) se duoc BO QUA.
   - Ngay sinh: random 2000-2006. Gioi tinh: xen ke Nam/Nu.
   - 150 SV duoc gan vao 1 Lop hoc phan MOI: 'BUS102_L01'
     (theo dung lop hoc phan BUS102 - Quan tri su thay doi
     trong file diem danh goc).
   - Diem: moi SV co 1 dong DiemThanhPhan (loai 'Tong ket'),
     tat ca >= 5.5 (tren trung binh).
   ============================================================ */

USE QuanLySinhVienDB;
GO

------------------------------------------------------------
-- BUOC 1: MON HOC (BUS102 - Quan tri su thay doi, 3 tin chi)
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.MonHoc WHERE MaMon = 'BUS102')
    INSERT INTO dbo.MonHoc (MaMon, TenMon, SoTinChi)
    VALUES ('BUS102', N'Quản trị sự thay đổi', 3);
GO

------------------------------------------------------------
-- BUOC 2: GIANG VIEN (61198 - Dinh Thi Mung, CBGD trong file goc)
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.GiangVien WHERE MaGV = '61198')
    INSERT INTO dbo.GiangVien (MaGV, TenGV, Username, Password, Quyen, Email, DienThoai)
    VALUES ('61198', N'Đinh Thị Mừng', 'gv61198', 'Matkhau@123', N'GiangVien', NULL, NULL);
GO

------------------------------------------------------------
-- BUOC 3: LOP HOC PHAN MOI (BUS102_L01), SiSo = 150, GVCN = 61198
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.LopHoc WHERE MaLop = 'BUS102_L01')
    INSERT INTO dbo.LopHoc (MaLop, TenLop, SiSo, MaGV)
    VALUES ('BUS102_L01', N'BUS102 - Quản trị sự thay đổi (Lớp 01)', 150, '61198');
GO

------------------------------------------------------------
-- BUOC 4: THEM 150 SINH VIEN (chi them neu MaSV chua ton tai)
------------------------------------------------------------
IF OBJECT_ID('tempdb..#SV_Moi') IS NOT NULL DROP TABLE #SV_Moi;
CREATE TABLE #SV_Moi
(
    MaSV       VARCHAR(20),
    TenSV      NVARCHAR(100),
    NgaySinh   DATE,
    GioiTinh   NVARCHAR(10),
    Email      VARCHAR(100),
    SoDienThoai VARCHAR(20),
    MaLop      VARCHAR(20)
);

INSERT INTO #SV_Moi (MaSV, TenSV, NgaySinh, GioiTinh, Email, SoDienThoai, MaLop) VALUES
('221A040022', N'Mai Xuân Bảo Trân', '2002-03-13', N'Nam', NULL, NULL, 'BUS102_L01'),
('221A240119', N'Trần Mạnh Khải', '2005-01-03', N'Nữ', NULL, NULL, 'BUS102_L01'),
('221A370315', N'Tô Nữ Kiều Oanh', '2006-09-04', N'Nam', NULL, NULL, 'BUS102_L01'),
('221A370618', N'Trương Thị Thảo Trinh', '2002-10-02', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A010057', N'Nguyễn Minh Nhựt', '2004-04-02', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A010298', N'Phan Ngọc Gia Hân', '2000-07-14', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A010317', N'Trương Quốc Dự', '2000-04-03', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A010627', N'Huỳnh Phạm Thanh Đức', '2004-07-02', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A010628', N'Lê Uyên Nhi', '2006-10-04', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A010702', N'Nguyễn Duy Tuấn', '2001-11-21', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A010947', N'Phạm Quốc Vinh', '2004-01-19', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A010949', N'Trần Anh Quân', '2004-07-02', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A010959', N'Nguyễn Thanh Tuấn', '2001-01-18', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A011005', N'Thái Quang Lâm', '2006-03-10', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A080006', N'Trần Thị Thu Hiền', '2003-03-18', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A080030', N'Huỳnh Thanh Hải', '2000-10-10', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A080160', N'Nguyễn Thùy Linh', '2004-11-06', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A080398', N'Đỗ Ngọc Trâm', '2000-10-19', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A090009', N'Huỳnh Diệu Hiền', '2005-04-12', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A090045', N'Nguyễn Duy Khang', '2000-09-23', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231a100367', N'Phạm Nguyễn Tâm Đan', '2000-10-02', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A100369', N'Nguyễn Huỳnh Anh Thư', '2004-04-16', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A140432', N'Nguyễn Ngọc Khánh Băng', '2005-09-14', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A170256', N'Hồ Thị Quỳnh Hương', '2006-06-15', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A170390', N'Hồ Thị Trân Châu', '2004-08-12', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A180017', N'Võ Huỳnh Hương', '2002-04-26', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A180019', N'Đàm Hoàng Diệu Linh', '2001-12-25', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A180023', N'Trần Thị Mỹ Hạnh', '2001-02-19', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A180035', N'Kha Kim Yến', '2002-09-16', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A210021', N'Hồ Yến Thiên', '2002-12-15', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A210144', N'Nguyễn Thị Ka Ti', '2002-10-03', N'Nam', NULL, NULL, 'BUS102_L01'),
('231a210870', N'Trần Nguyễn Kiều Anh', '2000-09-14', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A210993', N'Đặng Phương Anh', '2001-06-05', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A230004', N'Đồng Văn Tâm', '2003-07-02', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A230037', N'Nguyễn Xuân Nghi', '2005-02-25', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A240004', N'Phan Thị Mỹ Dung', '2004-10-26', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A240141', N'Trần Thị Vân Phi', '2006-06-11', N'Nam', NULL, NULL, 'BUS102_L01'),
('231A370810', N'Đào Thị Bảo Trân', '2005-06-20', N'Nữ', NULL, NULL, 'BUS102_L01'),
('231A371495', N'Nguyễn Thị Kim Hiền', '2003-10-26', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A010591', N'Trần Tuấn Tú', '2003-02-27', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A020009', N'Hoàng Minh Quân', '2000-05-16', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A020074', N'Phạm Hữu Nguyên', '2005-11-03', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A020079', N'Phạm Thái Kiệt', '2000-12-23', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A030465', N'Phan Mỹ Ngọc', '2002-11-19', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A040334', N'Phan Đặng Thùy Dung', '2005-08-10', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A040335', N'Lê Thị Thanh Hằng', '2005-07-22', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A070298', N'Trần Thị Ngọc Thư', '2002-01-15', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A070424', N'Mai Gia Huy', '2002-03-20', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A080107', N'Trần Ngọc Gia Khiêm', '2000-08-02', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A080190', N'Nguyễn Thị Phúc Lộc', '2001-05-05', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A080203', N'Trần Triệu Vĩ', '2005-04-13', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A080270', N'Lê Anh Nhi', '2003-08-03', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A080287', N'Vạn Nguyễn Như Quỳnh', '2001-08-13', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A080315', N'Nguyễn Thị Thúy Ngân', '2004-05-05', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A100029', N'Phan Ngọc Thùy', '2006-07-28', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A100354', N'Đặng Thụy Châu Trúc', '2004-05-23', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A140155', N'Nguyễn Ngọc Châu', '2003-06-22', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A170043', N'Nguyễn Thị Ngọc Nhi', '2003-04-05', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A170416', N'Đinh Thị Hoa Thanh Trúc', '2000-03-05', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A170420', N'Nguyễn Thị Thanh Trúc', '2001-11-08', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A170421', N'Huỳnh Thị Kiều Như', '2000-08-27', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A170426', N'Nguyễn Thúy Lan', '2004-03-09', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A170857', N'Lê Minh Khuê', '2002-01-05', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A211289', N'Trần Long Hải', '2003-09-12', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A230041', N'Nguyễn Hoàng Anh Huy', '2004-10-11', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A230089', N'Nguyễn Ngọc Diễm Vy', '2001-12-28', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A230164', N'Đinh Thị Khả Vy', '2004-10-21', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A230429', N'Trần Minh Khoa', '2005-12-02', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A230544', N'Hồ Thị Thu Thuỷ', '2003-11-26', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A240247', N'Nguyễn Lâm Thanh Thanh', '2004-07-13', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A370280', N'Nguyễn Hoàng Vân Lam', '2003-07-04', N'Nam', NULL, NULL, 'BUS102_L01'),
('241A371234', N'Trịnh Thị Quỳnh Như', '2003-11-13', N'Nữ', NULL, NULL, 'BUS102_L01'),
('241A371487', N'Trần Anh Tuấn Tú', '2000-04-03', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A110014', N'Trần Thị Mỹ Duyên', '2001-08-06', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A110133', N'Trần Quốc Long', '2000-06-20', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A110154', N'Phùng Tấn Đạt', '2000-02-01', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A110162', N'Nguyễn Hoàng Xuân Hiền', '2004-03-18', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A110192', N'Nguyễn Thị Mỹ', '2000-06-20', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A110206', N'Bùi Vũ Uyên', '2000-02-28', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A110233', N'Trần Triệu Vy', '2001-10-13', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A110247', N'Lê Thị Lưu Duyên', '2001-11-09', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A110249', N'Hoàng Kiều Nhi Thư', '2002-10-12', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A130012', N'Phạm Minh Nhật', '2003-02-04', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A130017', N'Nguyễn Khánh Hân', '2006-08-15', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A150015', N'Bùi Thanh Liêm', '2003-08-10', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A150033', N'Nguyễn Phan Cẩm Tú', '2000-03-04', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210056', N'Lê Minh Tấn', '2005-06-24', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210068', N'Bùi Nguyễn Hoàng Huy', '2002-08-27', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210101', N'Nguyễn Thị Thúy Huỳnh', '2005-03-17', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210122', N'Trương Nguyễn Thúy Hằng', '2000-04-17', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210124', N'Lê Ánh Dương', '2002-03-23', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210129', N'Võ Thị Ngọc Hân', '2004-01-25', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210136', N'Nguyễn Thị Anh Thư', '2004-05-21', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210153', N'Nguyễn Thanh Tồng', '2006-02-23', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210161', N'Trần Thị Thảo My', '2006-05-17', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210195', N'Lương Thúy Kiều Dy', '2002-03-12', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210212', N'Nguyễn Hữu Nhân', '2006-04-18', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210228', N'Lê Phương Yến Chi', '2004-09-11', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210241', N'Trần Nguyễn Gia Nguyên', '2005-04-20', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210242', N'Nguyễn Nữ Thanh Tuyền', '2006-04-26', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210246', N'Đào Thùy Gia Linh', '2001-07-24', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210323', N'Trần Ngọc Thanh Bình', '2006-04-07', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210350', N'Phạm Trung Kiên', '2004-08-12', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210382', N'Phan Gia Hân', '2005-01-01', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210464', N'Lý Trần Thảo Nguyên', '2006-05-16', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210476', N'Diệp Hào Nam', '2002-04-23', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210536', N'Đặng Thị Thảo Nguyên', '2004-06-15', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210538', N'Lê Thị Quỳnh Như', '2006-12-12', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210558', N'Hoàng Văn Thái', '2002-02-08', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210578', N'Nguyễn Thị Vân Anh', '2000-04-16', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210580', N'Đỗ Thị Tường Duy', '2001-06-07', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210604', N'Lê Nguyễn Yến Nhi', '2003-10-20', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210618', N'Huỳnh Thị Kim Hoa', '2006-01-16', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210632', N'Nguyễn Cẩm Ly', '2005-06-26', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210638', N'Mai Thị Thu Huyền', '2005-02-27', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210639', N'Trần Kiều Hân', '2005-02-13', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210647', N'Lê Thu Ngân', '2006-12-25', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210693', N'Nguyễn Võ Hải Yến', '2001-08-06', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210768', N'Nguyễn Thế Bằng', '2003-11-11', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210784', N'Nguyễn Thị Yến', '2000-12-13', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210833', N'Trần Nguyễn Hồng Ngân', '2003-07-24', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210835', N'Trần Thị Hương Trang', '2000-12-06', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210836', N'Nguyễn Quốc Thanh', '2001-03-01', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210850', N'Châu Cẩm Hân', '2001-10-15', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A210880', N'Diệp Hoàng Long', '2006-11-05', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A210987', N'Trần Thị Huỳnh Như', '2004-10-16', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211031', N'Lê Thị Ngọc Thư', '2005-06-05', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A211044', N'Huỳnh Thị Minh Thư', '2004-09-05', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211050', N'Nguyễn Thị Phi Nhung', '2000-01-26', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A211075', N'Lê Thị Hoàng Cúc', '2005-11-04', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211112', N'Nguyễn Phương Uyên', '2004-12-05', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A211114', N'Tăng Thị Bích Thủy', '2003-04-27', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211123', N'La Gia Huy', '2006-04-01', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A211129', N'Lâm Thị Thảo Vy', '2002-04-10', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211154', N'Nguyễn Thành Kiều Nhi', '2004-04-25', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A211155', N'Nguyễn Ngọc Phương Thảo', '2004-06-09', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211172', N'Phạm Huỳnh Nguyên', '2004-07-27', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A211176', N'Tiêu Như Quỳnh', '2001-01-24', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211214', N'Phan Thị Uyên Trang', '2002-08-22', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A211266', N'Nguyễn Võ Anh Thư', '2004-09-14', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211293', N'Phan Thị Cẩm Nhi', '2006-09-05', N'Nam', NULL, NULL, 'BUS102_L01'),
('251a211302', N'Phạm Thành Long', '2004-03-17', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211308', N'Nguyễn Ngọc Linh', '2004-01-28', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A211332', N'Lê Trương Minh Thư', '2003-03-20', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211348', N'Nguyễn Uyên Nhi', '2000-03-06', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A211351', N'Nguyễn Thúy Ngọc', '2001-08-20', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A211353', N'Đặng Thị Hoàng', '2005-02-18', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A211391', N'Nguyễn Thị Vân Anh', '2000-06-22', N'Nữ', NULL, NULL, 'BUS102_L01'),
('251A370556', N'Bùi Hải Đăng', '2004-09-18', N'Nam', NULL, NULL, 'BUS102_L01'),
('251A480009', N'Trần Thị Quỳnh Như', '2003-02-18', N'Nữ', NULL, NULL, 'BUS102_L01');

INSERT INTO dbo.SinhVien (MaSV, TenSV, NgaySinh, GioiTinh, Email, SoDienThoai, MaLop)
SELECT tmp.MaSV, tmp.TenSV, tmp.NgaySinh, tmp.GioiTinh, tmp.Email, tmp.SoDienThoai, tmp.MaLop
FROM #SV_Moi tmp
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.SinhVien sv WHERE sv.MaSV = tmp.MaSV
);

DROP TABLE #SV_Moi;
GO

------------------------------------------------------------
-- BUOC 5: THEM DIEM (moi SV 1 dong "Tong ket" mon BUS102, >= 5.5)
------------------------------------------------------------
IF OBJECT_ID('tempdb..#Diem_Moi') IS NOT NULL DROP TABLE #Diem_Moi;
CREATE TABLE #Diem_Moi
(
    MaSV      VARCHAR(20),
    MaMon     VARCHAR(20),
    LoaiDiem  NVARCHAR(50),
    Diem      FLOAT
);

INSERT INTO #Diem_Moi (MaSV, MaMon, LoaiDiem, Diem) VALUES
('221A040022', 'BUS102', N'Tổng kết', 5.8),
('221A240119', 'BUS102', N'Tổng kết', 6.4),
('221A370315', 'BUS102', N'Tổng kết', 5.7),
('221A370618', 'BUS102', N'Tổng kết', 5.9),
('231A010057', 'BUS102', N'Tổng kết', 7.5),
('231A010298', 'BUS102', N'Tổng kết', 5.6),
('231A010317', 'BUS102', N'Tổng kết', 9.5),
('231A010627', 'BUS102', N'Tổng kết', 5.8),
('231A010628', 'BUS102', N'Tổng kết', 7.0),
('231A010702', 'BUS102', N'Tổng kết', 9.9),
('231A010947', 'BUS102', N'Tổng kết', 8.2),
('231A010949', 'BUS102', N'Tổng kết', 6.4),
('231A010959', 'BUS102', N'Tổng kết', 6.7),
('231A011005', 'BUS102', N'Tổng kết', 7.8),
('231A080006', 'BUS102', N'Tổng kết', 9.1),
('231A080030', 'BUS102', N'Tổng kết', 7.8),
('231A080160', 'BUS102', N'Tổng kết', 6.6),
('231A080398', 'BUS102', N'Tổng kết', 7.9),
('231A090009', 'BUS102', N'Tổng kết', 9.4),
('231A090045', 'BUS102', N'Tổng kết', 9.7),
('231a100367', 'BUS102', N'Tổng kết', 9.7),
('231A100369', 'BUS102', N'Tổng kết', 9.5),
('231A140432', 'BUS102', N'Tổng kết', 6.4),
('231A170256', 'BUS102', N'Tổng kết', 7.5),
('231A170390', 'BUS102', N'Tổng kết', 7.4),
('231A180017', 'BUS102', N'Tổng kết', 7.3),
('231A180019', 'BUS102', N'Tổng kết', 6.9),
('231A180023', 'BUS102', N'Tổng kết', 8.5),
('231A180035', 'BUS102', N'Tổng kết', 7.4),
('231A210021', 'BUS102', N'Tổng kết', 6.5),
('231A210144', 'BUS102', N'Tổng kết', 6.9),
('231a210870', 'BUS102', N'Tổng kết', 6.1),
('231A210993', 'BUS102', N'Tổng kết', 9.0),
('231A230004', 'BUS102', N'Tổng kết', 9.7),
('231A230037', 'BUS102', N'Tổng kết', 8.4),
('231A240004', 'BUS102', N'Tổng kết', 7.1),
('231A240141', 'BUS102', N'Tổng kết', 6.6),
('231A370810', 'BUS102', N'Tổng kết', 6.1),
('231A371495', 'BUS102', N'Tổng kết', 7.6),
('241A010591', 'BUS102', N'Tổng kết', 8.9),
('241A020009', 'BUS102', N'Tổng kết', 5.9),
('241A020074', 'BUS102', N'Tổng kết', 9.5),
('241A020079', 'BUS102', N'Tổng kết', 6.2),
('241A030465', 'BUS102', N'Tổng kết', 8.5),
('241A040334', 'BUS102', N'Tổng kết', 6.5),
('241A040335', 'BUS102', N'Tổng kết', 8.7),
('241A070298', 'BUS102', N'Tổng kết', 10.0),
('241A070424', 'BUS102', N'Tổng kết', 7.3),
('241A080107', 'BUS102', N'Tổng kết', 7.4),
('241A080190', 'BUS102', N'Tổng kết', 7.1),
('241A080203', 'BUS102', N'Tổng kết', 5.9),
('241A080270', 'BUS102', N'Tổng kết', 7.1),
('241A080287', 'BUS102', N'Tổng kết', 7.0),
('241A080315', 'BUS102', N'Tổng kết', 7.6),
('241A100029', 'BUS102', N'Tổng kết', 8.7),
('241A100354', 'BUS102', N'Tổng kết', 7.2),
('241A140155', 'BUS102', N'Tổng kết', 7.8),
('241A170043', 'BUS102', N'Tổng kết', 6.8),
('241A170416', 'BUS102', N'Tổng kết', 9.8),
('241A170420', 'BUS102', N'Tổng kết', 6.0),
('241A170421', 'BUS102', N'Tổng kết', 9.6),
('241A170426', 'BUS102', N'Tổng kết', 6.5),
('241A170857', 'BUS102', N'Tổng kết', 9.4),
('241A211289', 'BUS102', N'Tổng kết', 5.9),
('241A230041', 'BUS102', N'Tổng kết', 6.7),
('241A230089', 'BUS102', N'Tổng kết', 9.6),
('241A230164', 'BUS102', N'Tổng kết', 6.3),
('241A230429', 'BUS102', N'Tổng kết', 8.9),
('241A230544', 'BUS102', N'Tổng kết', 9.2),
('241A240247', 'BUS102', N'Tổng kết', 9.3),
('241A370280', 'BUS102', N'Tổng kết', 8.5),
('241A371234', 'BUS102', N'Tổng kết', 9.8),
('241A371487', 'BUS102', N'Tổng kết', 7.3),
('251A110014', 'BUS102', N'Tổng kết', 7.9),
('251A110133', 'BUS102', N'Tổng kết', 7.8),
('251A110154', 'BUS102', N'Tổng kết', 7.7),
('251A110162', 'BUS102', N'Tổng kết', 7.0),
('251A110192', 'BUS102', N'Tổng kết', 6.8),
('251A110206', 'BUS102', N'Tổng kết', 9.1),
('251A110233', 'BUS102', N'Tổng kết', 6.3),
('251A110247', 'BUS102', N'Tổng kết', 9.5),
('251A110249', 'BUS102', N'Tổng kết', 6.7),
('251A130012', 'BUS102', N'Tổng kết', 5.6),
('251A130017', 'BUS102', N'Tổng kết', 5.9),
('251A150015', 'BUS102', N'Tổng kết', 6.7),
('251A150033', 'BUS102', N'Tổng kết', 8.2),
('251A210056', 'BUS102', N'Tổng kết', 6.5),
('251A210068', 'BUS102', N'Tổng kết', 6.7),
('251A210101', 'BUS102', N'Tổng kết', 6.0),
('251A210122', 'BUS102', N'Tổng kết', 5.6),
('251A210124', 'BUS102', N'Tổng kết', 10.0),
('251A210129', 'BUS102', N'Tổng kết', 7.4),
('251A210136', 'BUS102', N'Tổng kết', 9.6),
('251A210153', 'BUS102', N'Tổng kết', 8.3),
('251A210161', 'BUS102', N'Tổng kết', 5.7),
('251A210195', 'BUS102', N'Tổng kết', 8.7),
('251A210212', 'BUS102', N'Tổng kết', 9.7),
('251A210228', 'BUS102', N'Tổng kết', 9.9),
('251A210241', 'BUS102', N'Tổng kết', 6.7),
('251A210242', 'BUS102', N'Tổng kết', 6.3),
('251A210246', 'BUS102', N'Tổng kết', 9.7),
('251A210323', 'BUS102', N'Tổng kết', 8.3),
('251A210350', 'BUS102', N'Tổng kết', 7.9),
('251A210382', 'BUS102', N'Tổng kết', 6.4),
('251A210464', 'BUS102', N'Tổng kết', 7.5),
('251A210476', 'BUS102', N'Tổng kết', 8.5),
('251A210536', 'BUS102', N'Tổng kết', 6.7),
('251A210538', 'BUS102', N'Tổng kết', 9.1),
('251A210558', 'BUS102', N'Tổng kết', 10.0),
('251A210578', 'BUS102', N'Tổng kết', 5.7),
('251A210580', 'BUS102', N'Tổng kết', 5.6),
('251A210604', 'BUS102', N'Tổng kết', 7.8),
('251A210618', 'BUS102', N'Tổng kết', 9.9),
('251A210632', 'BUS102', N'Tổng kết', 7.8),
('251A210638', 'BUS102', N'Tổng kết', 6.6),
('251A210639', 'BUS102', N'Tổng kết', 7.5),
('251A210647', 'BUS102', N'Tổng kết', 8.5),
('251A210693', 'BUS102', N'Tổng kết', 8.4),
('251A210768', 'BUS102', N'Tổng kết', 8.5),
('251A210784', 'BUS102', N'Tổng kết', 8.0),
('251A210833', 'BUS102', N'Tổng kết', 9.5),
('251A210835', 'BUS102', N'Tổng kết', 9.9),
('251A210836', 'BUS102', N'Tổng kết', 6.9),
('251A210850', 'BUS102', N'Tổng kết', 6.5),
('251A210880', 'BUS102', N'Tổng kết', 6.5),
('251A210987', 'BUS102', N'Tổng kết', 6.4),
('251A211031', 'BUS102', N'Tổng kết', 9.5),
('251A211044', 'BUS102', N'Tổng kết', 8.8),
('251A211050', 'BUS102', N'Tổng kết', 6.1),
('251A211075', 'BUS102', N'Tổng kết', 10.0),
('251A211112', 'BUS102', N'Tổng kết', 9.9),
('251A211114', 'BUS102', N'Tổng kết', 9.3),
('251A211123', 'BUS102', N'Tổng kết', 5.6),
('251A211129', 'BUS102', N'Tổng kết', 8.3),
('251A211154', 'BUS102', N'Tổng kết', 9.5),
('251A211155', 'BUS102', N'Tổng kết', 7.4),
('251A211172', 'BUS102', N'Tổng kết', 5.7),
('251A211176', 'BUS102', N'Tổng kết', 8.5),
('251A211214', 'BUS102', N'Tổng kết', 7.2),
('251A211266', 'BUS102', N'Tổng kết', 7.8),
('251A211293', 'BUS102', N'Tổng kết', 9.9),
('251a211302', 'BUS102', N'Tổng kết', 8.2),
('251A211308', 'BUS102', N'Tổng kết', 8.6),
('251A211332', 'BUS102', N'Tổng kết', 5.7),
('251A211348', 'BUS102', N'Tổng kết', 6.3),
('251A211351', 'BUS102', N'Tổng kết', 6.7),
('251A211353', 'BUS102', N'Tổng kết', 5.5),
('251A211391', 'BUS102', N'Tổng kết', 7.1),
('251A370556', 'BUS102', N'Tổng kết', 7.0),
('251A480009', 'BUS102', N'Tổng kết', 9.9);

INSERT INTO dbo.DiemThanhPhan (MaSV, MaMon, LoaiDiem, Diem)
SELECT tmp.MaSV, tmp.MaMon, tmp.LoaiDiem, tmp.Diem
FROM #Diem_Moi tmp
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.DiemThanhPhan d
    WHERE d.MaSV = tmp.MaSV AND d.MaMon = tmp.MaMon AND d.LoaiDiem = tmp.LoaiDiem
);

DROP TABLE #Diem_Moi;
GO

------------------------------------------------------------
-- BUOC 6: KIEM TRA KET QUA
------------------------------------------------------------
SELECT 'SinhVien' AS Bang, COUNT(*) AS SoDong FROM dbo.SinhVien
UNION ALL SELECT 'MonHoc', COUNT(*) FROM dbo.MonHoc
UNION ALL SELECT 'LopHoc', COUNT(*) FROM dbo.LopHoc
UNION ALL SELECT 'GiangVien', COUNT(*) FROM dbo.GiangVien
UNION ALL SELECT 'DiemThanhPhan', COUNT(*) FROM dbo.DiemThanhPhan;
GO

-- Kiem tra: co sinh vien nao trong lop BUS102_L01 ma con thieu diem hoac duoi trung binh khong
SELECT sv.MaSV, sv.TenSV, dtp.Diem
FROM dbo.SinhVien sv
LEFT JOIN dbo.DiemThanhPhan dtp
    ON dtp.MaSV = sv.MaSV AND dtp.MaMon = 'BUS102'
WHERE sv.MaLop = 'BUS102_L01'
  AND (dtp.Diem IS NULL OR dtp.Diem < 5.0)
ORDER BY sv.MaSV;
-- Ket qua rong (0 dong) = tat ca 150 SV deu co diem va tren trung binh.
GO