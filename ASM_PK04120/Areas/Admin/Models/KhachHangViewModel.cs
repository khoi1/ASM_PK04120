using ASM_PK04120.Models;
namespace ASM_PK04120.Areas.Admin.Models
{
    public class KhachHangViewModel
    {
        public List<NguoiDungModel> DanhSachKhachHang { get; set; } = new List<NguoiDungModel>();
        public int TrangHienTai { get; set; }
        public int TongSoTrang { get; set; }
    }
}
