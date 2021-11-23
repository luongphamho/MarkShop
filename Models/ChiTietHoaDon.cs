using System;
using System.Collections.Generic;

#nullable disable

namespace MarkShop.Models
{
    public partial class ChiTietHoaDon
    {
        public int MaHd { get; set; }
        public int MaSp { get; set; }
        public int? SoLuong { get; set; }
        public decimal? DonGia { get; set; }

        public virtual HoaDon MaHdNavigation { get; set; }
        public virtual SanPham MaSpNavigation { get; set; }
    }
}
