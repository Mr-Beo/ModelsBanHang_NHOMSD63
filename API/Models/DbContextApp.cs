using API.Models;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Models
{
    public class DbContextApp : DbContext
    {
        public DbContextApp() { }

        public DbContextApp(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-UKQRGABP\SQLEXPRESS;Initial Catalog=DuanNhom63ModelsBanHang;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định dạng số tiền
            modelBuilder.Entity<GioHangChiTiet>().Property(x => x.Gia).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<HoaDonChiTiet>().Property(x => x.Gia).HasColumnType("decimal(18,2)");

            // Quan hệ GiayDotGiamGia
            modelBuilder.Entity<GiayDotGiamGia>().HasKey(x => x.GiayDotGiamGiaId);
            modelBuilder.Entity<GiayDotGiamGia>()
                .HasOne(x => x.GiayChiTiet)
                .WithMany()
                .HasForeignKey(x => x.GiayChiTietId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<GiayDotGiamGia>()
                .HasOne(x => x.GiamGia)
                .WithMany(x => x.GiayDotGiamGias)
                .HasForeignKey(x => x.GiamGiaId);

            // Quan hệ ChucVu – NhanVien
            modelBuilder.Entity<ChucVu>()
                .HasMany(x => x.nhanViens)
                .WithOne(x => x.ChucVu)
                .HasForeignKey(x => x.ChucVuId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed ChucVu
            modelBuilder.Entity<ChucVu>().HasData(
                new ChucVu
                {
                    ChucVuId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    TenChucVu = "Admin",
                    MotaChucVu = "Quản trị hệ thống",
                    TrangThai = 1
                },
                new ChucVu
                {
                    ChucVuId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    TenChucVu = "NhanVien",
                    MotaChucVu = "Nhân viên bán hàng",
                    TrangThai = 1
                });

            // Seed TaiKhoan + NhanVien Admin
            var adminTaiKhoanId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            modelBuilder.Entity<TaiKhoan>().HasData(new TaiKhoan
            {
                TaikhoanId = adminTaiKhoanId,
                Username = "admin",
                Password = "admin123",
                Ngaytaotaikhoan = DateTime.Now
            });

            modelBuilder.Entity<NhanVien>().HasData(new NhanVien
            {
                NhanVienId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                HoTen = "Nguyễn Văn Quản Trị",
                SoDienThoai = "0987654321",
                Email = "admin@shop.com",
                NgaySinh = new DateTime(1995, 1, 1),
                NgayCapNhatCuoiCung = DateTime.Now,
                TrangThai = true,
                TaikhoanId = adminTaiKhoanId,
                ChucVuId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            });

            // Seed Kích Cỡ 35-50
            var kichCoList = new List<KichCo>();
            for (int size = 35; size <= 50; size++)
            {
                kichCoList.Add(new KichCo
                {
                    KichCoId = Guid.NewGuid(),
                    TenKichCo = $"Size {size}",
                    size = size,
                    MoTa = $"Cỡ giày {size}",
                    TrangThai = true
                });
            }
            modelBuilder.Entity<KichCo>().HasData(kichCoList.ToArray());

            // Seed màu sắc cơ bản
            modelBuilder.Entity<MauSac>().HasData(
                new MauSac { MauSacId = Guid.NewGuid(), TenMau = "Đỏ", Color = "#FF0000", MoTa = "Màu đỏ cơ bản", TrangThai = true },
                new MauSac { MauSacId = Guid.NewGuid(), TenMau = "Xanh dương", Color = "#0000FF", MoTa = "Màu xanh dương cơ bản", TrangThai = true },
                new MauSac { MauSacId = Guid.NewGuid(), TenMau = "Xanh lá", Color = "#00FF00", MoTa = "Màu xanh lá cây", TrangThai = true },
                new MauSac { MauSacId = Guid.NewGuid(), TenMau = "Vàng", Color = "#FFFF00", MoTa = "Màu vàng", TrangThai = true },
                new MauSac { MauSacId = Guid.NewGuid(), TenMau = "Đen", Color = "#000000", MoTa = "Màu đen", TrangThai = true },
                new MauSac { MauSacId = Guid.NewGuid(), TenMau = "Trắng", Color = "#FFFFFF", MoTa = "Màu trắng", TrangThai = true }
            );
			modelBuilder.Entity<KhachHangVoucher>()
	   .HasIndex(x => new { x.KhachHangId, x.VoucherId })
	   .IsUnique();

			modelBuilder.Entity<KhachHangVoucher>()
				.HasOne(khv => khv.KhachHang)
				.WithMany(kh => kh.KhachHangVouchers)
				.HasForeignKey(khv => khv.KhachHangId);

			modelBuilder.Entity<KhachHangVoucher>()
				.HasOne(khv => khv.Voucher)
				.WithMany(vc => vc.KhachHangVouchers)
				.HasForeignKey(khv => khv.VoucherId);
			// HoaDon – NhanVien
			modelBuilder.Entity<HoaDon>()
     .HasOne(h => h.nhanVien)
     .WithMany(nv => nv.HoaDons)
     .HasForeignKey(h => h.NhanVienId)
     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.khachHang)
                .WithMany(kh => kh.HoaDons)
                .HasForeignKey(h => h.KhachHangId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.hinhThucThanhToan)
                .WithMany(ht => ht.HoaDons)
                .HasForeignKey(h => h.HinhThucThanhToanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.voucher)
                .WithMany(v => v.HoaDons)
                .HasForeignKey(h => h.VoucherId)
                .OnDelete(DeleteBehavior.SetNull);

            // KhachHang – TaiKhoan
            modelBuilder.Entity<KhachHang>()
           .HasOne(kh => kh.TaiKhoan)
           .WithOne(tk => tk.KhachHang)
           .HasForeignKey<KhachHang>(kh => kh.TaiKhoanId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GioHangChiTiet>()
                .HasOne(x => x.GioHang)
                .WithMany(x => x.GioHangChiTiets)
                .HasForeignKey(x => x.GioHangId)
                .OnDelete(DeleteBehavior.Cascade);
            // them
            modelBuilder.Entity<TraHang>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.HoaDon)
                      .WithMany()
                      .HasForeignKey(d => d.HoaDonId);

                entity.HasOne(d => d.GiayChiTiet)
                      .WithMany()
                      .HasForeignKey(d => d.GiayChiTietId);
            });
            modelBuilder.Entity<GioHangChiTiet>()
                .HasOne(x => x.GioHang)
                .WithMany(x => x.GioHangChiTiets)
                .HasForeignKey(x => x.GioHangId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GioHangChiTiet>()
                .HasOne(x => x.GiayChiTiet)
                .WithMany()
                .HasForeignKey(x => x.GiayChiTietId)
                .OnDelete(DeleteBehavior.Restrict);
            // thêm hình thức thanh toán
            modelBuilder.Entity<HinhThucThanhToan>().HasData(
    new HinhThucThanhToan
    {
        HinhThucThanhToanId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        TenHinhThuc = "Thanh toán khi nhận hàng (COD)",
        MoTa = "Khách thanh toán khi nhận hàng",
        TrangThai = true
    },
    new HinhThucThanhToan
    {
        HinhThucThanhToanId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
        TenHinhThuc = "Thanh toán VNPay",
        MoTa = "Thanh toán qua cổng VNPay",
        TrangThai = true
    }
);
        }

        // DbSet khai báo bảng
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<ChucVu> ChucVus { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<Giay> Giays { get; set; }
        public DbSet<GiayChiTiet> GiayChiTiets { get; set; }
        public DbSet<ChatLieu> ChatLieus { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<KichCo> KichCos { get; set; }
        public DbSet<DeGiay> DeGiays { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<GiamGia> GiamGias { get; set; }
        public DbSet<GiayDotGiamGia> GiayDotGiamGias { get; set; }
        public DbSet<ThuongHieu> ThuongHieus { get; set; }
        public DbSet<KieuDang> KieuDangs { get; set; }
        public DbSet<TheLoaiGiay> TheLoaiGiays { get; set; }
        public DbSet<Anh> Anhs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<HoaDonChiTiet> HoaDonChiTiets { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<GioHangChiTiet> GioHangChiTiets { get; set; }
        public DbSet<HinhThucThanhToan> hinhThucThanhToans { get; set; }
        public DbSet<DiaChiKhachHang> diaChiKhachHangs { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<GiayYeuThich> giayYeuThiches { get; set; }
        public DbSet<TraHang> TraHangs { get; set; } // them
        public DbSet<KhachHangVoucher> KhachHangVouchers { get; set; }

	}
}
