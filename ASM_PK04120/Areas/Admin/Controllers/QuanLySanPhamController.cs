using Microsoft.AspNetCore.Mvc;
using ASM_PK04120.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASM_PK04120.Models;

namespace ASM_PK04120.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLySanPhamController : AdminBaseController
    {
        private readonly IQuanLySanPhamService _quanLySanPhamService;
        public QuanLySanPhamController(IQuanLySanPhamService quanLySanPhamService)
        {
            _quanLySanPhamService = quanLySanPhamService;
        }

        public async Task<IActionResult> Index(string? tuKhoa, string? sapXep, int page = 1)
        {
            if (string.IsNullOrEmpty(sapXep))
            {
                ViewBag.sapXep = sapXep;
            }
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                ViewBag.TuKhoa = tuKhoa;
            }
            var viewModel = await _quanLySanPhamService.LayDanhSachSanPham(tuKhoa, sapXep, page);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPhamModel sanPham, IFormFile? hinhAnh, IFormFile? moTaAnh)
        {
            var ketQua = await _quanLySanPhamService.ThemSanPham(sanPham, hinhAnh, moTaAnh);
            if (ketQua)
            {
                TempData["ThongBao"] = "Thêm sản phẩm mới thành công!";
                TempData["LoaiThongBao"] = "success";
            }
            else
            {
                TempData["ThongBao"] = "Thêm sản phẩm thất bại.";
                TempData["LoaiThongBao"] = "error";
            }
            return RedirectToAction(nameof(Index)); // Luôn tải lại trang Index
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SanPhamModel sanPham, IFormFile? hinhAnhMoi, IFormFile? moTaAnhMoi)
        {
            var ketQua = await _quanLySanPhamService.CapNhatSanPham(sanPham, hinhAnhMoi, moTaAnhMoi);
            if (ketQua)
            {
                TempData["ThongBao"] = "Cập nhật sản phẩm thành công!";
                TempData["LoaiThongBao"] = "success";
            }
            else
            {
                TempData["ThongBao"] = "Cập nhật sản phẩm thất bại.";
                TempData["LoaiThongBao"] = "error";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int maSanPham)
        {
            if (maSanPham <= 0)
            {
                TempData["ThongBao"] = "Mã sản phẩm không hợp lệ.";
                TempData["LoaiThongBao"] = "error";
                return RedirectToAction(nameof(Index));
            }
            var ketQua = await _quanLySanPhamService.XoaSanPham(maSanPham);
            if (ketQua)
            {
                TempData["ThongBao"] = "Xóa sản phẩm thành công!";
                TempData["LoaiThongBao"] = "warning";
            }
            else
            {
                TempData["ThongBao"] = "Xóa sản phẩm thất bại.";
                TempData["LoaiThongBao"] = "error";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
