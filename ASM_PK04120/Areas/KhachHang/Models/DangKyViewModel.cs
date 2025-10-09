using System.ComponentModel.DataAnnotations;
namespace ASM_PK04120.Areas.KhachHang.Models
{
    public class DangKyViewModel
    {
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        [StringLength(100)]
        public required string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(255)]
        public required string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public required string MatKhau { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public required string XacNhanMatKhau { get; set; }
    }
}
