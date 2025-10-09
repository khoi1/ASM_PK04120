using System.ComponentModel.DataAnnotations;
namespace ASM_PK04120.Areas.KhachHang.Models
{
    public class DangNhapViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản hoặc email")]
        public required string TaiKhoanOrEmail { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public required string MatKhau { get; set; }
    }
}
