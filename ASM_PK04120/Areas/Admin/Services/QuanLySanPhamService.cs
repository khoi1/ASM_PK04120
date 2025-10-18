using ASM_PK04120.Data;
using Microsoft.EntityFrameworkCore;
using ASM_PK04120.Areas.Admin.Models;
using ASM_PK04120.Models;

namespace ASM_PK04120.Areas.Admin.Services
{
    public class QuanLySanPhamService : IQuanLySanPhamService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuanLySanPhamService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<DsSanPhamViewModel> LayDanhSachSanPham(string? tuKhoa, string? sapXep, int trang = 1, int kichThuocTrang = 10)
        {
            try
            {
                var danhSachDanhMuc = await _context.DanhMucs.AsNoTracking().ToListAsync();
                var truyVan = _context.SanPhams
                                      .Include(p => p.DanhMuc)
                                      .AsNoTracking();

                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    truyVan = truyVan.Where(p => p.TenSanPham.Contains(tuKhoa));
                }

                if (!string.IsNullOrEmpty(sapXep))
                {
                    switch (sapXep)
                    {
                        case "Hết hàng":
                            truyVan = truyVan.Where(p => p.SoLuongConLai <= 0);
                            break;
                        case "Sắp hết hàng":
                            truyVan = truyVan.Where(p => p.SoLuongConLai > 0 && p.SoLuongConLai <= 10);
                            break;
                        case "Tên từ a-z":
                            truyVan = truyVan.OrderBy(p => p.TenSanPham);
                            break;
                        case "Tên từ z-a":
                            truyVan = truyVan.OrderByDescending(p => p.TenSanPham);
                            break;
                        default:
                            truyVan = truyVan.OrderByDescending(p => p.NgayTao);
                            break;
                    }
                }

                var tongSoMuc = await truyVan.CountAsync();
                var tongSoTrang = (int)Math.Ceiling(tongSoMuc / (double)kichThuocTrang);

                var danhSachSanPham = await truyVan
                                          .OrderByDescending(p => p.NgayTao)
                                          .Skip((trang - 1) * kichThuocTrang)
                                          .Take(kichThuocTrang)
                                          .ToListAsync();

                var ds = danhSachSanPham.Select(sp => new SanPhamViewModel
                {
                    SanPham = sp,
                    DanhMuc = sp.DanhMuc!
                }).ToList();

                return new DsSanPhamViewModel
                {
                    DanhSachSanPham = ds,
                    DanhSachDanhMuc = danhSachDanhMuc,
                    TrangHienTai = trang,
                    TongSoTrang = tongSoTrang,
                    TuKhoa = tuKhoa,
                    SapXep = sapXep
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy danh sách sản phẩm: " + ex.Message);
                return new DsSanPhamViewModel();
            }
        }

        public async Task<bool> ThemSanPham(SanPhamModel sanPham, IFormFile? hinhAnh, IFormFile? moTaAnh)
        {
            try
            {
                if (hinhAnh != null)
                {
                    sanPham.HinhAnh = await LuuFileAnh(hinhAnh, "AnhSP");
                }

                if (moTaAnh != null)
                {
                    sanPham.MoTa = await LuuFileAnh(moTaAnh, "AnhMoTa");
                }

                sanPham.NgayTao = DateTime.Now;
                _context.SanPhams.Add(sanPham);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thêm sản phẩm: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> CapNhatSanPham(SanPhamModel sanPham, IFormFile? hinhAnhMoi, IFormFile? moTaAnhMoi)
        {
            try
            {
                var sanPhamTuDb = await _context.SanPhams.FindAsync(sanPham.MaSanPham);
                if (sanPhamTuDb == null) return false;
                sanPhamTuDb.TenSanPham = sanPham.TenSanPham;
                sanPhamTuDb.GiaBan = sanPham.GiaBan;
                sanPhamTuDb.GiaNhap = sanPham.GiaNhap;
                sanPhamTuDb.SoLuongConLai = sanPham.SoLuongConLai;
                sanPhamTuDb.MaDanhMuc = sanPham.MaDanhMuc;
                sanPhamTuDb.TinhTrang = sanPham.TinhTrang;

                if (hinhAnhMoi != null)
                {
                    XoaFileAnh(sanPhamTuDb.HinhAnh, "AnhSP");
                    sanPhamTuDb.HinhAnh = await LuuFileAnh(hinhAnhMoi, "AnhSP");
                }

                if (moTaAnhMoi != null)
                {
                    XoaFileAnh(sanPhamTuDb.MoTa, "AnhMoTa");
                    sanPhamTuDb.MoTa = await LuuFileAnh(moTaAnhMoi, "AnhMoTa");
                }

                _context.SanPhams.Update(sanPhamTuDb);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật sản phẩm: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> XoaSanPham(int maSanPham)
        {
            try
            {
                var sanPhamCanXoa = await _context.SanPhams.FindAsync(maSanPham);
                if (sanPhamCanXoa == null) return false;

                XoaFileAnh(sanPhamCanXoa.HinhAnh, "AnhSP");
                XoaFileAnh(sanPhamCanXoa.MoTa, "AnhMoTa");

                _context.SanPhams.Remove(sanPhamCanXoa);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa sản phẩm: " + ex.Message);
                return false;
            }
        }

        private async Task<string> LuuFileAnh(IFormFile tep, string thuMucCon)
        {
            string tenTepDocNhat = Guid.NewGuid().ToString() + "_" + tep.FileName;
            string duongDanThuMucUpload = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang/images", thuMucCon);
            string duongDanTep = Path.Combine(duongDanThuMucUpload, tenTepDocNhat);

            using (var luongTep = new FileStream(duongDanTep, FileMode.Create))
            {
                await tep.CopyToAsync(luongTep);
            }
            return tenTepDocNhat;
        }

        private void XoaFileAnh(string? tenTep, string thuMucCon)
        {
            if (string.IsNullOrEmpty(tenTep)) return;
            string duongDanTep = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang/images", thuMucCon, tenTep);
            if (File.Exists(duongDanTep))
            {
                File.Delete(duongDanTep);
            }
        }
    }
}