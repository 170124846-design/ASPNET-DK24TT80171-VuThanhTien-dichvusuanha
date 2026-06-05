using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatDichVuSuaChuaNhaCua.Database;
using DatDichVuSuaChuaNhaCua.Models;

using DatDichVuSuaChuaNhaCua.Services;

namespace DatDichVuSuaChuaNhaCua.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext db;
        private readonly IFileService _fileService;

        public AdminController(AppDbContext context, IFileService fileService)
        {
            db = context;
            _fileService = fileService;
        }

        // trang tong quan
        public async Task<IActionResult> TongQuan()
        {
            ViewBag.TongDichVu    = await db.DichVu.CountAsync(s => s.DangHoatDong);
            ViewBag.DonChoXuLy    = await db.DonDatLich.CountAsync(b => b.TrangThai == "Pending");
            ViewBag.DonHoanThanh  = await db.DonDatLich.CountAsync(b => b.TrangThai == "Completed");
            ViewBag.TongKhachHang = await db.NguoiDung.CountAsync(u => u.VaiTro == "Customer");

            var donGanDay = await db.DonDatLich
                .Include(b => b.DichVu)
                .Include(b => b.NguoiDung)
                .OrderByDescending(b => b.NgayTao)
                .Take(5)
                .ToListAsync();

            return View(donGanDay);
        }

        // ---- QUAN LY DANH MUC ----

        public async Task<IActionResult> QuanLyDanhMuc()
        {
            var ds = await db.DanhMuc.OrderBy(c => c.TenDanhMuc).ToListAsync();
            return View(ds);
        }

        public IActionResult ThemDanhMuc() => View(new DanhMuc());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemDanhMuc(DanhMuc model)
        {
            if (ModelState.IsValid)
            {
                db.DanhMuc.Add(model);
                await db.SaveChangesAsync();
                TempData["Success"] = "Thêm danh mục thành công!";
                return RedirectToAction("QuanLyDanhMuc");
            }
            return View(model);
        }

        public async Task<IActionResult> SuaDanhMuc(int id)
        {
            var dm = await db.DanhMuc.FindAsync(id);
            if (dm == null) return NotFound();
            return View(dm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuaDanhMuc(DanhMuc model)
        {
            if (ModelState.IsValid)
            {
                db.DanhMuc.Update(model);
                await db.SaveChangesAsync();
                TempData["Success"] = "Cập nhật danh mục thành công!";
                return RedirectToAction("QuanLyDanhMuc");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaDanhMuc(int id)
        {
            var dm = await db.DanhMuc.FindAsync(id);
            if (dm != null)
            {
                db.DanhMuc.Remove(dm);
                await db.SaveChangesAsync();
                TempData["Success"] = "Đã xóa danh mục!";
            }
            return RedirectToAction("QuanLyDanhMuc");
        }

        // ---- QUAN LY DICH VU ----

        public async Task<IActionResult> QuanLyDichVu()
        {
            var ds = await db.DichVu.Include(s => s.DanhMuc).OrderBy(s => s.TenDichVu).ToListAsync();
            return View(ds);
        }

        public async Task<IActionResult> ThemDichVu()
        {
            ViewBag.DanhMuc = new SelectList(
                await db.DanhMuc.Where(c => c.DangHoatDong).ToListAsync(), "MaDanhMuc", "TenDanhMuc");
            return View(new DichVu());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemDichVu(DichVu model, IFormFile? anhFile)
        {
            if (anhFile != null && anhFile.Length > 0)
                model.HinhAnh = await _fileService.SaveImageAsync(anhFile, "QuanLyDichVu");

            ModelState.Remove("HinhAnh");

            if (ModelState.IsValid)
            {
                model.NgayTao = DateTime.Now;
                db.DichVu.Add(model);
                await db.SaveChangesAsync();
                TempData["Success"] = "Thêm dịch vụ thành công!";
                return RedirectToAction("QuanLyDichVu");
            }

            ViewBag.DanhMuc = new SelectList(
                await db.DanhMuc.Where(c => c.DangHoatDong).ToListAsync(), "MaDanhMuc", "TenDanhMuc");
            return View(model);
        }

        public async Task<IActionResult> SuaDichVu(int id)
        {
            var dv = await db.DichVu.FindAsync(id);
            if (dv == null) return NotFound();

            ViewBag.DanhMuc = new SelectList(
                await db.DanhMuc.Where(c => c.DangHoatDong).ToListAsync(), "MaDanhMuc", "TenDanhMuc", dv.MaDanhMuc);
            return View(dv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuaDichVu(DichVu model, IFormFile? anhFile)
        {
            if (anhFile != null && anhFile.Length > 0)
                model.HinhAnh = await _fileService.SaveImageAsync(anhFile, "QuanLyDichVu");

            ModelState.Remove("ImageUrl");

            if (ModelState.IsValid)
            {
                db.DichVu.Update(model);
                await db.SaveChangesAsync();
                TempData["Success"] = "Cập nhật dịch vụ thành công!";
                return RedirectToAction("QuanLyDichVu");
            }

            ViewBag.DanhMuc = new SelectList(
                await db.DanhMuc.Where(c => c.DangHoatDong).ToListAsync(), "MaDanhMuc", "TenDanhMuc");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaDichVu(int id)
        {
            var dv = await db.DichVu.FindAsync(id);
            if (dv != null)
            {
                db.DichVu.Remove(dv);
                await db.SaveChangesAsync();
                TempData["Success"] = "Đã xóa dịch vụ!";
            }
            return RedirectToAction("QuanLyDichVu");
        }

        // ---- QUAN LY DON DAT LICH ----

        public async Task<IActionResult> DonDatLich(string? trangThai)
        {
            var query = db.DonDatLich
                .Include(b => b.DichVu)
                .Include(b => b.NguoiDung)
                .AsQueryable();

            if (!string.IsNullOrEmpty(trangThai))
                query = query.Where(b => b.TrangThai == trangThai);

            var ds = await query.OrderByDescending(b => b.NgayTao).ToListAsync();
            ViewBag.TrangThai = trangThai;
            return View(ds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatTrangThaiDonDatLich(int id, string status)
        {
            var don = await db.DonDatLich.FindAsync(id);
            if (don != null)
            {
                don.TrangThai = status;
                don.NgayCapNhat = DateTime.Now;
                await db.SaveChangesAsync();
                TempData["Success"] = "Đã cập nhật trạng thái!";
            }
            return RedirectToAction("DonDatLich");
        }

        // Deleted LuuAnh method
    }
}




