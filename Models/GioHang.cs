using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Web;
using MarkShop.Models;

namespace MarkShop.Models
{

    public class GioHang
    {
        
        //readonly QLSHOPTHOITRANGContext db = new QLSHOPTHOITRANGContext();
        private readonly QLSHOPTHOITRANGContext db;
        public GioHang(QLSHOPTHOITRANGContext Database)
        {
            db = Database;
        }
        public int maSP { get; set; }
        public string tenSP { get; set; }
        public string hinhAnh { get; set; }
        public double donGia { get; set; }
        public int soLuong { get; set; }
        //public HasNoKey()
        public double thanhTien
        {
            get { return soLuong * donGia; }
        }

        // Khởi tạo giỏ hàng
        public GioHang(int maSanPham)
        {
            maSP = maSanPham;
            //SanPham sanPham = db.SanPhams.Single(sp => sp.MaSp == maSP);
            SanPham sanPham = this.db.SanPhams.Single(sp => sp.MaSp == maSP);
            tenSP = sanPham.TenSp;
            hinhAnh = sanPham.Anh;
            donGia = double.Parse(sanPham.GiaBan.ToString());
            soLuong = 1;
        }
        public GioHang()
        { }
    }
}