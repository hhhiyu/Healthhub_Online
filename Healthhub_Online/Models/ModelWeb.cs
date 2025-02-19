using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Healthhub_Online.Models
{
    public partial class ModelWeb : DbContext
    {
        public ModelWeb()
            : base("name=ModelWeb")
        {
        }

        public virtual DbSet<DanhGia> DanhGias { get; set; }
        public virtual DbSet<DanhGiaChatLuong> DanhGiaChatLuongs { get; set; }
        public virtual DbSet<GioiTinh> GioiTinhs { get; set; }
        public virtual DbSet<HoiDap> HoiDaps { get; set; }
        public virtual DbSet<Khoa> Khoas { get; set; }
        public virtual DbSet<LichKham> LichKhams { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<QuanTri> QuanTris { get; set; }
        public virtual DbSet<Solieudichbenh> Solieudichbenhs { get; set; }
        public virtual DbSet<TinhThanh> TinhThanhs { get; set; }
        public virtual DbSet<Tintuc> Tintucs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuanTri>()
                .Property(e => e.TaiKhoan)
                .IsFixedLength();

            modelBuilder.Entity<QuanTri>()
                .Property(e => e.MatKhau)
                .IsFixedLength();

            modelBuilder.Entity<QuanTri>()
                .Property(e => e.AnhBia)
                .IsFixedLength();

            modelBuilder.Entity<Solieudichbenh>()
                .Property(e => e.Dichbenh)
                .IsFixedLength();

            modelBuilder.Entity<Tintuc>()
                .Property(e => e.TheLoai)
                .IsFixedLength();
        }
    }
}
