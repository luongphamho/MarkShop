using MarkShop.Helpers;
using MarkShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MarkShop.Controllers
{
    public class GioHangController : Controller
    {
        //readonly QLSHOPTHOITRANGContext db = new QLSHOPTHOITRANGContext();
        // GET: GioHang
        // Tạo đối tượng db chứa dữ liệu từ Model QLBanQuanAo
        private readonly QLSHOPTHOITRANGContext db;
        public GioHangController(QLSHOPTHOITRANGContext Database)
        {
            db = Database;
        }
        public List<GioHang> LayGioHang()
        {
            //List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            //var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            List<GioHang> listGioHang = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "GioHang");
            if (listGioHang == null)
            {
                // Nếu listGioHang chưa tồn tại thì khởi tạo
                listGioHang = new List<GioHang>();
                //listGioHang = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "GioHang");
                SessionHelper.SetObjectAsJson(HttpContext.Session, "GioHang", listGioHang);
            }
            return listGioHang;
        }

        // Xây dựng phương thức thêm vào giỏ hàng
        public ActionResult ThemGioHang(int msp, string strURL)
        {
            // Kiểm tra sản phẩm này trong database đã tồn tại chưa (tránh trường hợp user tự get URL). Nếu k tồn tại => Trang 404
            SanPham product = db.SanPhams.SingleOrDefault(pd => pd.MaSp.Equals(msp));
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // Lấy ra số lượng tồn hiện có hiển thị ra view HetHang
            //Session["SoLuongTonHienCo"] = product.SoLuongTon;
            //HttpContext.session.setstring
            HttpContext.Session.SetInt32("SoLuongTonHienCo", (int)product.SoLuongTon);
            // Lấy ra giỏ hàng
            List<GioHang> listGioHang = LayGioHang();
            // Kiểm tra sản phẩm này có tồn tại trong Session["GioHang"] hay chưa ?
            GioHang item = listGioHang.Find(sp => sp.maSP == msp);
            if (item != null)
            { // Đã có sản phẩm này trong giỏ 
                //Session["TenSP"] = item.tenSP;
                HttpContext.Session.SetString("TenSP", item.tenSP);
                item.soLuong++;
                // Kiểm tra tiếp xem số lượng tồn sản phẩm trong database có nhỏ hơn số lượng sản phẩm thêm hay k. 
                // Nếu nhỏ hơn thì báo hết hàng
                if (product.SoLuongTon < item.soLuong)
                {
                    item.soLuong = 1;
                    TempData["ErrorMessage"] = string.Format("Sản phẩm {0} chỉ còn {1} sản phẩm", HttpContext.Session.GetString("TenSP").ToString(), HttpContext.Session.GetInt32("SoLuongTonHienCo").ToString());
                }
                return Redirect(strURL);
            }
            item = new GioHang(msp);
            listGioHang.Add(item);
            return Redirect(strURL);
        }
        // Xây dựng phương thức tính tổng số lượng
        private int TongSoLuong()
        {
            int tongSoLuong = 0;
            List<GioHang> listGioHang = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "GioHang") as List<GioHang>;
            if (listGioHang != null)
            {
                tongSoLuong = listGioHang.Sum(sp => sp.soLuong);
                //Session.Add("TongSoLuong", tongSoLuong);
                HttpContext.Session.SetInt32("TongSoLuong", tongSoLuong);
            }
            return tongSoLuong;
        }

        // Xây dựng phương thức tính tổng thành tiền
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

        // Xây dựng trang giỏ hàng

        public IActionResult GioHang()
        {
            //List<GioHang> listGioHang = LayGioHang();
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSp);
            if (SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "GioHang") == null)
            {
                //SessionHelper.SetObjectAsJson(HttpContext.Session, "GioHang", 0);
                return RedirectToAction("GioHangTrong", "GioHang");
                //return RedirectToAction("Index", "Home");
                //View(listGioHang);
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

        public IActionResult CapNhatGioHang(int maSP, FormCollection f)
        {
            // Kiểm tra sản phẩm này trong database đã tồn tại chưa (tránh trường hợp user tự get URL). Nếu k tồn tại => Trang 404
            SanPham product = db.SanPhams.SingleOrDefault(pd => pd.MaSp.Equals(maSP));
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // Lấy ra số lượng tồn hiện có hiển thị ra view HetHang
            HttpContext.Session.SetInt32("SoLuongTonHienCo", (int)product.SoLuongTon);
            List<GioHang> listGH = LayGioHang();
            GioHang sp = listGH.Single(s => s.maSP == maSP);
            //Session["TenSP"] = sp.tenSP;
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
            return RedirectToAction("SanPhamPartial", "SanPham");
        }

        public IActionResult XoaGioHang(int maSP)
        {
            List<GioHang> listGH = LayGioHang();
            GioHang sp = listGH.Single(s => s.maSP == maSP);
            if (sp != null)
            {
                listGH.RemoveAll(s => s.maSP == maSP);
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
            //Kiểm tra đăng nhập
            //if (Session["taikhoan"] == null || Session["taikhoan"].ToString() == "")
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
        public IActionResult DatHang(FormCollection f)
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