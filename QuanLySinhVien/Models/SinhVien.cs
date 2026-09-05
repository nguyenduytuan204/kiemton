using System;

namespace QuanLySinhVien.Models
{
    public class SinhVien
    {
        public string MaSV { get; set; } = string.Empty;
        public string TenSV { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; } = DateTime.Now;
        public string GioiTinh { get; set; } = "Nam";
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string MaLop { get; set; } = string.Empty;
    }
}
