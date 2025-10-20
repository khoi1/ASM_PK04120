using ASM_PK04120.Areas.Admin.Models;

namespace ASM_PK04120.Areas.Admin.Services
{
    public interface IThongKeService
    {
        ThongKeTongQuan LayThongKeTongQuan();

        List<TopSanPham> LayTopSanPhamWeek();
        List<TopSanPham> LayTopSanPhamMonth();
        List<TopSanPham> LayTopSanPhamYear();

        List<ThongKeDanhMuc> LayDoanhThuDanhMuc();
        List<ThongKeDanhMuc> LaySoLuongBanDanhMuc();

        List<DoanhThuNgay> LayDoanhThuTheoNgay();

        List<DoanhThuTheoThoiGian> LayDoanhThuTheoThang();
        List<DoanhThuTheoThoiGian> LayDoanhThuTheoNam();
    }
}
