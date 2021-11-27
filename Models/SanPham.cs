using System;
using System.Collections.Generic;

#nullable disable

namespace MarkShop.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        public int MaSp { get; set; }
        public string TenSp { get; set; }
        public string MoTa { get; set; }
        public string GioiTinh { get; set; }
        public decimal? GiaBan { get; set; }
        public decimal? GiaNhap { get; set; }
        public string Anh { get; set; }
        public int? MaLoaiSp { get; set; }
        public int? MaNcc { get; set; }
        public int? SoLuongTon { get; set; }

        public virtual LoaiSanPham? MaLoaiSpNavigation { get; set; }
        public virtual NhaCungCap? MaNccNavigation { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
