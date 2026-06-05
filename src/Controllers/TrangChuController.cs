using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatDichVuSuaChuaNhaCua.Database;

namespace DatDichVuSuaChuaNhaCua.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly AppDbContext db;

        public TrangChuController(AppDbContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            // lay danh muc va dich vu noi bat
            var DanhMuc = await db.DanhMuc
                .Where(c => c.DangHoatDong)
                .OrderBy(c => c.TenDanhMuc)
                .ToListAsync();

            var dsMoiNhat = await db.DichVu
                .Include(s => s.DanhMuc)
                .Where(s => s.DangHoatDong)
                .OrderByDescending(s => s.NgayTao)
                .Take(6)
                .ToListAsync();

            ViewBag.DanhMuc = DanhMuc;
            ViewBag.DichVuMoi = dsMoiNhat;

            return View();
        }

        // tim kiem dich vu
        public async Task<IActionResult> TimKiem(string? tuKhoa, int? maDanhMuc)
        {
            var query = db.DichVu
                .Include(s => s.DanhMuc)
                .Where(s => s.DangHoatDong)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tuKhoa))
                query = query.Where(s => s.TenDichVu.Contains(tuKhoa) || (s.MoTa != null && s.MoTa.Contains(tuKhoa)));

            if (maDanhMuc.HasValue)
                query = query.Where(s => s.MaDanhMuc == maDanhMuc.Value);

            var ketQua = await query.OrderBy(s => s.TenDichVu).ToListAsync();

            var DanhMuc = await db.DanhMuc.Where(c => c.DangHoatDong).ToListAsync();

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.MaDanhMuc = maDanhMuc;
            ViewBag.DanhMuc = DanhMuc;

            return View(ketQua);
        }
    }
}




