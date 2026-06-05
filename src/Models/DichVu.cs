using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatDichVuSuaChuaNhaCua.Models
{
    [Table("DichVu")]
    public class DichVu
    {
        [Key]
        public int MaDichVu { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên dịch vụ")]
        [StringLength(200)]
        [Display(Name = "Tên dịch vụ")]
        public string TenDichVu { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá (VND)")]
        public decimal GiaTien { get; set; }

        [Display(Name = "Thời gian (phút)")]
        public int ThoiGian { get; set; } = 60;

        [StringLength(500)]
        [Display(Name = "Hình ảnh")]
        public string? HinhAnh { get; set; }

        [Display(Name = "Đang cung cấp")]
        public bool DangHoatDong { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        public DanhMuc? DanhMuc { get; set; }
        public ICollection<DonDatLich> DsDonDat { get; set; } = new List<DonDatLich>();
    }
}



