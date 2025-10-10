using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ASM_PK04120.Models
{
    [Table("GIOHANG")]
    public class GioHangModel
    {
        [Key]
        public int MaGioHang { get; set; }
        [Required]
        public int MaNguoiDung { get; set; }
        [Required]
        public int MaSanPham { get; set; }
        [Required]
        public int SoLuong { get; set; }

        // Navigation property: Giỏ hàng này ứng với một sản phẩm
        [ForeignKey("MaSanPham")]
        public virtual required SanPhamModel SanPham { get; set; }
        // Navigation property: Giỏ hàng này thuộc về một khách hàng
        [ForeignKey("MaNguoiDung")]
        public virtual required NguoiDungModel NguoiDung { get; set; }

    }
}
