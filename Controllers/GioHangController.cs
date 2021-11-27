using MarkShop.Helpers;
using MarkShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;
using X.PagedList.Mvc.Core;


namespace MarkShop.Controllers
{
    public class GioHangController : Controller
    {
        private readonly QLSHOPTHOITRANGContext db;
        public GioHangController(QLSHOPTHOITRANGContext Database)
        {
            db = Database;
        }
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGioHang = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "GioHang") as List<GioHang>;
            if (listGioHang == null)
            {
                listGioHang = new List<GioHang>();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "GioHang", listGioHang);
            }
            return listGioHang;
        }

        public IActionResult ThemGioHang(int msp, string strURL)
        {
            SanPham product = db.SanPhams.SingleOrDefault(pd => pd.MaSp.Equals(msp));
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            HttpContext.Session.SetInt32("SoLuongTonHienCo", (int)product.SoLuongTon);
            List<GioHang> listGioHang = LayGioHang();
            GioHang item = listGioHang.Find(sp => sp.maSP == msp);
            if (item != null)
            { 
                HttpContext.Session.SetString("TenSP", item.tenSP);
                item.soLuong++;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "GioHang", listGioHang);
                if (product.SoLuongTon < item.soLuong)
                {
                    item.soLuong = 1;
                    TempData["ErrorMessage"] = string.Format("Sản phẩm {0} chỉ còn {1} sản phẩm", HttpContext.Session.GetString("TenSP").ToString(), HttpContext.Session.GetInt32("SoLuongTonHienCo").ToString());
                }
                return Redirect(strURL);
            }
            item = new GioHang(msp, db);
            listGioHang.Add(item);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "GioHang", listGioHang);
            return Redirect(strURL);
        }
        private int TongSoLuong()
        {
            int tongSoLuong = 0;
            List<GioHang> listGioHang = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "GioHang") as List<GioHang>;
            if (listGioHang != null)
            {
                tongSoLuong = listGioHang.Sum(sp => sp.soLuong);
                HttpContext.Session.SetInt32("TongSoLuong", tongSoLuong);
            }
            return tongSoLuong;
        }

        private double TongThanhTien()
        {
            double tongThanhTien = 0;
            List<GioHang> listGioHang = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "GioHang") as List<GioHang>;
            if (listGioHang != null)
            {
                tongThanhTien += listGioHang.Sum(sp => sp.thanhTien);
            }
            return tongThanhTien;
        }


        public IActionResult GioHang()
        {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSp);
            if (SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "GioHang") == null)
            {
                return RedirectToAction("GioHangTrong", "GioHang");
            }
            List<GioHang> listGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return View(listGioHang);
        }

        public IActionResult GioHangPartial()
        {
            List<GioHang> listGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }

        public IActionResult CapNhatGioHang(int maSP, IFormCollection f)
        {
            SanPham product = db.SanPhams.SingleOrDefault(pd => pd.MaSp.Equals(maSP));
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            HttpContext.Session.SetInt32("SoLuongTonHienCo", (int)product.SoLuongTon);
            List<GioHang> listGH = LayGioHang();
            GioHang sp = listGH.Single(s => s.maSP == maSP);
            HttpContext.Session.SetString("TenSP", sp.tenSP);
            if (sp != null)
            {
                sp.soLuong = int.Parse(f["txtSoLuong"].ToString());
                if (product.SoLuongTon < sp.soLuong)
                {
                    sp.soLuong = 1;
                    TempData["ErrorMessage"] = string.Format("Sản phẩm {0} chỉ còn {1} sản phẩm", HttpContext.Session.GetString("TenSP").ToString(), HttpContext.Session.GetInt32("SoLuongTonHienCo").ToString());

                }
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "GioHang", listGH);
            return RedirectToAction("SanPhamPartial", "SanPham");
        }

        public IActionResult XoaGioHang(int maSP)
        {
            List<GioHang> listGH = LayGioHang();
            GioHang sp = listGH.Single(s => s.maSP == maSP);
            if (sp != null)
            {
                listGH.RemoveAll(s => s.maSP == maSP);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "GioHang", listGH);
                return RedirectToAction("SanPhamPartial", "SanPham");
            }
            if (listGH.Count == 0)
            {
                return RedirectToAction("SanPhamPartial", "SanPham");
            }
            return RedirectToAction("SanPhamPartial", "SanPham");
        }

        public IActionResult XoaGioHangAll()
        {
            List<GioHang> listGH = LayGioHang();
            listGH.Clear();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "GioHang", listGH);
            if (listGH.Count == 0)
            {
                return RedirectToAction("SanPhamPartial", "SanPham");
            }
            return RedirectToAction("SanPhamPartial", "SanPham");
        }

        public IActionResult GioHangTrong()
        {
            ViewBag.thongBao = "Your cart is empty";
            return View();
        }

        public IActionResult ViewGioHangHover()
        {
            List<GioHang> listGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.thongBao = "Your cart is empty";
            ViewBag.TongThanhTien = TongThanhTien();
            return View(listGioHang);
        }

        [HttpGet]
        public IActionResult DatHang()
        {   
            if (HttpContext.Session.GetString("taikhoan") == null)
            {
                return RedirectToAction("DangNhap", "User");
            }
            if (SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "GioHang") == null)
            {
                return RedirectToAction("SanPhamPartial", "SanPham");
            }

            // Lấy giỏ hàng từ Session
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            if (ViewBag.TongSoLuong == 0)
            {
                return RedirectToAction("SanPhamPartial", "SanPham");
            }
            ViewBag.TongThanhTien = TongThanhTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public IActionResult DatHang(IFormCollection f)
        {
            // Thêm đơn hàng
            HoaDon ddh = new HoaDon();
            //KhachHang kh = (KhachHang)Session["taikhoan"];
            //SessionHelper.SetObjectAsJson(HttpContext.Session, "taikhoan", KhachHang kh);
            KhachHang kh = (KhachHang)SessionHelper.GetObjectFromJson<KhachHang>(HttpContext.Session, "taikhoan");
            List<GioHang> gh = LayGioHang();
            ddh.MaKh = kh.MaKh;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = String.Format("{0:mm/dd/yyyy}", f["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(NgayGiao);
            ddh.TinhTrang = false;
            //db.HoaDons.InsertOnSubmit(ddh);
            db.HoaDons.Add(ddh);
            //db.SubmitChanges();
            db.SaveChanges();
            //Session.Add("NgayGiao", (string)(ddh.NgayGiao));
            //SessionHelper.SetObjectAsJson(HttpContext.Session, NgayGiao, ddh.NgayGiao);
            HttpContext.Session.SetString("NgayGiao", ddh.NgayGiao.ToString());
            //Session.Add("MaHD", ddh.MaHd);
            HttpContext.Session.SetInt32("MaHD", ddh.MaHd);
            foreach (var item in gh)
            {
                ChiTietHoaDon ctdh = new ChiTietHoaDon();
                ctdh.MaHd = ddh.MaHd;
                ctdh.MaSp = item.maSP;
                ctdh.SoLuong = item.soLuong;
                ctdh.DonGia = (decimal)item.donGia;
                db.ChiTietHoaDons.Add(ctdh);
                // Lấy mã sản phẩm trong database
                SanPham sp = db.SanPhams.SingleOrDefault(i => i.MaSp == item.maSP);
                // Cập nhật số lượng tồn trong database
                sp.SoLuongTon = sp.SoLuongTon - item.soLuong;
            }
            db.SaveChanges();
            return RedirectToAction("XacNhanDatHang", "GioHang");
        }

        public IActionResult XacNhanDatHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            //Session["GioHang"] = null;
            //SessionHelper.SetObjectFromJson<List<GioHang>>(HttpContext.Session, "GioHang")
            SessionHelper.SetObjectAsJson(HttpContext.Session, "GioHang", null);
            return View(lstGioHang);
        }
    }
}