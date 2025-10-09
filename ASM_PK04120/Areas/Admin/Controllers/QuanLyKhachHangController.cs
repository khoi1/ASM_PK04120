using Microsoft.AspNetCore.Mvc;

namespace ASM_PK04120.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyKhachHangController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
