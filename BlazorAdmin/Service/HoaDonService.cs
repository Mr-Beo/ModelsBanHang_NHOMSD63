using BlazorAdmin.Service.IService;
using Data.Models;
using System.Text.Json;

namespace BlazorAdmin.Service
{
    public class HoaDonService : IHoaDonService
    {
        private readonly HttpClient _httpClient;

        public HoaDonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HoaDon> Add(HoaDon hoaDon)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/HoaDon", hoaDon);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<HoaDon>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    return default;
                }
            }
            catch (Exception)
            {

                return default;
            }
            
            
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
        // thêm xác nhận đơn hàng
        public async Task<(bool Success, string Message)> XacNhanDon(Guid id)
        {
            var response = await _httpClient.PutAsync($"api/HoaDon/xac-nhan/{id}", null);

            var message = await response.Content.ReadAsStringAsync();

            return (response.IsSuccessStatusCode, message);
        }
        public async Task BatDauGiao(Guid id)
        {
            var response = await _httpClient.PutAsync($"api/HoaDon/bat-dau-giao/{id}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task GiaoThanhCong(Guid id)
        {
            var response = await _httpClient.PutAsync($"api/HoaDon/giao-thanh-cong/{id}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task HuyDon(Guid id)
        {
            var response = await _httpClient.PutAsync($"api/HoaDon/huy-don/{id}", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
