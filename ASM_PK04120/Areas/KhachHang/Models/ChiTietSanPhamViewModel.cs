using ASM_PK04120.Models;

namespace ASM_PK04120.Areas.KhachHang.Models
{
    public class ChiTietSanPhamViewModel
    {
        public required SanPhamModel SanPham { get; set; }
        public List<SanPhamModel>? SanPhamTuongTu { get; set; }
    }
}
