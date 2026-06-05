using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatDichVuSuaChuaNhaCua.Models
{
    [Table("DanhMuc")]
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [StringLength(100)]
        [Display(Name = "Tên danh mục")]
        public string TenDanhMuc { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [StringLength(50)]
        [Display(Name = "Biểu tượng")]
        public string LopBieuTuong { get; set; } = "bi-tools";

        [Display(Name = "Đang hiển thị")]
        public bool DangHoatDong { get; set; } = true;

        public ICollection<DichVu> DsDichVu { get; set; } = new List<DichVu>();
    }
}



