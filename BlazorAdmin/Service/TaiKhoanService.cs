using API.Models.DTO;
using BlazorAdmin.Service.IService;
using Data.Models;
using System.Text.Json;



namespace BlazorAdmin.Service
{
    public class TaiKhoanService : ITaiKhoanService
    {
        private readonly HttpClient _httpClient;

        public TaiKhoanService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateTaiKhoanAsync(TaiKhoan tk)
        {
            await _httpClient.PostAsJsonAsync("/api/TaiKhoans", tk);
        }

        public async Task DeleteTaiKhoanAsync(Guid id)
        {
            await _httpClient.DeleteAsync("/api/TaiKhoans/" + id);
        }

        public async Task<List<TaiKhoan>> GetAllTaiKhoanAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TaiKhoan>>("/api/TaiKhoans");
        }

        public async Task<TaiKhoan> GetByIdTaiKhoanAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/TaiKhoans/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                    return null;

                return JsonSerializer.Deserialize<TaiKhoan>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                return null;
            }
        }

        public async Task UpdateTaiKhoanAsync(TaiKhoan tk)
        {
            await _httpClient.PutAsJsonAsync($"/api/TaiKhoans/{tk.TaikhoanId}", tk);
        }


        public async Task<LoginResponseDto> GetByIdChucVuAsync(string username , string password )
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/TaiKhoans/login?username={username}&pass={password}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            }
            catch (Exception)
            {
                return new LoginResponseDto() { IsSuccess = false , Message = "Tài khoản không tồn tại" };
            }
            
        }
        public async Task<TaiKhoan> GetByUsernameAsync(string username)
        {
            try
            {
                var encodedUsername = Uri.EscapeDataString(username);

                var response = await _httpClient.GetAsync(
                    $"api/TaiKhoans/username/{encodedUsername}");

                // nếu không tìm thấy => trả về null
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<TaiKhoan>();
            }
            catch
            {
                return null;
            }
        }

    }

}
