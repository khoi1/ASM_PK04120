using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASM_PK04120.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBaseController : Controller, IAsyncActionFilter
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var vaiTro = context.HttpContext.Session.GetString("VaiTro");
            // Kiểm tra nếu người dùng chưa đăng nhập  
            if (string.IsNullOrEmpty(vaiTro))
            {
                // Chuyển hướng người dùng đến trang đăng nhập  
                context.Result = new RedirectToActionResult("DangNhap", "TaiKhoan", new { area = "KhachHang" });
                return; // Kết thúc việc thực thi action hiện tại  
            }

            // Kiểm tra nếu người dùng không phải là Admin
            if (vaiTro != "Admin")
            {
                context.Result = new RedirectToActionResult("Index", "TrangChu", new { area = "KhachHang" });
                return; // Kết thúc việc thực thi action hiện tại  
            }
            // Nếu đã đăng nhập, tiếp tục thực thi action  
            await next();
        }
    }
}
