using ASM_PK04120.Data;
using ASM_PK04120.Models;
using Microsoft.EntityFrameworkCore;
namespace ASM_PK04120.Areas.KhachHang.Services
{
    public class GioHangService : IGioHangService
    {
        private readonly AppDbContext _context;

        public GioHangService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GioHangModel>> LayGioHang(int maNguoiDung)
        {
            // Lấy danh sách các món hàng và đính kèm thông tin sản phẩm tương ứng
            return await _context.GioHangs
                                 .Where(gh => gh.MaNguoiDung == maNguoiDung)
                                 .Include(gh => gh.SanPham) // Dùng Include để lấy cả thông tin SanPham
                                 .ToListAsync();
        }

        public async Task ThemVaoGio(int maNguoiDung, int maSanPham, int soLuong)
        {
            // Kiểm tra xem sản phẩm này đã có trong giỏ của người dùng chưa
            var gioHangItem = await _context.GioHangs.FirstOrDefaultAsync(
                gh => gh.MaNguoiDung == maNguoiDung && gh.MaSanPham == maSanPham);

            if (gioHangItem == null)
            {
                // Nếu chưa có, tạo một dòng mới trong bảng GIOHANG
                var newItem = new GioHangModel
                {
                    MaNguoiDung = maNguoiDung,
                    MaSanPham = maSanPham,
                    SoLuong = soLuong
                };
                _context.GioHangs.Add(newItem);
            }
            else
            {
                // Nếu đã có, chỉ cần cộng thêm số lượng
                gioHangItem.SoLuong += soLuong;
            }

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();
        }

        public async Task XoaKhoiGio(int maGioHang)
        {
            var item = await _context.GioHangs.FindAsync(maGioHang);
            if (item != null)
            {
                _context.GioHangs.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
