
using API.IService;
using BlazorAdmin.Components;
using BlazorAdmin.Service;
using BlazorAdmin.Service.IService;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorBootstrap();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/";

    options.AccessDeniedPath = "/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});
builder.Services.AddAuthorizationCore();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string url = "https://localhost:7246/";
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(url)
});
builder.Services.AddHttpClient("voucher", client =>
{
    client.BaseAddress = new Uri(url);
});
builder.Services.AddHttpClient<IKhachHangService, KhachHangService>(client =>
{
    client.BaseAddress = new Uri(url);
});

builder.Services.AddHttpClient<IDeGiayService, DeGiayService>(client =>
{
    client.BaseAddress = new Uri(url);
});
builder.Services.AddHttpClient<IVoucherService, VoucherService>(client =>
{
    client.BaseAddress = new Uri(url); // đúng base URL API
});
builder.Services.AddHttpClient<IGiamGiaService, GiamGiaService>(client =>
{
    client.BaseAddress = new Uri(url); // hoặc địa chỉ backend của bạn
});
builder.Services.AddHttpClient<IKichCoService, KichCoService>(client =>
{
    client.BaseAddress = new Uri(url); // hoặc địa chỉ backend của bạn
});
builder.Services.AddHttpClient("giay", client =>
{
    client.BaseAddress = new Uri(url); // thay đổi tùy theo API của bạn
});

builder.Services.AddHttpClient<IVoucherService, VoucherService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7246"); // đúng base URL API
});
builder.Services.AddHttpClient<IGiamGiaService, GiamGiaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7246/"); // hoặc địa chỉ backend của bạn
});
builder.Services.AddHttpClient<IKichCoService, KichCoService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7246/"); // hoặc địa chỉ backend của bạn
});
builder.Services.AddHttpClient("giay", client =>
{
    client.BaseAddress = new Uri("https://localhost:7246/"); // thay đổi tùy theo API của bạn
});
builder.Services.AddHttpClient<IMauSacService, MauSacService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7246/");
});
builder.Services.AddHttpClient<IGiayService, GiayService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7246/");
});
builder.Services.AddHttpClient<IGiayChiTietService, GiayChiTietService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7246/");
});
builder.Services.AddHttpClient<IAnhService, AnhService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7246/"); // Đảm bảo đúng địa chỉ API backend
});
builder.Services.AddHttpClient<IKhachHangVoucherService, KhachHangVoucherService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7246/"); // Đảm bảo đúng địa chỉ API backend
});
builder.Services.AddScoped<IKhachHangVoucherService, KhachHangVoucherService>();
builder.Services.AddScoped<IXuLyDiaChi, XuLyDiaChi>();
builder.Services.AddScoped<IHoaDonChiTietService, HoaDonChiTietService>();
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IAnhService, AnhService>();
builder.Services.AddScoped<IChatLieuService, ChatLieuService>();
builder.Services.AddScoped<IDiaChiKhachHangService, DiaChiKhachHangService>();
builder.Services.AddScoped<IChiTietHoaDonService, ChiTietHoaDonService>();
builder.Services.AddScoped<IDeGiayService, DeGiayService>();
builder.Services.AddScoped<IGiamGiaService, GiamGiaService>();
builder.Services.AddScoped<IGiayChiTietService, GiayChiTietService>();
builder.Services.AddScoped<IGiayService, GiayService>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<ITheLoaiGiayService, TheLoaiGiayService>();
builder.Services.AddScoped<IThuongHieuService, ThuongHieuService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();
builder.Services.AddScoped<IKieuDangService, KieuDangService>();
builder.Services.AddScoped<IMauSacService, MauSacService>();
builder.Services.AddScoped<IKichCoService, KichCoService>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
builder.Services.AddScoped<IReturnService, ReturnService>();
builder.Services.AddScoped<IThongBaoService, ThongBaoService>();
builder.WebHost.UseWebRoot("wwwroot");
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    });
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 52428800;
});
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();                         // Dùng Swagger
    app.UseSwaggerUI();
}




app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.UseHttpsRedirection();

app.Run();
