using Microsoft.EntityFrameworkCore;
using ASM_PK04120.Models;

namespace ASM_PK04120.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<DanhMucModel> DanhMucs { get; set; }
        public DbSet<NguoiDungModel> NguoiDungs { get; set; }
        public DbSet<SanPhamModel> SanPhams { get; set; }
        public DbSet<DonHangModel> DonHangs { get; set; }
        public DbSet<ChiTietDonHangModel> ChiTietDonHangs { get; set; }
        public DbSet<GioHangModel> GioHangs { get; set; }
    }
}
