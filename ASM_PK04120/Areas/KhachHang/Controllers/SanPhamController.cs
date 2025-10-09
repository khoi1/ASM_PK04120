using ASM_PK04120.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASM_PK04120.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class SanPhamController : Controller
    {
        private readonly ISanPhamService _sanPhamService;
        public SanPhamController(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        public async Task<IActionResult> SanPham(int? maDanhMuc, string? phamViGia, string? thuTuSapXep, int trang = 1)
        {
            var sp = await _sanPhamService.LayDanhSachSanPhamAsync(maDanhMuc, phamViGia, thuTuSapXep, trang);
            return View(sp);
        }

        public async Task<IActionResult> ChiTietSanPham(int id)
        {
            var viewModel = await _sanPhamService.LayChiTietSanPhamAsync(id);
            if (viewModel == null)
            {
                return RedirectToAction("Index", "TrangChu", new { area = "KhachHang" });
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> TimKiem(string tuKhoa)
        {
            var ketQua = await _sanPhamService.TimKiemSanPhamAsync(tuKhoa);

            // Dùng ViewBag để truyền từ khóa tìm kiếm sang View
            ViewBag.TuKhoa = tuKhoa;

            return View("KetQuaTimKiem", ketQua);
        }
    }
}
