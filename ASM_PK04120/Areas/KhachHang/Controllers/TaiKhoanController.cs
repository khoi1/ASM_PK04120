using Microsoft.AspNetCore.Mvc;
using ASM_PK04120.Data;
using ASM_PK04120.Models;
using Microsoft.EntityFrameworkCore;
using ASM_PK04120.Areas.KhachHang.Models;
using ASM_PK04120.Areas.KhachHang.Services;

namespace ASM_PK04120.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TaiKhoanController : Controller
    {
        private readonly ITaiKhoanService _taiKhoanService;

        public TaiKhoanController(ITaiKhoanService taiKhoanService)
        {
            _taiKhoanService = taiKhoanService;
        }

        [HttpGet]
        public IActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(DangNhapViewModel viewModel)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                   return View(viewModel);
                }
                var tk = await _taiKhoanService.DangNhapAsync(viewModel);
                if (tk != null)
                {
                    // Lưu thông tin người dùng vào session
                    HttpContext.Session.SetString("TaiKhoan", tk.TaiKhoan);
                    HttpContext.Session.SetString("HoTen", tk.HoTen);
                    HttpContext.Session.SetInt32("MaNguoiDung", tk.MaNguoiDung);
                    HttpContext.Session.SetString("VaiTro", tk.VaiTro);

                    TempData["ThongBao"] = "Đăng nhập thành công!";
                    TempData["LoaiThongBao"] = "success";
                    return RedirectToAction("Index", "TrangChu");
                }

                // Nếu service trả về null, nghĩa là đăng nhập thất bại
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không chính xác.");
                return View(viewModel);
            }
            catch (Exception)
            {
                // Ghi log lỗi nếu cần thiết
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangKy(DangKyViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                // Gọi service để xử lý logic đăng ký
                var kt = await _taiKhoanService.DangKyAsync(viewModel);

                if (kt == null) // Nếu không có lỗi trả về
                {
                    // Đăng ký thành công
                    TempData["ThongBao"] = "Đăng ký tài khoản thành công! Vui lòng đăng nhập.";
                    TempData["LoaiThongBao"] = "success";
                    return RedirectToAction("DangNhap");
                }

                // Nếu có lỗi, hiển thị lỗi đó
                ModelState.AddModelError(string.Empty, kt);
                return View(viewModel);
            }
            catch (Exception)
            {
                // Ghi log lỗi nếu cần thiết
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại.");
                return View(viewModel);
            }
        }

        public IActionResult DangXuat()
        {
            // Xóa tất cả các dữ liệu đã lưu trong Session
            HttpContext.Session.Clear();

            // Thêm một thông báo tạm thời
            TempData["ThongBao"] = "Bạn đã đăng xuất thành công.";
            TempData["LoaiThongBao"] = "info"; // Thông báo màu xanh dương

            // Chuyển hướng người dùng về trang chủ
            return RedirectToAction("Index", "TrangChu", new { area = "KhachHang" });
        }
    }
}
