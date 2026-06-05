using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DatDichVuSuaChuaNhaCua.Database;
using DatDichVuSuaChuaNhaCua.Models;
using DatDichVuSuaChuaNhaCua.Models.ViewModels;

namespace DatDichVuSuaChuaNhaCua.Controllers
{
    [Authorize]
    public class DatLichController : Controller
    {
        private readonly AppDbContext db;

        public DatLichController(AppDbContext context)
        {
            db = context;
        }

        // form dat lich - truyen vao ma dich vu
        public async Task<IActionResult> TaoDon(int maDichVu)
        {
            var dv = await db.DichVu
                .Include(s => s.DanhMuc)
                .FirstOrDefaultAsync(s => s.MaDichVu == maDichVu && s.DangHoatDong);

            if (dv == null)
                return NotFound();

            int maND = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var nd = await db.NguoiDung.FindAsync(maND);

            var model = new BookingCreateViewModel
            {
                MaDichVu = dv.MaDichVu,
                TenDichVu = dv.TenDichVu,
                GiaDichVu = dv.GiaTien,
                TenKhachHang = nd?.HoTen ?? "",
                SdtKhachHang = nd?.SoDienThoai ?? "",
                NgayHen = DateTime.Today.AddDays(1)
            };

            ViewBag.DichVu = dv;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoDon(BookingCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DichVu = await db.DichVu.FindAsync(model.MaDichVu);
                return View(model);
            }

            int maND = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var dv = await db.DichVu.FindAsync(model.MaDichVu);

            var donDat = new DonDatLich
            {
                MaNguoiDung = maND,
                MaDichVu = model.MaDichVu,
                NgayHen = model.NgayHen,
                KhungGio = model.KhungGio,
                TenKhachHang = model.TenKhachHang,
                SdtKhachHang = model.SdtKhachHang,
                DiaChi = model.DiaChi,
                GhiChu = model.GhiChu,
                TrangThai = "Pending",
                TongTien = dv?.GiaTien ?? 0,
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };

            db.DonDatLich.Add(donDat);
            await db.SaveChangesAsync();

            TempData["Success"] = "Đặt lịch thành công! Chúng tôi sẽ liên hệ sớm.";
            return RedirectToAction("DonCuaToi");
        }

        // xem don dat lich cua toi
        public async Task<IActionResult> DonCuaToi()
        {
            int maND = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var dsDon = await db.DonDatLich
                .Include(b => b.DichVu)
                    .ThenInclude(s => s!.DanhMuc)
                .Where(b => b.MaNguoiDung == maND)
                .OrderByDescending(b => b.NgayTao)
                .ToListAsync();

            return View(dsDon);
        }

        // huy don
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyDon(int id)
        {
            int maND = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var don = await db.DonDatLich.FirstOrDefaultAsync(b => b.MaDon == id && b.MaNguoiDung == maND);

            if (don == null)
            {
                TempData["Error"] = "Không tìm thấy đơn!";
                return RedirectToAction("DonCuaToi");
            }

            if (don.TrangThai != "Pending")
            {
                TempData["Error"] = "Chỉ hủy được đơn đang chờ xác nhận!";
                return RedirectToAction("DonCuaToi");
            }

            don.TrangThai = "Cancelled";
            don.NgayCapNhat = DateTime.Now;
            await db.SaveChangesAsync();

            TempData["Success"] = "Đã hủy đơn thành công!";
            return RedirectToAction("DonCuaToi");
        }
    }
}




