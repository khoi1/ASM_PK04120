using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASM_PK04120.Models
{
    [Table("SANPHAM")]
    public class SanPhamModel
    {
        [Key]
        public int MaSanPham { get; set; }

        public int MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(255)]
        public required string TenSanPham { get; set; }

        public string? MoTa { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal GiaBan { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal GiaNhap { get; set; }

        [StringLength(500)]
        public string? HinhAnh { get; set; }

        [Required]
        public int SoLuongConLai { get; set; }

        [Required]
        public DateTime NgayTao { get; set; }

        public bool TinhTrang { get; set; }

        // Navigation property: Một sản phẩm thuộc về một danh mục
        [ForeignKey("MaDanhMuc")]
        public virtual DanhMucModel DanhMuc { get; set; } = null!;

        // Navigation property: Một sản phẩm có trong nhiều chi tiết đơn hàng
        public virtual ICollection<ChiTietDonHangModel> ChiTietDonHangs { get; set; } = new List<ChiTietDonHangModel>();

        public virtual ICollection<GioHangModel> GioHangs { get; set; } = new List<GioHangModel>();
    }
}
