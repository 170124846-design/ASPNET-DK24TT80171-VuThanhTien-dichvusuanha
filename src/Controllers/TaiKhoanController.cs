using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DatDichVuSuaChuaNhaCua.Database;
using DatDichVuSuaChuaNhaCua.Models;
using DatDichVuSuaChuaNhaCua.Models.ViewModels;

using DatDichVuSuaChuaNhaCua.Services;

namespace DatDichVuSuaChuaNhaCua.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly AppDbContext db;
        private readonly IAccountService _accountService;

        public TaiKhoanController(AppDbContext context, IAccountService accountService)
        {
            db = context;
            _accountService = accountService;
        }

        // trang dang nhap
        public IActionResult DangNhap()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "TrangChu");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string matKhauHash = _accountService.HashPassword(model.Password);

            var nd = db.NguoiDung.FirstOrDefault(u =>
                u.Email == model.Email &&
                u.MatKhauHash == matKhauHash &&
                u.DangHoatDong == true);

            if (nd == null)
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng!");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, nd.MaNguoiDung.ToString()),
                new Claim(ClaimTypes.Name, nd.HoTen),
                new Claim(ClaimTypes.Email, nd.Email),
                new Claim(ClaimTypes.Role, nd.VaiTro)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            if (nd.VaiTro == "Admin")
                return RedirectToAction("TongQuan", "Admin");

            return RedirectToAction("Index", "TrangChu");
        }

        // trang dang ky
        public IActionResult DangKy()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "TrangChu");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // kiem tra email da ton tai chua
            if (db.NguoiDung.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email này đã được đăng ký!");
                return View(model);
            }

            var nd = new NguoiDung
            {
                HoTen = model.HoTen,
                Email = model.Email,
                MatKhauHash = _accountService.HashPassword(model.Password),
                SoDienThoai = model.SoDienThoai,
                VaiTro = "Customer",
                DangHoatDong = true,
                NgayTao = DateTime.Now
            };

            db.NguoiDung.Add(nd);
            await db.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("DangNhap");
        }

        // dang xuat
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "TrangChu");
        }

    }
}




