using System.Globalization;
using ASM_PK04120.Areas.Admin.Models;
using ASM_PK04120.Data;
using Microsoft.EntityFrameworkCore;

namespace ASM_PK04120.Areas.Admin.Services
{
    public class ThongKeService : IThongKeService
    {
        private readonly AppDbContext _context;
        public ThongKeService(AppDbContext context)
        {
            _context = context;
        }

        public ThongKeTongQuan LayThongKeTongQuan()
        {
            var homNay = DateTime.Now;
            // Lấy ngày đầu tiên của tháng hiện tại
            var ngayDauTien = new DateTime(homNay.Year, homNay.Month, 1);

            var donHangThangNay = _context.DonHangs
                .Where(d => d.NgayDatHang >= ngayDauTien && d.NgayDatHang <= homNay)
                .ToList();

            var khachHangMoiThangNay = _context.NguoiDungs
                .Where(kh => kh.NgayTao >= ngayDauTien && kh.NgayTao <= homNay)
                .Count();

            decimal tongDoanhThu = donHangThangNay.Sum(d => d.TongTien);
            int tongDonHang = donHangThangNay.Count;
            decimal giaTriTB = tongDonHang == 0 ? 0 : tongDoanhThu / tongDonHang;

            return new ThongKeTongQuan
            {
                DoanhThuThangNay = tongDoanhThu,
                DonHangThangNay = tongDonHang,
                KhachHangMoi = khachHangMoiThangNay,
                GiaTriTrungBinh = giaTriTB,
            };
        }

        public List<TopSanPham> LayTopSanPhamWeek()
        {
            var ngayBatDau = DateTime.Now.AddDays(-7);
            return LayTopSanPhamTheoKhoang(ngayBatDau);
        }

        public List<TopSanPham> LayTopSanPhamMonth()
        {
            var ngayBatDau = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return LayTopSanPhamTheoKhoang(ngayBatDau);
        }

        public List<TopSanPham> LayTopSanPhamYear()
        {
            var ngayBatDau = new DateTime(DateTime.Now.Year, 1, 1);
            return LayTopSanPhamTheoKhoang(ngayBatDau);
        }

        private List<TopSanPham> LayTopSanPhamTheoKhoang(DateTime ngayBatDau)
        {
            return _context.ChiTietDonHangs
                .Include(c => c.SanPham)
                .Include(c => c.DonHang)
                .Where(c => c.DonHang.TrangThai == "Đã giao"
                         && c.DonHang.NgayDatHang >= ngayBatDau)
                .GroupBy(c => new { c.SanPham.MaSanPham, c.SanPham.TenSanPham })
                .Select(g => new TopSanPham
                {
                    TenSanPham = g.Key.TenSanPham,
                    SoLuongBan = g.Sum(x => x.SoLuong),
                    DoanhThu = g.Sum(x => x.SoLuong * x.GiaLucMua)
                })
                .OrderByDescending(x => x.DoanhThu)
                .Take(5)
                .ToList();
        }

        public List<ThongKeDanhMuc> LayDoanhThuDanhMuc()
        {
            return LayThongKeDanhMucTheo("DoanhThu");
        }

        public List<ThongKeDanhMuc> LaySoLuongBanDanhMuc()
        {
            return LayThongKeDanhMucTheo("SoLuongBan");
        }

        private List<ThongKeDanhMuc> LayThongKeDanhMucTheo(string TrangThai)
        {
            var ds = _context.SanPhams
                .Include(s => s.DanhMuc)
                .Include(s => s.ChiTietDonHangs)
                .ThenInclude(ct => ct.DonHang)
                .Where(s => s.ChiTietDonHangs.Any(ct => ct.DonHang.TrangThai == "Đã giao"));

            return ds
                .GroupBy(s => s.DanhMuc.TenDanhMuc)
                .Select(g => new ThongKeDanhMuc
                {
                    TenDanhMuc = g.Key ?? "Khác",
                    GiaTri = TrangThai == "DoanhThu"
                        ? g.Sum(s => s.ChiTietDonHangs.Sum(ct => ct.SoLuong * ct.GiaLucMua))
                        : g.Sum(s => s.ChiTietDonHangs.Sum(ct => ct.SoLuong))
                })
                .OrderByDescending(x => x.GiaTri)
                .ToList();
        }

        public List<DoanhThuNgay> LayDoanhThuTheoNgay()
        {
            var homNay = DateTime.Today;
            var ngayBatDau = homNay.AddDays(-29);

            // Lấy dữ liệu thực tế và giữ lại TẤT CẢ thông tin cần thiết
            var doanhThuThucTe = _context.DonHangs
                .Where(d => d.TrangThai == "Đã giao" && d.NgayDatHang.Date >= ngayBatDau && d.NgayDatHang.Date <= homNay)
                .GroupBy(d => d.NgayDatHang.Date)
                .Select(g => new // Tạo một đối tượng tạm chứa đủ thông tin
                {
                    Ngay = g.Key,
                    TongDoanhThu = g.Sum(x => x.TongTien),
                    SoDonHang = g.Count(),
                    GiaTriTrungBinh = g.Count() == 0 ? 0 : g.Sum(x => x.TongTien) / g.Count()
                })
                .ToDictionary(x => x.Ngay, x => x); // x => x nghĩa là lấy cả object làm value

            var ketQuaDayDu = new List<DoanhThuNgay>();
            for (var ngay = ngayBatDau; ngay <= homNay; ngay = ngay.AddDays(1))
            {
                // Kiểm tra xem ngày này có dữ liệu trong Dictionary không
                if (doanhThuThucTe.TryGetValue(ngay, out var duLieuNgay))
                {
                    // Nếu có, lấy đầy đủ thông tin từ đối tượng tạm
                    ketQuaDayDu.Add(new DoanhThuNgay
                    {
                        Ngay = ngay,
                        TongDoanhThu = duLieuNgay.TongDoanhThu,
                        SoDonHang = duLieuNgay.SoDonHang,
                        GiaTriTrungBinh = duLieuNgay.GiaTriTrungBinh,
                        GhiChu = "" // Hoặc thêm logic ghi chú nếu cần
                    });
                }
                else
                {
                    // Nếu không có, thêm dòng với giá trị 0
                    ketQuaDayDu.Add(new DoanhThuNgay
                    {
                        Ngay = ngay,
                        TongDoanhThu = 0,
                        SoDonHang = 0,
                        GiaTriTrungBinh = 0,
                        GhiChu = ""
                    });
                }
            }

            return ketQuaDayDu.OrderByDescending(x => x.Ngay).ToList();
        }


        public List<DoanhThuTheoThoiGian> LayDoanhThuTheoThang()
        {
            var namNay = DateTime.Now.Year;

            // Lấy doanh thu thực tế theo từng tháng trong năm nay
            var doanhThuThucTe = _context.DonHangs
                .Where(d => d.TrangThai == "Đã giao" && d.NgayDatHang.Year == namNay)
                .GroupBy(d => d.NgayDatHang.Month)
                .Select(g => new
                {
                    Thang = g.Key, // Số tháng (1-12)
                    TongDoanhThu = g.Sum(x => x.TongTien)
                    
                })
                .ToDictionary(x => x.Thang, x => x.TongDoanhThu);

            // Tạo danh sách đầy đủ 12 tháng
            var ketQuaDayDu = new List<DoanhThuTheoThoiGian>();
            for (int thang = 1; thang <= 12; thang++)
            {
                decimal doanhThuThang = doanhThuThucTe.ContainsKey(thang) ? doanhThuThucTe[thang] : 0;
                ketQuaDayDu.Add(new DoanhThuTheoThoiGian
                {
                    // Lấy tên tháng tiếng Việt
                    ThoiGian = CultureInfo.GetCultureInfo("vi-VN").DateTimeFormat.GetMonthName(thang),
                    TongDoanhThu = doanhThuThang
                });
            }

            // Trả về danh sách (đã tự sắp xếp từ tháng 1 đến 12)
            return ketQuaDayDu;
        }


        public List<DoanhThuTheoThoiGian> LayDoanhThuTheoNam()
        {
            // Tìm năm đầu tiên và năm cuối cùng có đơn hàng
            var namDauTien = _context.DonHangs
                                .Where(d => d.TrangThai == "Đã giao")
                                .Min(d => (int?)d.NgayDatHang.Year) ?? DateTime.Now.Year;
            var namCuoiCung = _context.DonHangs
                                .Where(d => d.TrangThai == "Đã giao")
                                .Max(d => (int?)d.NgayDatHang.Year) ?? DateTime.Now.Year;

            // Lấy doanh thu thực tế theo từng năm
            var doanhThuThucTe = _context.DonHangs
                .Where(d => d.TrangThai == "Đã giao")
                .GroupBy(d => d.NgayDatHang.Year)
                .Select(g => new
                {
                    Nam = g.Key,
                    TongDoanhThu = g.Sum(x => x.TongTien)
                })
                .ToDictionary(x => x.Nam, x => x.TongDoanhThu);

            // Tạo danh sách đầy đủ các năm từ năm đầu tiên đến năm cuối cùng
            var ketQuaDayDu = new List<DoanhThuTheoThoiGian>();
            for (int nam = namDauTien; nam <= namCuoiCung; nam++)
            {
                decimal doanhThuNam = doanhThuThucTe.ContainsKey(nam) ? doanhThuThucTe[nam] : 0;
                ketQuaDayDu.Add(new DoanhThuTheoThoiGian
                {
                    ThoiGian = nam.ToString(),
                    TongDoanhThu = doanhThuNam
                });
            }

            // Sắp xếp theo năm tăng dần và trả về
            return ketQuaDayDu.OrderBy(x => x.ThoiGian).ToList();
        }
    }
}
