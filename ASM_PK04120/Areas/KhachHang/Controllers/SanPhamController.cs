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

        // Action này dùng để tải trang sản phẩm
        public async Task<IActionResult> SanPham(int? maDanhMuc, string? phamViGia, string? thuTuSapXep, int trang = 1)
        {
            var sp = await _sanPhamService.LayDanhSachSanPhamAsync(maDanhMuc, phamViGia, thuTuSapXep, trang);
            return View(sp);
        }

        // Action cho trang chi tiết sản phẩm
        public async Task<IActionResult> ChiTietSanPham(int id)
        {
            var viewModel = await _sanPhamService.LayChiTietSanPhamAsync(id);
            if (viewModel == null)
            {
                return RedirectToAction("Index", "TrangChu", new { area = "KhachHang" });
            }
            return View(viewModel);
        }

        // Action cho TÌM KIẾM AJAX TRÊN HEADER
        [HttpGet]
        public async Task<JsonResult> TimKiemSanPham(string tuKhoa)
        {
            var result = await _sanPhamService.TimKiemSanPhamAsync(tuKhoa);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> _FilterProductsPartial(int? maDanhMuc, string? phamViGia, string? thuTuSapXep, int trang = 1)
        {
            var sp = await _sanPhamService.LayDanhSachSanPhamAsync(maDanhMuc, phamViGia, thuTuSapXep, trang);
            return PartialView("_dsSanPhamPartial", sp);
        }
    }
}