using ASM_PK04120.Areas.KhachHang.Models;
using ASM_PK04120.Models;

namespace ASM_PK04120.Areas.KhachHang.Services
{
    public interface ITaiKhoanService
    {
        // Hàm đăng nhập, trả về NguoiDungModel nếu thành công, ngược lại trả về null
        Task<NguoiDungModel?> DangNhapAsync(DangNhapViewModel viewModel);

        // Hàm đăng ký, trả về thông báo lỗi nếu thất bại, ngược lại trả về null
        Task<string?> DangKyAsync(DangKyViewModel viewModel);
    }
}
