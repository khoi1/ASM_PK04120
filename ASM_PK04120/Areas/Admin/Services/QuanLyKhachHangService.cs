using ASM_PK04120.Data;
using ASM_PK04120.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM_PK04120.Areas.Admin.Services
{
    public class QuanLyKhachHangService : IQuanLyKhachHangService
    {
        private readonly AppDbContext _context;
        public QuanLyKhachHangService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<KhachHangViewModel> LayDanhSachKhachHang(int page = 1, int pageSize = 10)
        {
            try
            {
                var dsKhachHang = _context.NguoiDungs
                                        .Where(kh => kh.VaiTro == "Khách hàng")
                                        .AsNoTracking();

                // Đếm tổng số khách hàng
                var soKH = await dsKhachHang.CountAsync();
                var soTrang = (int)Math.Ceiling(soKH / (double)pageSize);

                var KhachHangs = await dsKhachHang
                                       .OrderByDescending(kh => kh.NgayTao)
                                       .Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

                return new KhachHangViewModel
                {
                    DanhSachKhachHang = KhachHangs,
                    TongSoTrang = soTrang,
                    TrangHienTai = page
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy danh sách khách hàng: " + ex.Message);
                return new KhachHangViewModel();
            } 
        }

        public async Task<NguoiDungModel?> LayKhachHangTheoId(int maNguoiDung)
        {
            if (maNguoiDung <= 0) return null;
            try
            {
                return await _context.NguoiDungs
                            .AsNoTracking()
                            .FirstOrDefaultAsync(kh => kh.MaNguoiDung == maNguoiDung && kh.VaiTro == "Khách hàng");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy khách hàng theo ID: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> ThemKhachHang(NguoiDungModel nguoiDung)
        {
            if (nguoiDung == null) return false;
            try
            {
                nguoiDung.VaiTro = "Khách hàng";
                nguoiDung.NgayTao = DateTime.Now;

                _context.NguoiDungs.Add(nguoiDung);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thêm khách hàng: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> CapNhatKhachHang(NguoiDungModel nguoiDung)
        {
            if (nguoiDung == null) return false;
            try
            {
                var nguoiDungDb = await _context.NguoiDungs.FindAsync(nguoiDung.MaNguoiDung);
                if (nguoiDungDb == null) return false;

                nguoiDungDb.HoTen = nguoiDung.HoTen;
                nguoiDungDb.Email = nguoiDung.Email;
                nguoiDungDb.SoDienThoai = nguoiDung.SoDienThoai;
                nguoiDungDb.DiaChi = nguoiDung.DiaChi;
                nguoiDungDb.TrangThai = nguoiDung.TrangThai;

                _context.NguoiDungs.Update(nguoiDungDb);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật khách hàng: " + ex.Message);
                return false;
            }
        }
    }
}
