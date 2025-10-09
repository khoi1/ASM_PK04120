using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASM_PK04120.Models
{
    [Table("DONHANG")]
    public class DonHangModel
    {
        [Key]
        public int MaDonHang { get; set; }

        public int MaNguoiDung { get; set; }

        [Required]
        [StringLength(255)]
        public required string HoTenNguoiNhan { get; set; }

        [Required]
        [StringLength(20)]
        public required string SDTNguoiNhan { get; set; }

        [Required]
        [StringLength(500)]
        public required string DiaChiGiaoHang { get; set; }

        [StringLength(1000)]
        public string? GhiChu { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TongTien { get; set; }

        [Required]
        [StringLength(100)]
        public required string PhuongThucThanhToan { get; set; }

        [StringLength(100)]
        public string? TrangThai { get; set; }

        public DateTime NgayDatHang { get; set; }

        // Navigation property: Một đơn hàng thuộc về một người dùng
        [ForeignKey("MaNguoiDung")]
        public virtual required NguoiDungModel NguoiDung { get; set; }

        // Navigation property: Một đơn hàng có nhiều chi tiết
        public virtual required ICollection<ChiTietDonHangModel> ChiTietDonHangs { get; set; }
    }
}
