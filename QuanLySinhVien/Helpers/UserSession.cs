namespace QuanLySinhVien.Helpers
{
    public static class UserSession
    {
        public static string MaGV { get; set; } = string.Empty;
        public static string TenGV { get; set; } = string.Empty;
        public static string Username { get; set; } = string.Empty;
        public static string Quyen { get; set; } = string.Empty; // "Admin" hoặc "GiangVien"

        public static bool IsAdmin => Quyen.Equals("Admin", System.StringComparison.OrdinalIgnoreCase);

        public static void Clear()
        {
            MaGV = string.Empty;
            TenGV = string.Empty;
            Username = string.Empty;
            Quyen = string.Empty;
        }
    }
}
