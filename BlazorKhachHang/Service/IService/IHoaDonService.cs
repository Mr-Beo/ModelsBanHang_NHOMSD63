
using API.Models.DTO.BanHang;
using Data.Models;
using static API.Models.DTO.BanHang.DatHang;

namespace BlazorKhachHang.Service.IService
{
    public interface IHoaDonService
    {
        Task Add(HoaDon hoaDon);
        Task<List<HoaDon>> GetAll();
        Task<HoaDon> GetById(Guid id);
        Task Update(HoaDon hoaDon);
        Task Delete(Guid id);
        Task<bool> DatHangAsync(DatHangRequest request); // thêm 
        // dùng cho trang /Oder hiện tại
        Task<List<HoaDonKhachHangDTO>> GetDonHangKhachHangAsync(Guid khachHangId); // thêm 
        //  dùng nếu sau này lọc theo trạng thái
        Task<List<HoaDonKhachHangDTO>> GetDonHangKhachHangAsync(Guid khachHangId, string trangThai); // thêm 
        Task CapNhatThongTinGiaoHangAsync(UpdateThongTinGiaoHangDTO dto); // thêm sửa thông tin đơn hàng
        Task HuyDonHangAsync(Guid hoaDonId, string lyDo);
        Task<HoaDonKhachHangDTO> GetHoaDonByIdAsync(Guid hoaDonId);// id hóa đơn
    }
}
