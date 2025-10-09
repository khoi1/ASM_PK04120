using ASM_PK04120.Models;
using ASM_PK04120.Areas.KhachHang.Models;
using ASM_PK04120.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace ASM_PK04120.Areas.KhachHang.Services
{
    
    public class SanPhamService : ISanPhamService
    {
        private readonly AppDbContext _context;
        public SanPhamService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SanPhamBanChayViewModel>> LaySanPhamBanChayAsync(int soLuong)
        {
            try
            {
                // Xác định khoảng thời gian (ví dụ: 30 ngày gần đây)
                var mocThoiGian = DateTime.Now.AddDays(-30);

                // Lấy danh sách sản phẩm bán chạy dựa trên số lượng đã bán
                var sp = _context.ChiTietDonHangs
                    .Where(chiTiet => chiTiet.DonHang.NgayDatHang >= mocThoiGian)
                    .GroupBy(ct => ct.MaSanPham)
                    .Select(g => new
                    {
                        MaSanPham = g.Key,
                        TongSoLuong = g.Sum(ct => ct.SoLuong)
                    })
                    .OrderByDescending(x => x.TongSoLuong)
                    .Take(soLuong);

                var dsSanPhamViewModel = await sp
                   .Join(_context.SanPhams,
                         spBanChay => spBanChay.MaSanPham,
                         sp => sp.MaSanPham,
                         (spBanChay, sp) => new SanPhamBanChayViewModel
                         {
                             MaSanPham = sp.MaSanPham,
                             TenSanPham = sp.TenSanPham,
                             HinhAnh = sp.HinhAnh,
                             GiaBan = sp.GiaBan,
                             SoLuongDaBan = spBanChay.TongSoLuong
                         })
                   .ToListAsync();

                return dsSanPhamViewModel;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<SanPhamBanChayViewModel>();
            }
        }

        public async Task<ChiTietSanPhamViewModel?> LayChiTietSanPhamAsync(int maSanPham)
        {
            try
            {
                var sanPham = await _context.SanPhams
                .Include(sp => sp.DanhMuc)
                .FirstOrDefaultAsync(sp => sp.MaSanPham == maSanPham);

                if (sanPham == null)
                {
                    return null;
                }

                var sanPhamTuongTu = await _context.SanPhams
                    .Where(sp => sp.MaDanhMuc == sanPham.MaDanhMuc && sp.MaSanPham != maSanPham)
                    .Take(4)
                    .ToListAsync();
                return new ChiTietSanPhamViewModel
                {
                    SanPham = sanPham,
                    SanPhamTuongTu = sanPhamTuongTu
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task<SanPhamViewModel> LayDanhSachSanPhamAsync(int? maDanhMuc, string? phamViGia, string? thuTuSapXep, int soTrang)
        {
            try
            {
                var sp = _context.SanPhams.AsQueryable();

                // Lọc theo Danh mục
                if (maDanhMuc.HasValue)
                {
                    sp = sp.Where(s => s.MaDanhMuc == maDanhMuc.Value);
                }

                // Lọc theo Phạm vi giá
                if (!string.IsNullOrEmpty(phamViGia) && phamViGia != "all")
                {
                    switch (phamViGia)
                    {
                        case "0-100000":
                            sp = sp.Where(s => s.GiaBan < 100000);
                            break;
                        case "100000-200000":
                            sp = sp.Where(s => s.GiaBan >= 100000 && s.GiaBan <= 200000);
                            break;
                        case "200000-300000":
                            sp = sp.Where(s => s.GiaBan > 200000 && s.GiaBan <= 300000);
                            break;
                        case "300000-9999999":
                            sp = sp.Where(s => s.GiaBan > 300000);
                            break;
                    }
                }

                // Sắp xếp
                switch (thuTuSapXep)
                {
                    case "priceAsc":
                        sp = sp.OrderBy(p => p.GiaBan);
                        break;
                    case "priceDesc":
                        sp = sp.OrderByDescending(p => p.GiaBan);
                        break;
                    default: // "newest" or default
                        sp = sp.OrderByDescending(p => p.NgayTao);
                        break;
                }

                var tongSoSanPham = await sp.CountAsync();

                var tongSoTrang = (int)Math.Ceiling((double)tongSoSanPham / 9);

                var danhSachSanPham = await sp
                    .Skip((soTrang - 1) * 9)
                    .Take(9)
                    .ToListAsync();

                var danhSachDanhMuc = await _context.DanhMucs.ToListAsync();
                return new SanPhamViewModel
                {
                    DanhSachSanPham = danhSachSanPham,
                    DanhSachDanhMuc = danhSachDanhMuc,
                    TrangHienTai = soTrang,
                    TongSoTrang = tongSoTrang,
                    MaDanhMucHienTai = maDanhMuc,
                    SapXepHienTai = thuTuSapXep,
                    PhamViGiaHienTai = phamViGia
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new SanPhamViewModel
                {
                    DanhSachSanPham = new List<SanPhamModel>(),
                    DanhSachDanhMuc = new List<DanhMucModel>(),
                    TrangHienTai = 1,
                    TongSoTrang = 1,
                    MaDanhMucHienTai = maDanhMuc,
                    SapXepHienTai = thuTuSapXep,
                    PhamViGiaHienTai = phamViGia
                };
            }

        }

        public async Task<List<SanPhamModel>> TimKiemSanPhamAsync(string tuKhoa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tuKhoa))
                {
                    return new List<SanPhamModel>(); // Trả về danh sách rỗng nếu không có từ khóa
                }

                // Tìm kiếm không phân biệt chữ hoa/thường
                var tuKhoaLower = tuKhoa.ToLower();

                return await _context.SanPhams
                    .Where(p => p.TenSanPham.ToLower().Contains(tuKhoaLower))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<SanPhamModel>();
            }
        }
    }
}
