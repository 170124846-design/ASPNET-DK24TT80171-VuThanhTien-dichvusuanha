using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatDichVuSuaChuaNhaCua.Database;

namespace DatDichVuSuaChuaNhaCua.Controllers
{
    public class DichVuController : Controller
    {
        private readonly AppDbContext db;

        public DichVuController(AppDbContext context)
        {
            db = context;
        }

        // danh sach dich vu, co the loc theo danh muc
        public async Task<IActionResult> Index(int? maDanhMuc)
        {
            var DanhMuc = await db.DanhMuc.Where(c => c.DangHoatDong).ToListAsync();

            var query = db.DichVu.Include(s => s.DanhMuc).Where(s => s.DangHoatDong).AsQueryable();

            if (maDanhMuc.HasValue)
                query = query.Where(s => s.MaDanhMuc == maDanhMuc.Value);

            var dsDichVu = await query.OrderBy(s => s.TenDichVu).ToListAsync();

            ViewBag.DanhMuc = DanhMuc;
            ViewBag.MaDanhMucDaChon = maDanhMuc;

            return View(dsDichVu);
        }

        // xem chi tiet 1 dich vu
        public async Task<IActionResult> ChiTiet(int id)
        {
            var dv = await db.DichVu
                .Include(s => s.DanhMuc)
                .FirstOrDefaultAsync(s => s.MaDichVu == id && s.DangHoatDong);

            if (dv == null)
                return NotFound();

            // lay them dich vu lien quan cung danh muc
            var dvLienQuan = await db.DichVu
                .Where(s => s.MaDanhMuc == dv.MaDanhMuc && s.MaDichVu != id && s.DangHoatDong)
                .Take(4)
                .ToListAsync();

            ViewBag.DvLienQuan = dvLienQuan;

            return View(dv);
        }
    }
}




