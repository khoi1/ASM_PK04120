using ASM_PK04120.Areas.KhachHang.Models;
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

        public async Task<KetQuaDichVu> ThemVaoGio(int maNguoiDung, int maSanPham, int soLuong)
        {
            var kq = new KetQuaDichVu { ThanhCong = false, SoLuongThem = soLuong };

            try
            {
                // Lấy sản phẩm để biết tồn kho
                var sanPham = await _context.SanPhams.FindAsync(maSanPham);
                if (sanPham == null)
                {
                    kq.ThongBao = "Sản phẩm không tồn tại.";
                    return kq;
                }
                kq.TonKho = sanPham.SoLuongConLai;

                // Lấy item giỏ hiện có
                var gioHangItem = await _context.GioHangs.FirstOrDefaultAsync(
                    gh => gh.MaNguoiDung == maNguoiDung && gh.MaSanPham == maSanPham);

                int hienCo = gioHangItem?.SoLuong ?? 0;
                kq.SoLuongTrongGio = hienCo;

                int tongSauCong = hienCo + soLuong;

                if (soLuong <= 0)
                {
                    kq.ThongBao = "Số lượng phải lớn hơn 0.";
                    return kq;
                }

                if (tongSauCong > sanPham.SoLuongConLai)
                {
                    kq.ThongBao = $"Không thể thêm {soLuong} sp. Tổng {tongSauCong} vượt tồn kho {sanPham.SoLuongConLai}.";
                    return kq;
                }

                if (gioHangItem == null)
                {
                    _context.GioHangs.Add(new GioHangModel
                    {
                        MaNguoiDung = maNguoiDung,
                        MaSanPham = maSanPham,
                        SoLuong = soLuong
                    });
                }
                else
                {
                    gioHangItem.SoLuong = tongSauCong;
                }

                await _context.SaveChangesAsync();
                kq.ThanhCong = true;
                kq.ThongBao = "Đã thêm vào giỏ hàng.";
                return kq;
            }
            catch (Exception ex)
            {
                kq.ThongBao = "Đã xảy ra lỗi không mong muốn. " + ex.Message;
            }

            return kq;
        }

        public async Task<KetQuaDichVu> XoaKhoiGio(int maGioHang)
        {
            var kq = new KetQuaDichVu { ThanhCong = false };

            try
            {
                var item = await _context.GioHangs
                    .Include(x => x.SanPham)
                    .FirstOrDefaultAsync(x => x.MaGioHang == maGioHang);

                if (item == null)
                {
                    kq.ThongBao = "Không tìm thấy sản phẩm trong giỏ hàng để xóa.";
                    return kq;
                }

                string tenSp = item.SanPham?.TenSanPham ?? "Sản phẩm";

                _context.GioHangs.Remove(item);
                await _context.SaveChangesAsync();

                kq.ThanhCong = true;
                kq.ThongBao = $"Đã xóa {tenSp} khỏi giỏ hàng";
                return kq;
            }
            catch (Exception ex)
            {
                kq.ThongBao = "Đã xảy ra lỗi không mong muốn khi xóa sản phẩm. " + ex.Message;
            }

            return kq;
        }
    }
}
