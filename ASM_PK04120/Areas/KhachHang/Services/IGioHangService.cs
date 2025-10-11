using ASM_PK04120.Models;

namespace ASM_PK04120.Areas.KhachHang.Services
{
    public interface IGioHangService
    {
        // Lấy giỏ hàng của một người dùng dựa vào mã người dùng
        Task<List<GioHangModel>> LayGioHang(int maNguoiDung);

        // Thêm một sản phẩm vào giỏ của người dùng
        Task ThemVaoGio(int maNguoiDung, int maSanPham, int soLuong);

        // Xóa một món hàng khỏi giỏ (dựa vào mã giỏ hàng)
        Task XoaKhoiGio(int maGioHang);
    }
}
