using ASM_PK04120.Areas.Admin.Services;
using ASM_PK04120.Areas.KhachHang.Services;
using ASM_PK04120.Data;
using Microsoft.EntityFrameworkCore;

namespace ASM_PK04120
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>
            (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ĐĂNG KÝ DỊCH VỤ SESSION
            builder.Services.AddSession(options =>
            {
                // Tùy chọn: Cài đặt thời gian timeout cho session, ví dụ 30 phút
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Đăng ký các service
            builder.Services.AddScoped<ITrangChuService, TrangChuService>();
            builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();
            builder.Services.AddScoped<ISanPhamService, SanPhamService>();
            builder.Services.AddScoped<IGioHangService, GioHangService>();
            builder.Services.AddScoped<IQuanLyKhachHangService, QuanLyKhachHangService>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Kích hoạt middleware session
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=TrangChu}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=TrangChu}/{action=Index}/{id?}",
                defaults: new { area = "KhachHang" });

            app.Run();
        }
    }
}
