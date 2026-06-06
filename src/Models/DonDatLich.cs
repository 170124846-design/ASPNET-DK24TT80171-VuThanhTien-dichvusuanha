using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatDichVuSuaChuaNhaCua.Models
{
    [Table("DonDatLich")]
    public class DonDatLich
    {
        [Key]
        public int MaDon { get; set; }

        [Required]
        public int MaNguoiDung { get; set; }

        [Required]
        public int MaDichVu { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày hẹn")]
        public DateTime NgayHen { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khung giờ")]
        [StringLength(50)]
        [Display(Name = "Khung giờ")]
        public string KhungGio { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tên người liên hệ")]
        [StringLength(200)]
        [Display(Name = "Tên khách hàng")]
        public string TenKhachHang { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string SdtKhachHang { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [StringLength(500)]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        // Pending / Confirmed / InProgress / Completed / Cancelled
        [StringLength(30)]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Pending";

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng tiền")]
        public decimal TongTien { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [Display(Name = "Cập nhật lúc")]
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public NguoiDung? NguoiDung { get; set; }
        public DichVu? DichVu { get; set; }
        // Lưu đánh giá của khách hàng, dùng để kiểm tra xem khách hàng đã đánh giá chưa
        public virtual ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();
    }
}



