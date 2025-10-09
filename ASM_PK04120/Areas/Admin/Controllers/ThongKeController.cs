using Microsoft.AspNetCore.Mvc;

namespace ASM_PK04120.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThongKeController : AdminBaseController
    {
        [Route("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
