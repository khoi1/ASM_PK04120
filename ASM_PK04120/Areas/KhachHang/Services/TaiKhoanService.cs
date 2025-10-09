using ASM_PK04120.Areas.KhachHang.Models;
using ASM_PK04120.Models;
using ASM_PK04120.Data;
using Microsoft.EntityFrameworkCore;
namespace ASM_PK04120.Areas.KhachHang.Services
{
    public class TaiKhoanService : ITaiKhoanService
    {
        private readonly AppDbContext _context;
        public TaiKhoanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<NguoiDungModel?> DangNhapAsync(DangNhapViewModel viewModel)
        {
            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.TaiKhoan == viewModel.TaiKhoanOrEmail || u.Email == viewModel.TaiKhoanOrEmail);

            if (user != null && user.MatKhau == viewModel.MatKhau)
            {
                return user; // Đăng nhập thành công, trả về thông tin người dùng
            }

            return null; // Đăng nhập thất bại
        }

        public async Task<string?> DangKyAsync(DangKyViewModel viewModel)
        {
            var KT = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.Email == viewModel.Email || u.TaiKhoan == viewModel.TaiKhoan);

            if (KT != null)
            {
                return "Tài khoản hoặc Email đã tồn tại."; // Trả về thông báo lỗi
            }

            var nguoiDung = new NguoiDungModel
            {
                TaiKhoan = viewModel.TaiKhoan,
                HoTen = viewModel.HoTen,
                Email = viewModel.Email,
                MatKhau = viewModel.MatKhau, // LƯU Ý: Mật khẩu không được mã hóa
                NgayTao = DateTime.Now,
                VaiTro = "Khách hàng",
                TrangThai = 1
            };

            _context.NguoiDungs.Add(nguoiDung);
            await _context.SaveChangesAsync();

            return null; // Đăng ký thành công
        }
    }
}
