using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASM_PK04120.Models
{
    [Table("CHITIETDONHANG")]
    public class ChiTietDonHangModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int MaDonHang { get; set; }

        [Required]
        public int MaSanPham { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal GiaLucMua { get; set; }

        // Navigation property: Chi tiết này thuộc về một đơn hàng
        [ForeignKey("MaDonHang")]
        public virtual required DonHangModel DonHang { get; set; }

        // Navigation property: Chi tiết này ứng với một sản phẩm
        [ForeignKey("MaSanPham")]
        public virtual required SanPhamModel SanPham { get; set; }
    }
}
