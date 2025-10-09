using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_PK04120.Models
{
    [Table("DANHMUC")]
    public class DanhMucModel
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(255)]
        public required string TenDanhMuc { get; set; }

        // Navigation property: Một danh mục có nhiều sản phẩm
        public virtual required ICollection<SanPhamModel> SanPhams { get; set; }
    }
}
