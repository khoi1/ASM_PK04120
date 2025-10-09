namespace ASM_PK04120.Areas.KhachHang.Models
{
    public class SanPhamBanChayViewModel
    {
        public int MaSanPham { get; set; }
        public required string TenSanPham { get; set; }
        public string? HinhAnh { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongDaBan { get; set; } // Thêm thông tin số lượng đã bán
    }
}
