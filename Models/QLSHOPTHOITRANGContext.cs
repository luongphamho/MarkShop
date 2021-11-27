using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
#nullable disable

namespace MarkShop.Models
{
    public partial class QLSHOPTHOITRANGContext : DbContext
    {
        /*public QLSHOPTHOITRANGContext()
        {
        }*/

        public QLSHOPTHOITRANGContext(DbContextOptions<QLSHOPTHOITRANGContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer("Default");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.HasKey(e => new { e.MaHd, e.MaSp })
                    .HasName("PK__ChiTietH__F557F661C22C41D6");

                entity.ToTable("ChiTietHoaDon");

                entity.Property(e => e.MaHd).HasColumnName("MaHD");

                entity.Property(e => e.MaSp).HasColumnName("MaSP");

                entity.Property(e => e.DonGia).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.MaHdNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaHd)
                    .HasConstraintName("FK__ChiTietHoa__MaHD__4316F928");

                entity.HasOne(d => d.MaSpNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaSp)
                    .HasConstraintName("FK__ChiTietHoa__MaSP__440B1D61");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.MaHd)
                    .HasName("PK__HoaDon__2725A6E0AE3B3D41");

                entity.ToTable("HoaDon");

                entity.Property(e => e.MaHd).HasColumnName("MaHD");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.NgayDat).HasColumnType("date");

                entity.Property(e => e.NgayGiao).HasColumnType("date");

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaKh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__HoaDon__MaKH__403A8C7D");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.MaKh)
                    .HasName("PK__KhachHan__2725CF1E76FDC66D");

                entity.ToTable("KhachHang");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.DiaChi).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GioiTinh).HasMaxLength(5);

                entity.Property(e => e.MatKhau)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NgaySinh).HasColumnType("datetime");

                entity.Property(e => e.Sdt)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("SDT")
                    .IsFixedLength(true);

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenKh)
                    .HasMaxLength(50)
                    .HasColumnName("TenKH");
            });

            modelBuilder.Entity<LoaiSanPham>(entity =>
            {
                entity.HasKey(e => e.MaLoaiSp)
                    .HasName("PK__LoaiSanP__1224CA7C6A86464F");

                entity.ToTable("LoaiSanPham");

                entity.Property(e => e.MaLoaiSp).HasColumnName("MaLoaiSP");

                entity.Property(e => e.TenLoaiSp)
                    .HasMaxLength(50)
                    .HasColumnName("TenLoaiSP");
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasKey(e => e.MaNcc)
                    .HasName("PK__NhaCungC__3A185DEB3FA44DD0");

                entity.ToTable("NhaCungCap");

                entity.Property(e => e.MaNcc).HasColumnName("MaNCC");

                entity.Property(e => e.DiaChi).HasMaxLength(50);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("SDT")
                    .IsFixedLength(true);

                entity.Property(e => e.TenNcc)
                    .HasMaxLength(50)
                    .HasColumnName("TenNCC");
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.MaSp)
                    .HasName("PK__SanPham__2725081C44DEE804");

                entity.ToTable("SanPham");

                entity.Property(e => e.MaSp).HasColumnName("MaSP");

                entity.Property(e => e.Anh).HasMaxLength(50);

                entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GiaNhap).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GioiTinh).HasMaxLength(5);

                entity.Property(e => e.MaLoaiSp).HasColumnName("MaLoaiSP");

                entity.Property(e => e.MaNcc).HasColumnName("MaNCC");

                entity.Property(e => e.TenSp).HasColumnName("TenSP");

                entity.HasOne(d => d.MaLoaiSpNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaLoaiSp)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__SanPham__MaLoaiS__3C69FB99");

                entity.HasOne(d => d.MaNccNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaNcc)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__SanPham__MaNCC__3D5E1FD2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
