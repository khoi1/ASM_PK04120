using ASM_PK04120.Models;

namespace ASM_PK04120.Areas.Admin.Models
{
    public class SanPhamViewModel
    {
        public required SanPhamModel SanPham { get; set; }
        public required DanhMucModel DanhMuc { get; set; }
    }

    public class DsSanPhamViewModel
    {
        public List<SanPhamViewModel> DanhSachSanPham { get; set; } = new List<SanPhamViewModel>();
        public List<DanhMucModel> DanhSachDanhMuc { get; set; } = new List<DanhMucModel>();
        public int TrangHienTai { get; set; }
        public int TongSoTrang { get; set; }
        public string? TuKhoa { get; set; }
        public string? SapXep { get; set; }
    }
}