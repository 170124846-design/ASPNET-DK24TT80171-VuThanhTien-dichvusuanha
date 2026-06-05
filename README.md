# Đồ án môn học: Chuyên đề ASP.NET
## Website Đặt Dịch Vụ Sửa Chữa Nhà Cửa

**Sinh viên:** Vũ Thành Tiến  
**Lớp:** DK24TT80171  
**Giảng viên hướng dẫn:** TS. Đoàn Phước Miền  

---

### 📖 Giới thiệu

Website cung cấp nền tảng kết nối giữa khách hàng có nhu cầu sửa chữa nhà cửa (điện, nước, sơn, mộc, điều hòa...) và đơn vị cung cấp dịch vụ. Khách hàng có thể xem danh sách dịch vụ, đặt lịch sửa chữa trực tuyến và theo dõi trạng thái đơn hàng. Admin có thể quản lý toàn bộ danh mục, dịch vụ và đơn đặt lịch.

| Thành phần | Công nghệ sử dụng |
| :--- | :--- |
| **Backend** | ASP.NET Core MVC (.NET 9) |
| **Database** | SQL Server |
| **ORM** | Entity Framework Core 9 |
| **Giao diện** | Bootstrap 5 |

---

### ✅ Yêu cầu cài đặt

Trước khi chạy dự án, cần đảm bảo máy tính đã có đầy đủ các phần mềm sau:

- **Hệ điều hành:** Windows 10 hoặc Windows 11
- **[.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)** *(bắt buộc — tải về và cài đặt nếu chưa có)*
- **Visual Studio 2022** (phiên bản 17.8 trở lên, có cài workload **ASP.NET and web development**)
- **SQL Server** (bất kỳ phiên bản nào, khuyến nghị SQL Server 2019+)
- **SQL Server Management Studio (SSMS)**

---

### 📁 Cấu trúc thư mục

```text
ASPNET-DK24TT80171-VuThanhTien-dichvusuanha/
├── README.md
├── setup/
│   ├── DatDichVuSuaChuaNhaCua.sql  # Script tạo CSDL (Khuyên dùng)
│   └── DatDichVuSuaChuaNhaCua.bak  # File Backup CSDL (Dự phòng)
├── src/
│   ├── DatDichVuSuaChuaNhaCua.slnx  # File Solution để mở project
│   ├── Controllers/                # Xử lý logic nghiệp vụ
│   ├── Database/                   # Kết nối Entity Framework Core
│   ├── Models/                     # Các lớp dữ liệu (ánh xạ bảng CSDL)
│   ├── Services/                   # Các dịch vụ hỗ trợ (file, tài khoản...)
│   ├── Views/                      # Giao diện người dùng (Razor)
│   ├── wwwroot/                    # File tĩnh (CSS, JS, hình ảnh)
│   ├── appsettings.json            # Cấu hình chuỗi kết nối Database
│   └── Program.cs                  # Điểm khởi động ứng dụng
└── thesis/
    ├── doc/                        # Báo cáo bản Word
    └── pdf/                        # Báo cáo bản PDF
```

---

### 🚀 Hướng dẫn cài đặt và chạy dự án

#### Bước 1: Tải source code về máy

Truy cập trang GitHub của dự án, nhấn nút **Code → Download ZIP**, sau đó giải nén ra một thư mục tùy ý trên máy.

*(Hoặc dùng lệnh: `git clone <đường_link_github>`)*

#### Bước 2: Khởi tạo Cơ sở dữ liệu

Dự án cung cấp sẵn 2 phương án để khởi tạo CSDL. Vui lòng chọn 1 trong 2 cách dưới đây:

**▶ Cách 1: Dùng file Script .sql (Khuyên dùng vì tương thích mọi phiên bản SQL Server)**
1. Mở phần mềm **SQL Server Management Studio (SSMS)** và đăng nhập.
2. Vào menu **File → Open → File...**, tìm và mở file **`setup/DatDichVuSuaChuaNhaCua.sql`**.
3. Nhấn nút **Execute** (hoặc phím **F5**) để chạy script. Hệ thống sẽ tự tạo CSDL và nạp dữ liệu.
4. Nhấn chuột phải vào thư mục **Databases** ở cột trái, chọn **Refresh** để kiểm tra CSDL đã xuất hiện.

**▶ Cách 2: Restore từ file Backup .bak**
1. Mở SSMS, nhấp chuột phải vào thư mục **Databases** chọn **Restore Database...**
2. Tích chọn vào mục **Device**, bấm nút `...` (3 chấm).
3. Bấm nút **Add** và trỏ tới file **`setup/DatDichVuSuaChuaNhaCua.bak`**.
4. Bấm **OK** ở các cửa sổ để tiến hành khôi phục CSDL.

#### Bước 3: Cấu hình kết nối Database

1. Mở thư mục `src/`, tìm file **`appsettings.json`**.
2. Mở file đó bằng Notepad hoặc bất kỳ trình soạn thảo nào.
3. Tìm đoạn `"DefaultConnection"` và sửa lại tên **Server** cho khớp với máy đang chạy:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TÊN_SERVER_MÁY_BẠN;Database=DatDichVuSuaChuaNhaCua;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

> 💡 **Cách tìm tên Server:** Mở SSMS lên, tên Server hiển thị ngay tại ô **Server name** ở màn hình đăng nhập đầu tiên. Copy tên đó rồi dán vào chỗ `TÊN_SERVER_MÁY_BẠN` là xong.

#### Bước 4: Mở và chạy dự án trong Visual Studio

1. Mở **Visual Studio 2022**.
2. Chọn **Open a project or solution**.
3. Điều hướng vào thư mục `src/`, chọn file **`DatDichVuSuaChuaNhaCua.slnx`** rồi nhấn **Open**.
4. Đợi Visual Studio tải xong các gói thư viện (NuGet packages) — có thể mất 1-2 phút lần đầu.
5. Nhấn **F5** (hoặc nút ▶ **Run**) để khởi chạy.
6. Trình duyệt sẽ tự động mở trang web tại địa chỉ `http://localhost:5065`.

---

### 🔑 Tài khoản Demo

| Vai trò | Email | Mật khẩu | Chức năng chính |
| :--- | :--- | :--- | :--- |
| **Admin** | `tienvt@admin.vn` | `tien123` | Quản lý danh mục, dịch vụ, duyệt và cập nhật đơn đặt lịch |
| **Khách hàng** | `khach@demo.vn` | `khach123` | Xem dịch vụ, đặt lịch sửa chữa, theo dõi đơn hàng |

> *Khách hàng mới có thể tự đăng ký tài khoản trực tiếp trên website.*

---

*Đồ án phục vụ mục đích học tập môn Chuyên đề ASP.NET.*
