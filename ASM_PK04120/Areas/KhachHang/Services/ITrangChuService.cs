using ASM_PK04120.Areas.KhachHang.Models;

namespace ASM_PK04120.Areas.KhachHang.Services
{
    public interface ITrangChuService
    {
        Task<TrangChuViewModel> LayDuLieuTrangChuAsync();
    }
}
