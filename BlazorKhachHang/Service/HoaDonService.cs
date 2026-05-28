using API.Models.DTO.BanHang;
using BlazorKhachHang.Service.IService;
using Data.Models;
using static API.Models.DTO.BanHang.DatHang;
using static System.Net.WebRequestMethods;

namespace BlazorKhachHang.Service
{
    public class HoaDonService : IHoaDonService
    {
        private readonly HttpClient _httpClient;

        public HoaDonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Add(HoaDon hoaDon)
        {
            var response = await _httpClient.PostAsJsonAsync("api/HoaDon", hoaDon);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<HoaDon>> GetAll() =>
            await _httpClient.GetFromJsonAsync<List<HoaDon>>("api/HoaDon");

        public async Task<HoaDon> GetById(Guid id) =>
            await _httpClient.GetFromJsonAsync<HoaDon>($"api/HoaDon/{id}");

        public async Task Update(HoaDon hoaDon)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/HoaDon/{hoaDon.HoaDonId}", hoaDon);
            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/HoaDon/{id}");
            response.EnsureSuccessStatusCode();
        }

        // thêm 
        public async Task<bool> DatHangAsync(DatHangRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/HoaDon/dat-hang", request);
            return response.IsSuccessStatusCode;
        }
        // thêm
        public async Task<List<HoaDonKhachHangDTO>> GetDonHangKhachHangAsync(Guid khachHangId)
        {
            return await _httpClient.GetFromJsonAsync<List<HoaDonKhachHangDTO>>(
                $"api/HoaDon/khach-hang/{khachHangId}"
            );
        }
        // thêm 
        public async Task<List<HoaDonKhachHangDTO>> GetDonHangKhachHangAsync(
        Guid khachHangId,
        string trangThai
)
        {
            return await _httpClient.GetFromJsonAsync<List<HoaDonKhachHangDTO>>(
                $"api/HoaDon/khach-hang/{khachHangId}?trangThai={trangThai}"
            );
        }
        // thêm sửa thông tin đơn hàng
        public async Task CapNhatThongTinGiaoHangAsync(UpdateThongTinGiaoHangDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync(
                "api/HoaDon/cap-nhat-giao-hang",
                dto
            );

            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                throw new Exception(msg);
            }
        }
        //thêm hủy đơn
        public async Task HuyDonHangAsync(Guid hoaDonId, string lyDo)
        {
            var url = $"api/HoaDon/huy-don/{hoaDonId}?lyDo={Uri.EscapeDataString(lyDo)}";

            var response = await _httpClient.PutAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                throw new Exception(msg);
            }
        }
        // lấy id hóa đơn
        public async Task<HoaDonKhachHangDTO> GetHoaDonByIdAsync(Guid hoaDonId)
        {
            return await _httpClient.GetFromJsonAsync<HoaDonKhachHangDTO>(
                $"api/HoaDon/chi-tiet/{hoaDonId}"
            );
        }
    }
}
