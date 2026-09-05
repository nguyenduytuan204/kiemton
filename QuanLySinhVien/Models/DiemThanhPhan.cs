namespace QuanLySinhVien.Models
{
    public class DiemThanhPhan
    {
        public int MaDiem { get; set; }
        public string MaSV { get; set; } = string.Empty;
        public string MaMon { get; set; } = string.Empty;
        public string LoaiDiem { get; set; } = "Chuyên cần";
        public double Diem { get; set; }
    }
}
