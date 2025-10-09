using ASM_PK04120.Models;
using ASM_PK04120.Areas.KhachHang.Models;
using ASM_PK04120.Data;
using Microsoft.EntityFrameworkCore;
namespace ASM_PK04120.Areas.KhachHang.Services
{
    public class TrangChuService : ITrangChuService
    {
        private readonly ISanPhamService _sanPhamService;

        public TrangChuService(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        public async Task<TrangChuViewModel> LayDuLieuTrangChuAsync()
        {
            // Gọi sang SanPhamService để lấy dữ liệu
            var spBanChay = await _sanPhamService.LaySanPhamBanChayAsync(8);

            // Đóng gói dữ liệu vào ViewModel
            var viewModel = new TrangChuViewModel
            {
                SanPhamBanChay = spBanChay
            };

            return viewModel;
        }
    }
}
