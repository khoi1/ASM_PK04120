using ASM_PK04120.Areas.Admin.Models;
using ASM_PK04120.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASM_PK04120.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThongKeController : AdminBaseController
    {
        private readonly IThongKeService _thongKeService;

        public ThongKeController(IThongKeService thongKeService)
        {
            _thongKeService = thongKeService;
        }

        public IActionResult Index()
        {
            var model = new ThongKeViewModel
            {
                TongQuan = _thongKeService.LayThongKeTongQuan(),

                // Top sản phẩm bán chạy 3 loại
                TopSanPhamWeek = _thongKeService.LayTopSanPhamWeek(),
                TopSanPhamMonth = _thongKeService.LayTopSanPhamMonth(),
                TopSanPhamYear = _thongKeService.LayTopSanPhamYear(),

                // Thống kê danh mục 2 loại
                DoanhThuDanhMuc = _thongKeService.LayDoanhThuDanhMuc(),
                SoLuongBanDanhMuc = _thongKeService.LaySoLuongBanDanhMuc(),

                // Bảng chi tiết doanh thu theo ngày
                DoanhThuNgays = _thongKeService.LayDoanhThuTheoNgay(),
                DoanhThuThangs = _thongKeService.LayDoanhThuTheoThang(),
                DoanhThuNams = _thongKeService.LayDoanhThuTheoNam()

            };

            return View(model);
        }
    }
}
