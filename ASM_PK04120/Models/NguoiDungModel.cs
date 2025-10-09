using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASM_PK04120.Models
{
    [Table("NGUOIDUNG")]
    public class NguoiDungModel
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Required(ErrorMessage = "Tài khoản không được để trống")]
        [StringLength(100)]
        public required string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255)]
        public required string MatKhau { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(255)]
        public required string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(255)]
        public required string Email { get; set; }

        [StringLength(20)]
        public string? SoDienThoai { get; set; } // Dấu ? cho phép giá trị NULL

        [StringLength(500)]
        public string? DiaChi { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống")]
        [StringLength(50)]
        public required string VaiTro { get; set; }

        public DateTime NgayTao { get; set; }

        public int TrangThai { get; set; }

        // Navigation property: Một người dùng có nhiều đơn hàng
        public virtual ICollection<DonHangModel>? DonHangs { get; set; }
    }
}
