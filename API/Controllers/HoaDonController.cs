using API.IRepository;
using API.IRepository.Repository;
using API.Models.DTO.BanHang;
using Azure.Core;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonRepo _hoaDonRepo;
        private readonly IThongBaoRepository _thongBaoRepo; // ✅ thêm repo thông báo
        private readonly IChiTietHoaDonRepository _chiTietHoaDonRepo; // thêm
        private readonly IGioHangChiTietRepository _gioHangChiTietRepo; // thêm 
        private readonly IVoucherRepo _voucherRepo;
        public HoaDonController(IHoaDonRepo hoaDonRepo, IThongBaoRepository thongBaoRepo, IChiTietHoaDonRepository chiTietHoaDonRepo, IGioHangChiTietRepository gioHangChiTietRepo, IVoucherRepo voucherRepo)
        {
            _hoaDonRepo = hoaDonRepo;
            _thongBaoRepo = thongBaoRepo;
            _chiTietHoaDonRepo = chiTietHoaDonRepo; // thêm
            _gioHangChiTietRepo = gioHangChiTietRepo;
            _voucherRepo = voucherRepo;
        }

        // GET: api/HoaDon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetAllHoaDons()
        {
            var hoaDons = await _hoaDonRepo.GetAll();
            return Ok(hoaDons);


        }

        // GET: api/HoaDon/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<HoaDon>> GetHoaDonById(Guid id)
        {
            var hoaDon = await _hoaDonRepo.GetById(id);
            if (hoaDon == null)
                return NotFound();

            return Ok(hoaDon);
        }

        // POST: api/HoaDon
        [HttpPost]
        public async Task<ActionResult<HoaDon>> CreateHoaDon(HoaDon hoaDon)
        {
            await _hoaDonRepo.Create(hoaDon);

            // ✅ Ghi thông báo tạo hoá đơn
            await _thongBaoRepo.ThemThongBaoAsync($"🧾 Đã tạo hoá đơn mới với mã: {hoaDon.HoaDonId}");

            return CreatedAtAction(nameof(GetHoaDonById), new { id = hoaDon.HoaDonId }, hoaDon);
        }

        // PUT: api/HoaDon/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHoaDon(Guid id, HoaDon hoaDon)
        {
            if (id != hoaDon.HoaDonId)
                return BadRequest("ID mismatch");

            await _hoaDonRepo.Update(hoaDon);

            // ✅ Ghi thông báo cập nhật hoá đơn
            await _thongBaoRepo.ThemThongBaoAsync($"✏️ Đã cập nhật hoá đơn: {hoaDon.HoaDonId}");

            return NoContent();
        }

        // DELETE: api/HoaDon/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoaDon(Guid id)
        {
            var hoaDon = await _hoaDonRepo.GetById(id);
            if (hoaDon == null) return NotFound();

            var result = await _hoaDonRepo.Delete(id);
            if (!result) return NotFound();

            // ✅ Ghi thông báo xoá hoá đơn
            await _thongBaoRepo.ThemThongBaoAsync($"🗑️ Đã xoá hoá đơn: {hoaDon.HoaDonId}");

            return NoContent();
        }
        // thêm
        [HttpPost("dat-hang")]
        public async Task<IActionResult> DatHang(DatHang.DatHangRequest request)
        {
            if (request.Items == null || !request.Items.Any())
                return BadRequest("Không có sản phẩm để đặt hàng");
            Voucher? voucher = null;

            if (request.VoucherId.HasValue)
            {
                voucher = await _voucherRepo.GetById(request.VoucherId.Value);

                if (voucher == null)
                    return BadRequest("Voucher không tồn tại");

                // (optional) validate thêm
                if (DateTime.Now < voucher.NgayBatDau || DateTime.Now > voucher.NgayKetThuc)
                    return BadRequest("Voucher hết hạn");
            }
            // ✔ xác định hình thức thanh toán trước
            var hinhThucThanhToanId =
                request.HinhThucThanhToanId == Guid.Empty
                ? Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") // COD
                : request.HinhThucThanhToanId;

            // 1️⃣ tạo hóa đơn
            var hoaDon = new HoaDon
            {
                HoaDonId = Guid.NewGuid(),
                KhachHangId = request.KhachHangId,
                NhanVienId = null,
                NgayTao = DateTime.Now,
                TrangThai = "Chờ xác nhận",
                HinhThucThanhToanId = hinhThucThanhToanId,

                TrangThaiThanhToan =
                    hinhThucThanhToanId == Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                    ? "Chờ thanh toán"
                    : "Đã thanh toán",

                TenNguoiNhan = request.TenNguoiNhan,
                SoDienThoai = request.SoDienThoai,
                DiaChi = request.DiaChi,

                PhiShip = request.PhiShip,
                TongTienSauKhiGiam = request.TongTienSauKhiGiam,
                GhiChu = request.GhiChu
            };

            // 2️⃣ Map chi tiết hóa đơn
            var chiTiets = request.Items.Select(i => new HoaDonChiTiet
            {
                HoaDonChiTietId = Guid.NewGuid(),
                GiayChiTietId = i.GiayChiTietId,

                SoLuongSanPham = i.SoLuong,

                DonGia = i.Gia, // giá 1 sản phẩm

                Gia = i.Gia * i.SoLuong, // thành tiền

                TrangThai = false
            }).ToList();

            // 3️⃣ Chỉ lấy ID giỏ hàng hợp lệ (bỏ BuyNow)
            var gioHangIds = request.Items
                .Where(x => x.GioHangChiTietId != Guid.Empty)
                .Select(x => x.GioHangChiTietId)
                .ToList();

            // 4️⃣ GỌI REPO (transaction + trừ kho + xóa giỏ)
            var ok = await _hoaDonRepo.DatHangAsync(
                hoaDon,
                chiTiets,
                gioHangIds
            );

            if (!ok)
                return StatusCode(500, "Đặt hàng thất bại");

            // 5️⃣ Thông báo
            await _thongBaoRepo.ThemThongBaoAsync(
                $"🧾 Đơn hàng mới chờ xác nhận: {hoaDon.HoaDonId}"
            );

            return Ok(new
            {
                hoaDon.HoaDonId,
                hoaDon.TrangThai
            });
        }

        // thêm 
        [HttpGet("khach-hang/{khachHangId}")]
        public async Task<IActionResult> GetHoaDonByKhachHang(Guid khachHangId)
        {
            try
            {
                var hoaDons = await _hoaDonRepo.GetByKhachHangId(khachHangId);

                if (hoaDons == null || !hoaDons.Any())
                    return Ok(new List<object>());

                var result = hoaDons.Select(h => new
                {
                    HoaDonId = h.HoaDonId,
                    TrangThai = h.TrangThai,
                    TrangThaiThanhToan = h.TrangThaiThanhToan,
                    NgayTao = h.NgayTao,
                    HinhThucThanhToan = h.hinhThucThanhToan != null
                    ? h.hinhThucThanhToan.TenHinhThuc
                    : "",
                    TenNguoiNhan = h.TenNguoiNhan,
                    SoDienThoai = h.SoDienThoai,
                    DiaChi = h.DiaChi,
                    TongTien = h.TongTienSauKhiGiam,

                    ChiTiet = h.HoaDonChiTiets.Select(ct => new
                    {
                        GiayChiTietId = ct.GiayChiTietId,
                        SoLuongSanPham = ct.SoLuongSanPham,
                        DonGia = ct.DonGia ?? 0m,
                        Gia = ct.Gia,
                        TenGiay = ct.GiayChiTiet.Giay.TenGiay,
                        Size = ct.GiayChiTiet.KichCo.TenKichCo,
                        MauSac = ct.GiayChiTiet.MauSac.TenMau,
                        Anh = ct.GiayChiTiet.Anhs.FirstOrDefault()?.DuongDan
                    })
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // thêm chỉnh thông tin khách hàng khi đặt hàng
        [HttpPut("cap-nhat-giao-hang")]
        public async Task<IActionResult> CapNhatThongTinGiaoHang(UpdateThongTinGiaoHangDTO dto)
        {
            var hoaDon = await _hoaDonRepo.GetById(dto.HoaDonId);
            if (hoaDon == null)
                return NotFound("Không tìm thấy hóa đơn");

            // ✅ CHỈ UPDATE KHI KHÔNG NULL / KHÔNG RỖNG
            if (!string.IsNullOrWhiteSpace(dto.TenNguoiNhan))
                hoaDon.TenNguoiNhan = dto.TenNguoiNhan.Trim();

            if (!string.IsNullOrWhiteSpace(dto.SoDienThoai))
                hoaDon.SoDienThoai = dto.SoDienThoai.Trim();

            if (!string.IsNullOrWhiteSpace(dto.DiaChi))
                hoaDon.DiaChi = dto.DiaChi.Trim();

            await _hoaDonRepo.Update(hoaDon);

            await _thongBaoRepo.ThemThongBaoAsync(
                $"✏️ Khách sửa thông tin giao hàng: {hoaDon.HoaDonId}"
            );

            return Ok();
        }
        [HttpPut("huy-don/{hoaDonId}")]
        public async Task<IActionResult> HuyDon(Guid hoaDonId, [FromQuery] string? lyDo)
        {
            try
            {
                var ok = await _hoaDonRepo.HuyDonHangAsync(hoaDonId, lyDo);

                if (!ok)
                    return BadRequest("Không thể hủy đơn");

                await _thongBaoRepo.ThemThongBaoAsync(
                    $"❌ Đơn hàng đã bị hủy: {hoaDonId} - Lý do: {lyDo ?? "Admin hủy"}"
                );

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // thêm hoá đơn chi tiết
        [HttpGet("chi-tiet/{hoaDonId}")]
        public async Task<IActionResult> GetHoaDonChiTiet(Guid hoaDonId)
        {
            var h = await _hoaDonRepo.GetById(hoaDonId);

            if (h == null)
                return NotFound();

            var result = new HoaDonKhachHangDTO
            {
                HoaDonId = h.HoaDonId,
                TrangThai = h.TrangThai,
                TrangThaiThanhToan = h.TrangThaiThanhToan,
                NgayTao = h.NgayTao,

                TenNguoiNhan = h.TenNguoiNhan,
                SoDienThoai = h.SoDienThoai,
                DiaChi = h.DiaChi,
                PhiShip = (decimal)h.PhiShip,

                GiamGiaVoucher = h.voucher != null
                ? (decimal)h.voucher.SoTienGiam
                : 0m,
                TongTien = Convert.ToDecimal(h.TongTienSauKhiGiam),

                HinhThucThanhToan = h.hinhThucThanhToan != null
          ? h.hinhThucThanhToan.TenHinhThuc
          : "",

                ChiTiet = h.HoaDonChiTiets.Select(ct => new HoaDonChiTietDTO
                {
                    GiayChiTietId = ct.GiayChiTietId,
                    SoLuongSanPham = ct.SoLuongSanPham,
                    Gia = ct.Gia,
                    DonGia = ct.DonGia ?? 0m,
                    TenGiay = ct.GiayChiTiet.Giay.TenGiay,
                    Anh = ct.GiayChiTiet.Anhs.FirstOrDefault()?.DuongDan,
                    Size = ct.GiayChiTiet.KichCo?.TenKichCo,
                    MauSac = ct.GiayChiTiet.MauSac?.TenMau
                }).ToList()
            };

            return Ok(result);

        }
        // thêm
        [HttpPut("giao-thanh-cong/{id}")]
        public async Task<IActionResult> GiaoThanhCong(Guid id)
        {
            var ok = await _hoaDonRepo.GiaoThanhCongAsync(id);

            if (!ok)
                return BadRequest("Không thể cập nhật đơn");

            await _thongBaoRepo.ThemThongBaoAsync(
                $"✅ Đơn hàng hoàn thành: {id}"
            );

            return Ok();
        }
        // xác nhận đơn hàng
        [HttpPut("xac-nhan/{id}")]
        public async Task<IActionResult> XacNhanDon(Guid id)
        {
            var result = await _hoaDonRepo.XacNhanDonAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            await _thongBaoRepo.ThemThongBaoAsync(
                $"📦 Đã xác nhận đơn: {id}"
            );

            return Ok(result.Message);
        }

        [HttpPut("bat-dau-giao/{id}")]
        public async Task<IActionResult> BatDauGiao(Guid id)
        {
            var ok = await _hoaDonRepo.BatDauGiaoHangAsync(id);

            if (!ok)
                return BadRequest("Không thể chuyển trạng thái");

            await _thongBaoRepo.ThemThongBaoAsync(
                $"🚚 Đơn hàng đang giao: {id}"
            );

            return Ok();
        }

        [HttpPut("duyet-huy/{hoaDonId}")]
        public async Task<IActionResult> DuyetHuy(Guid hoaDonId, bool chapNhan)
        {
            try
            {
                var ok = await _hoaDonRepo.DuyetHuyDonAsync(hoaDonId, chapNhan);

                if (!ok)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
 

