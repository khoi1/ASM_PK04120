using ASM_PK04120.Models;
namespace ASM_PK04120.Areas.Admin.Models
{
    public class ThongKeViewModel
    {
        public ThongKeTongQuan? TongQuan { get; set; }

        public List<TopSanPham>? TopSanPhamWeek { get; set; }
        public List<TopSanPham>? TopSanPhamMonth { get; set; }
        public List<TopSanPham>? TopSanPhamYear { get; set; }

        public List<ThongKeDanhMuc>? DoanhThuDanhMuc { get; set; }
        public List<ThongKeDanhMuc>? SoLuongBanDanhMuc { get; set; }

        public List<DoanhThuNgay>? DoanhThuNgays { get; set; }
        public List<DoanhThuTheoThoiGian>? DoanhThuThangs { get; set; }
        public List<DoanhThuTheoThoiGian>? DoanhThuNams { get; set; }
    }

    public class ThongKeTongQuan
    {
        public decimal DoanhThuThangNay { get; set; }

        public int DonHangThangNay { get; set; }

        public int KhachHangMoi { get; set; }

        public decimal GiaTriTrungBinh { get; set; }
    }

    public class TopSanPham
    {
        public string? TenSanPham { get; set; }
        public int SoLuongBan { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class ThongKeDanhMuc
    {
        public string? TenDanhMuc { get; set; }
        public decimal GiaTri { get; set; } // Doanh thu hoặc số lượng
    }

    public class DoanhThuNgay
    {
        public DateTime Ngay { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int SoDonHang { get; set; }
        public decimal GiaTriTrungBinh { get; set; }
        public string? GhiChu { get; set; }
    }

    public class DoanhThuTheoThoiGian
    {
        public string? ThoiGian { get; set; } // Sẽ là "Tháng 1", "Tháng 2" hoặc "2023", "2024"
        public decimal TongDoanhThu { get; set; }
    }
}
