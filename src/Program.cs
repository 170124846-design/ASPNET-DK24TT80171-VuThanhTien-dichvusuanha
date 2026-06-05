// ============================================================
// FILE: Program.cs
// MỤC ĐÍCH: Điểm khởi động của toàn bộ ứng dụng web
// Đây là file đầu tiên chạy khi bạn nhấn nút Run trong Visual Studio
// Nhiệm vụ chính:
//   1. Đăng ký các dịch vụ (Services) cần thiết
//   2. Cấu hình các Middleware (lớp xử lý request)
//   3. Khởi động web server
// ============================================================

// Khai báo dùng thư viện Data và EF Core
using DatDichVuSuaChuaNhaCua.Database;
using DatDichVuSuaChuaNhaCua.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

// Tạo đối tượng builder - dùng để cấu hình ứng dụng trước khi chạy
// WebApplication.CreateBuilder() = khởi tạo ứng dụng ASP.NET với cấu hình mặc định
var builder = WebApplication.CreateBuilder(args);

// ============================================================
// PHẦN 1: ĐĂNG KÝ CÁC DỊCH VỤ (Services)
// builder.Services = nơi đăng ký mọi thứ ứng dụng cần dùng
// ============================================================

// Bật tính năng MVC: cho phép dùng Controllers và Views (Razor)
// ⚠️ KHÔNG XÓA: Đây là dòng bắt buộc để MVC hoạt động
builder.Services.AddControllersWithViews();

// Đăng ký kết nối database với Entity Framework Core
// AddDbContext<AppDbContext> = đăng ký AppDbContext vào hệ thống
// options.UseSqlServer() = chỉ định dùng SQL Server
// GetConnectionString("DefaultConnection") = đọc chuỗi kết nối từ appsettings.json
// ⚠️ KHÔNG THAY ĐỔI: "DefaultConnection" phải khớp với tên trong appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFileService, FileService>();

// Cấu hình xác thực người dùng bằng Cookie
// AddAuthentication() = đăng ký hệ thống xác thực
// CookieAuthenticationDefaults.AuthenticationScheme = dùng Cookie (phổ biến nhất cho web)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Trang đăng nhập: khi chưa đăng nhập mà vào trang cần Authorize
        // ✅ CÓ THỂ THAY ĐỔI: Đường dẫn trang login nếu bạn đổi tên
        options.LoginPath = "/TaiKhoan/DangNhap";

        // Trang từ chối: khi đã đăng nhập nhưng không có quyền
        options.AccessDeniedPath = "/TaiKhoan/DangNhap";

        // Thời gian Cookie hết hạn: 7 ngày
        // ✅ CÓ THỂ THAY ĐỔI: Tăng/giảm số ngày tùy ý
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

// Xây dựng ứng dụng từ các cấu hình đã đăng ký ở trên
var app = builder.Build();


// ============================================================
// PHẦN 2: CẤU HÌNH MIDDLEWARE
// Middleware = các lớp xử lý request theo thứ tự từ trên xuống
// THỨ TỰ RẤT QUAN TRỌNG - không được đảo lộn
// ============================================================

// Xử lý lỗi khi chạy production (không phải môi trường Development)
if (!app.Environment.IsDevelopment())
{
    // Nếu có lỗi không xử lý được → chuyển đến trang /Home/Error
    app.UseExceptionHandler("/Home/Error");
}

// Phục vụ các file tĩnh từ thư mục wwwroot (CSS, JS, hình ảnh upload...)
// ⚠️ KHÔNG XÓA: Thiếu dòng này thì CSS, ảnh sẽ không load được
app.UseStaticFiles();

// Bật tính năng routing: phân tích URL để xác định Controller và Action
// ⚠️ KHÔNG XÓA: Bắt buộc phải có trước UseAuthentication và UseAuthorization
app.UseRouting();

// Kích hoạt xác thực: đọc Cookie và xác định ai đang đăng nhập
// ⚠️ KHÔNG XÓA và KHÔNG ĐỔI THỨ TỰ với UseAuthorization
app.UseAuthentication();

// Kích hoạt phân quyền: kiểm tra quyền truy cập dựa trên Role
// Phải đặt SAU UseAuthentication
// ⚠️ KHÔNG XÓA: Thiếu dòng này thì [Authorize] sẽ không hoạt động
app.UseAuthorization();

// Cấu hình routing mặc định theo pattern: {controller}/{action}/{id?}
// controller = tên Controller (bỏ chữ "Controller")
// action = tên phương thức trong Controller
// id? = tham số không bắt buộc (? = có thể không có)
// Ví dụ: URL "/Service/Detail/5" → ServiceController.Detail(id=5)
// ✅ CÓ THỂ THAY ĐỔI: defaults để đổi trang chủ mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}"
    // controller=TrangChu → Trang chủ dùng HomeController
    // action=Index → Hàm mặc định là Index()
);

// Khởi động web server và bắt đầu lắng nghe request
// ⚠️ KHÔNG XÓA: Dòng cuối cùng bắt buộc
app.Run();




