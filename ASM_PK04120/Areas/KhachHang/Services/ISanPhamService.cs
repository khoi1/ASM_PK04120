using ASM_PK04120.Areas.KhachHang.Models;
using ASM_PK04120.Models;

namespace ASM_PK04120.Areas.KhachHang.Services
{
    public interface ISanPhamService
    {
        Task<List<SanPhamBanChayViewModel>> LaySanPhamBanChayAsync(int soLuong);
        Task<ChiTietSanPhamViewModel?> LayChiTietSanPhamAsync(int maSanPham);
        Task<SanPhamViewModel> LayDanhSachSanPhamAsync(int? maDanhMuc, string? phamViGia, string? thuTuSapXep, int soTrang);
        Task<List<SanPhamTimKiemViewModel>> TimKiemSanPhamAsync(string tuKhoa);
    }
}
