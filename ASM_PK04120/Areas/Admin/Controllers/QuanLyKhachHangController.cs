using ASM_PK04120.Areas.Admin.Services;
using ASM_PK04120.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASM_PK04120.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyKhachHangController : Controller
    {
        private readonly IQuanLyKhachHangService _quanLyKhachHangService;

        public QuanLyKhachHangController(IQuanLyKhachHangService quanLyKhachHangService)
        {
            _quanLyKhachHangService = quanLyKhachHangService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var danhSachKhachHang = await _quanLyKhachHangService.LayDanhSachKhachHang(page, pageSize);
            return View(danhSachKhachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NguoiDungModel nguoiDung)
        {
            if (ModelState.IsValid)
            {
                var result = await _quanLyKhachHangService.ThemKhachHang(nguoiDung);
                if (result)
                {
                    TempData["ThongBao"] = "Thêm khách hàng mới thành công!";
                    TempData["LoaiThongBao"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Thêm lỗi vào ModelState để hiển thị trên form
                    ModelState.AddModelError(string.Empty, "Thêm khách hàng thất bại. Tài khoản hoặc Email có thể đã tồn tại.");
                }
            }
            // Nếu thất bại, quay lại trang Index. Modal sẽ không tự mở lại, nhưng TempData có thể dùng để báo lỗi.
            TempData["ThongBao"] = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại.";
            TempData["LoaiThongBao"] = "error";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int MaNguoiDung, NguoiDungModel nguoiDung)
        {
            if (MaNguoiDung != nguoiDung.MaNguoiDung)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _quanLyKhachHangService.CapNhatKhachHang(nguoiDung);
                if (result)
                {
                    TempData["ThongBao"] = "Cập nhật thông tin khách hàng thành công!";
                    TempData["LoaiThongBao"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ThongBao"] = "Cập nhật thất bại. Vui lòng thử lại.";
                    TempData["LoaiThongBao"] = "error";
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}