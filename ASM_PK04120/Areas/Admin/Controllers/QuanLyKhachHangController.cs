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

        public IActionResult Create()
        {
            // Chỉ cần trả về View rỗng để hiển thị form
            return View();
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
                    TempData["ThongBao"] = "Thêm khách hàng thất bại. Vui lòng thử lại.";
                    TempData["LoaiThongBao"] = "error";
                }
            }
            // Nếu có lỗi validation, quay lại form Create với dữ liệu người dùng đã nhập
            return View(nguoiDung);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var khachHang = await _quanLyKhachHangService.LayKhachHangTheoId(id);
            if (khachHang == null)
            {
                TempData["ThongBao"] = "Không tìm thấy khách hàng.";
                TempData["LoaiThongBao"] = "error";
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NguoiDungModel nguoiDung)
        {
            if (id != nguoiDung.MaNguoiDung)
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
            // Nếu có lỗi, quay lại form Edit với dữ liệu người dùng đã nhập
            return View(nguoiDung);
        }
    }
}