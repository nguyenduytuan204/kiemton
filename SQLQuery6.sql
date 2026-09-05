-- CHUYÊN CẦN
INSERT INTO DiemThanhPhan (MaSV, MaMon, LoaiDiem, Diem)
SELECT 
    sv.MaSV,
    'BUS102',
    N'Chuyên cần',
    ROUND(6 + RAND(CHECKSUM(NEWID())) * 4, 1)
FROM SinhVien sv
WHERE sv.MaLop = 'BUS102_L01'
AND NOT EXISTS (
    SELECT 1 FROM DiemThanhPhan d 
    WHERE d.MaSV = sv.MaSV AND d.MaMon = 'BUS102' AND d.LoaiDiem = N'Chuyên cần'
);

-- GIỮA KỲ
INSERT INTO DiemThanhPhan (MaSV, MaMon, LoaiDiem, Diem)
SELECT 
    sv.MaSV,
    'BUS102',
    N'Giữa kỳ',
    ROUND(5.5 + RAND(CHECKSUM(NEWID())) * 4.5, 1)
FROM SinhVien sv
WHERE sv.MaLop = 'BUS102_L01'
AND NOT EXISTS (
    SELECT 1 FROM DiemThanhPhan d 
    WHERE d.MaSV = sv.MaSV AND d.MaMon = 'BUS102' AND d.LoaiDiem = N'Giữa kỳ'
);

-- CUỐI KỲ
INSERT INTO DiemThanhPhan (MaSV, MaMon, LoaiDiem, Diem)
SELECT 
    sv.MaSV,
    'BUS102',
    N'Cuối kỳ',
    ROUND(5 + RAND(CHECKSUM(NEWID())) * 5, 1)
FROM SinhVien sv
WHERE sv.MaLop = 'BUS102_L01'
AND NOT EXISTS (
    SELECT 1 FROM DiemThanhPhan d 
    WHERE d.MaSV = sv.MaSV AND d.MaMon = 'BUS102' AND d.LoaiDiem = N'Cuối kỳ'
);