using ASM_PK04120.Areas.Admin.Models;
using ASM_PK04120.Models;
namespace ASM_PK04120.Areas.Admin.Services
{
    public interface IQuanLyKhachHangService
    {
        Task<KhachHangViewModel> LayDanhSachKhachHang(int page, int pageSize);
        Task<NguoiDungModel?> LayKhachHangTheoId(int maNguoiDung);
        Task<bool> ThemKhachHang(NguoiDungModel khachHang);
        Task<bool> CapNhatKhachHang(NguoiDungModel khachHang);
    }
}
