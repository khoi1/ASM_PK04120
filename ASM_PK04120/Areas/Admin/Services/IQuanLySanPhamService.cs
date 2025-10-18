using ASM_PK04120.Areas.Admin.Models;
using ASM_PK04120.Models;
namespace ASM_PK04120.Areas.Admin.Services
{
    public interface IQuanLySanPhamService
    {
        Task<DsSanPhamViewModel> LayDanhSachSanPham(string? tuKhoa, string? sapXep, int trang = 1, int kichThuocTrang = 10);

        Task<bool> ThemSanPham(SanPhamModel sanPham, IFormFile? hinhAnh, IFormFile? moTaAnh);

        Task<bool> CapNhatSanPham(SanPhamModel sanPham, IFormFile? hinhAnhMoi, IFormFile? moTaAnhMoi);

        Task<bool> XoaSanPham(int maSanPham);
    }
}
