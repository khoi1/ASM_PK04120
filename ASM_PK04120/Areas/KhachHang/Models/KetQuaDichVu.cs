namespace ASM_PK04120.Areas.KhachHang.Models
{
    public class KetQuaDichVu
    {
        public bool ThanhCong { get; set; }
        public string? ThongBao { get; set; }
        public string LoaiThongBao => ThanhCong ? "success" : "error";
        public int SoLuongTrongGio { get; set; }
        public int SoLuongThem { get; set; }
        public int TonKho { get; set; }
        public int TongSauKhiCong => SoLuongTrongGio + SoLuongThem;
    }
}
