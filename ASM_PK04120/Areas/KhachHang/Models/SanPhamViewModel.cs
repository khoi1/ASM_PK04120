using ASM_PK04120.Models;
namespace ASM_PK04120.Areas.KhachHang.Models
{
    public class SanPhamViewModel
    {
        public List<SanPhamModel> DanhSachSanPham { get; set; } = new List<SanPhamModel>();
        public List<DanhMucModel> DanhSachDanhMuc { get; set; } = new List<DanhMucModel>();
        public int TrangHienTai { get; set; }
        public int TongSoTrang { get; set; }
        public int? MaDanhMucHienTai { get; set; }
        public string? SapXepHienTai { get; set; }
        public string? PhamViGiaHienTai { get; set; }
    }
}
