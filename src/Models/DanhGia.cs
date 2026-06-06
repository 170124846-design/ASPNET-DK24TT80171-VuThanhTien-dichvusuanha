using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatDichVuSuaChuaNhaCua.Models
{
    public class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn mức độ đánh giá của bạn")]
        [Range(1, 5, ErrorMessage = "Số sao phải từ 1 đến 5")]
        public int SoSao { get; set; }

        [StringLength(500, ErrorMessage = "Vui lòng bình luận không quá 500 ký tự bạn nhé!")]
        public string? BinhLuan { get; set; }

        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        // Khóa ngoại nối với bảng DonDatLich
        public int MaDon { get; set; }

        [ForeignKey("MaDon")]
        public virtual DonDatLich? DonDatLich { get; set; }
    }
}