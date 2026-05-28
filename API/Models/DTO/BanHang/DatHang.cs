namespace API.Models.DTO.BanHang
{
    // thêm
    public class DatHang
    {
        public class DatHangRequest
        {   
            public Guid KhachHangId { get; set; } 
            public float PhiShip { get; set; }
            public float TongTienSauKhiGiam { get; set; }
            public string? GhiChu { get; set; }
            public string TenNguoiNhan { get; set; } = "";
            public string SoDienThoai { get; set; } = "";
            public string DiaChi { get; set; } = "";
            public Guid HinhThucThanhToanId { get; set; }
            public Guid? VoucherId { get; set; }
            public List<DatHangItem> Items { get; set; } = new();
        }

        public class DatHangItem
        {
            public Guid GioHangChiTietId { get; set; }
            public Guid GiayChiTietId { get; set; }
            public int SoLuong { get; set; }
            public decimal Gia { get; set; }
        }
    }
}
