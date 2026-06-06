using Microsoft.EntityFrameworkCore;
using DatDichVuSuaChuaNhaCua.Models;

namespace DatDichVuSuaChuaNhaCua.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Khai báo DbSet cho bảng DanhMuc
        public DbSet<DanhMuc> DanhMuc { get; set; }
        //  Khai báo DbSet cho bảng DichVu
        public DbSet<DichVu> DichVu { get; set; }
        // Khai báo DbSet cho bảng NguoiDung

        public DbSet<NguoiDung> NguoiDung { get; set; }
        // Khai báo DbSet cho bảng DonDatLich
        public DbSet<DonDatLich> DonDatLich { get; set; }

        // Khai báo DbSet cho bảng DanhGia
        public DbSet<DanhGia> DanhGia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1 danh muc co nhieu dich vu
            modelBuilder.Entity<DichVu>()
                .HasOne(dv => dv.DanhMuc)
                .WithMany(dm => dm.DsDichVu)
                .HasForeignKey(dv => dv.MaDanhMuc)
                .OnDelete(DeleteBehavior.Restrict);

            // 1 nguoi dung co nhieu don dat lich
            modelBuilder.Entity<DonDatLich>()
                .HasOne(d => d.NguoiDung)
                .WithMany(nd => nd.DsDonDat)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.Restrict);

            // 1 dich vu co nhieu don dat lich
            modelBuilder.Entity<DonDatLich>()
                .HasOne(d => d.DichVu)
                .WithMany(dv => dv.DsDonDat)
                .HasForeignKey(d => d.MaDichVu)
                .OnDelete(DeleteBehavior.Restrict);

            // email nguoi dung khong trung nhau
            modelBuilder.Entity<NguoiDung>()
                .HasIndex(nd => nd.Email)
                .IsUnique();
        }
    }
}




