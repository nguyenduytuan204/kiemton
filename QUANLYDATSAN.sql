/*
Created		10/07/2026
Modified		10/07/2026
Project		
Model			
Company		
Author		
Version		
Database		MS SQL 2005 
*/
CREATE DATABASE QUANLYDATSAN
GO
USE QUANLYDATSAN
GO
Create table [LOAISAN]
(
	[MALS] Char(10) NOT NULL,
	[TENLS] Nvarchar(50) NULL,
Primary Key ([MALS])
) 
go

Create table [SAN]
(
	[MASAN] Char(10) NOT NULL,
	[TENSAN] Nvarchar(50) NULL,
	[VITRI] Nvarchar(50) NULL,
	[TRANGTHAI] Bit NULL,
	[MALS] Char(10) NOT NULL,
Primary Key ([MASAN])
) 
go

Create table [PHIEUDATSAN]
(
	[SOPHIEU] Char(10) NOT NULL,
	[NGAYDAT] Date NULL,
	[SODT] Char(10) NULL,
	[TENKH] Nvarchar(100) NULL,
Primary Key ([SOPHIEU])
) 
go

Create table [THONGTINDATSAN]
(
	[SOPHIEU] Char(10) NOT NULL,
	[MASAN] Char(10) NOT NULL,
	[GIOBD] Time NOT NULL,
	[GIOKT] Time NOT NULL,
	[DONGIATHUE] INT NOT NULL,
Primary Key ([MASAN],[SOPHIEU])
) 
go

Alter table [SAN] add  foreign key([MALS]) references [LOAISAN] ([MALS])  on update no action on delete no action 
go
Alter table [THONGTINDATSAN] add  foreign key([MASAN]) references [SAN] ([MASAN])  on update no action on delete no action 
go
Alter table [THONGTINDATSAN] add  foreign key([SOPHIEU]) references [PHIEUDATSAN] ([SOPHIEU])  on update no action on delete no action 
go
INSERT INTO LOAISAN (MALS, TENLS) VALUES
('CL', N'Cầu lông'),
('BD', N'Bóng đá'),
('PB', N'Pickleball'),
('BR', N'Bóng rổ'),
('TN', N'Quần vợt');
GO
SELECT * FROM LOAISAN
go
INSERT INTO SAN (MASAN, TENSAN, VITRI, TRANGTHAI, MALS) VALUES
('CL001', N'Sân Cầu Lông Số 1', N'Nhà thi đấu A', 'TRUE', 'CL'),
('CL002', N'Sân Cầu Lông Số 2', N'Nhà thi đấu A', 'FALSE', 'CL'),
('BD001', N'Sân Bóng Đá Nhân Tạo 5 Người', N'Khu sân cỏ ngoài trời', 'FALSE', 'BD'),
('BD002', N'Sân Bóng Đá Nhân Tạo 7 Người', N'Khu sân cỏ ngoài trời', 'TRUE', 'BD'),
('PB001', N'Sân Pickleball Indoor 1', N'Khu phức hợp B', 'FALSE', 'PB'),
('PB002', N'Sân Pickleball Outdoor 2', N'Khu phức hợp B', 'FALSE', 'PB'),
('BR001', N'Sân Bóng Rổ Trong Nhà', N'Nhà thi đấu B', 'TRUE', 'PB'),
('BR002', N'Sân Bóng Rổ Ngoài Trời', N'Công viên trung tâm', 'FALSE', 'PB'),
('TN001', N'Sân Tennis Đất Nện số 1', N'Khu Tennis phía Tây', 'FALSE', 'TN'),
('TN002', N'Sân Tennis cứng số 1', N'Khu Tennis phía Tây', 'TRUE', 'TN');
GO
SELECT * FROM SAN
GO
INSERT INTO PHIEUDATSAN (SOPHIEU, NGAYDAT, SODT, TENKH) VALUES
('P001', '2026-03-01', '0901234567', N'Nguyễn Văn Anh'),
('P002', '2026-03-01', '0912345678', N'Trần Thị Bình'),
('P003', '2026-03-02', '0983456789', N'Lê Hoàng Cường'),
('P004', '2026-03-02', '0934567890', N'Phạm Minh Đức'),
('P005', '2026-03-03', '0975678901', N'Hoàng Lan Hương'),
('P006', '2026-03-04', '0966789012', N'Võ Văn Khánh'),
('P007', '2026-03-04', '0947890123', N'Đặng Thị Linh'),
('P008', '2026-03-05', '0928901234', N'Bùi Quang Nam'),
('P009', '2026-03-05', '0899012345', N'Phan Thanh Sơn'),
('P010', '2026-03-06', '0880123456', N'Đỗ Thúy Vy');
go
SELECT * FROM PHIEUDATSAN
GO
INSERT INTO THONGTINDATSAN (SOPHIEU, MASAN, GIOBD, GIOKT, DONGIATHUE) VALUES
-- Phiếu P001 đặt 2 sân khác nhau
('P001', 'CL001', '08:00:00', '10:00:00', 80000),
('P001', 'CL002', '10:00:00', '11:00:00', 90000),

-- Phiếu P002 đặt sân bóng đá
('P002', 'BD001', '17:00:00', '19:00:00', 250000),

-- Phiếu P003 đặt sân bóng đá và cầu lông
('P003', 'BD002', '18:00:00', '20:00:00', 300000),
('P003', 'CL001', '20:00:00', '22:00:00', 100000),

-- Phiếu P004 đặt sân Pickleball
('P004', 'PB001', '06:00:00', '08:00:00', 150000),
('P004', 'PB002', '06:00:00', '08:00:00', 120000),

-- Phiếu P005 đặt sân bóng rổ
('P005', 'BR001', '15:00:00', '17:00:00', 200000),

-- Phiếu P006 đặt sân tennis
('P006', 'TN001', '16:00:00', '18:00:00', 180000),

-- Phiếu P007 đặt các khung giờ khác nhau của sân Pickleball
('P007', 'PB001', '16:00:00', '18:00:00', 150000),
('P007', 'PB002', '18:00:00', '20:00:00', 150000),

-- Phiếu P008 đặt sân bóng đá và bóng rổ
('P008', 'BR001', '19:00:00', '21:00:00', 250000),
('P008', 'BR002', '17:00:00', '19:00:00', 150000),

-- Phiếu P009 đặt chuỗi sân cầu lông
('P009', 'CL001', '14:00:00', '16:00:00', 80000),
('P009', 'CL002', '14:00:00', '16:00:00', 80000),

-- Phiếu P010 đặt nhiều loại sân vào cuối ngày
('P010', 'BD001', '20:00:00', '22:00:00', 300000),
('P010', 'BD002', '20:00:00', '22:00:00', 150000),
('P010', 'CL001', '19:00:00', '21:00:00', 200000),
('P010', 'PB001', '20:00:00', '22:00:00', 180000);
go
SELECT * FROM THONGTINDATSAN order by SOPHIEU