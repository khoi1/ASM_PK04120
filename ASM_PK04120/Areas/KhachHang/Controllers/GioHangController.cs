using ASM_PK04120.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASM_PK04120.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class GioHangController : Controller
    {
        private readonly IGioHangService _gioHangService;
        public GioHangController(IGioHangService gioHangService)
        {
            _gioHangService = gioHangService;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy mã người dùng từ Session
            var maNguoiDungStr = HttpContext.Session.GetString("MaNguoiDung");

            // KIỂM TRA ĐĂNG NHẬP
            if (string.IsNullOrEmpty(maNguoiDungStr))
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "TaiKhoan", new { area = "KhachHang" });
            }

            int maNguoiDung = int.Parse(maNguoiDungStr);

            // Lấy giỏ hàng từ service và truyền vào View
            var gioHang = await _gioHangService.LayGioHang(maNguoiDung);
            return View(gioHang);
        }

        [HttpPost]
        public async Task<IActionResult> ThemVaoGio(int maSanPham, int soLuong = 1)
        {
            var maNguoiDungStr = HttpContext.Session.GetString("MaNguoiDung");
            if (string.IsNullOrEmpty(maNguoiDungStr))
            {
                return RedirectToAction("DangNhap", "TaiKhoan", new { area = "KhachHang" });
            }

            int maNguoiDung = int.Parse(maNguoiDungStr);

            await _gioHangService.ThemVaoGio(maNguoiDung, maSanPham, soLuong);

            // Chuyển hướng về trang giỏ hàng sau khi thêm
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> XoaKhoiGio(int maGioHang)
        {
            var maNguoiDungStr = HttpContext.Session.GetString("MaNguoiDung");
            if (string.IsNullOrEmpty(maNguoiDungStr))
            {
                return RedirectToAction("DangNhap", "TaiKhoan", new { area = "KhachHang" });
            }
            // Bạn có thể thêm một bước kiểm tra xem `maGioHang` này có thực sự thuộc về người dùng đang đăng nhập không để tăng bảo mật

            await _gioHangService.XoaKhoiGio(maGioHang);
            return RedirectToAction("Index");
        }
    }
}
