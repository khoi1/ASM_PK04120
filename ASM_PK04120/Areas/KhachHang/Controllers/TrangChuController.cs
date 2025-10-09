using ASM_PK04120.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASM_PK04120.Controllers
{
    [Area("KhachHang")]
    public class TrangChuController : Controller
    {
        private readonly ITrangChuService _trangChuService;
        public TrangChuController(ITrangChuService trangChuService)
        {
            _trangChuService = trangChuService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _trangChuService.LayDuLieuTrangChuAsync();
            return View(viewModel);
        }
    }
}
