using MarkShop.Helpers;
using MarkShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarkShop.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        //QLSHOPTHOITRANGContext db = new QLSHOPTHOITRANGContext();
        private readonly QLSHOPTHOITRANGContext db;
        public UserController(QLSHOPTHOITRANGContext Database)
        {
            db = Database;
        }
        public IActionResult DangKy()
        {
            return View();
        }
        public IActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangNhap(Information user, IFormCollection f)
        {
            if (!String.IsNullOrEmpty(user.UserName) && !String.IsNullOrEmpty(user.Password))
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(khachHang => khachHang.TaiKhoan == user.UserName && khachHang.MatKhau == user.Password);
                if (kh != null)
                {
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "taikhoan", kh);
                    HttpContext.Session.SetString("SessionUser", kh.TaiKhoan);
                    return RedirectToAction("SanPhamPartial", "SanPham");
                }
                else
                {
                    ViewBag.ThongBao = "Sai tên đăng nhập hoặc mật khẩu";
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(Information user)
        {
            try
            {
                // Kiểm tra trùng tài khoản
                KhachHang checkKH = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan.Equals(user.tendangnhap));
                if (checkKH != null)
                {
                    TempData["ErrorUserName"] = "Tên tài khoản đã được sử dụng";
                }
                else
                {
                    KhachHang kh = new KhachHang();
                    kh.TenKh = user.tenKhachHang;
                    kh.NgaySinh = DateTime.Parse(user.ngaySinh);
                    kh.GioiTinh = user.gioitinh;
                    kh.Sdt = user.dienthoai;
                    kh.TaiKhoan = user.tendangnhap;
                    kh.MatKhau = user.matkhau;
                    kh.Email = user.email;
                    kh.DiaChi = user.diachi;
                    db.KhachHangs.Add(kh);
                    db.SaveChanges();
                }
                return RedirectToAction("DangNhap", "User");
            }
            catch
            {

            }
            return View();
        }
        public ActionResult DangXuat()
        {
            //ISession sessionAdmin;

            SessionHelper.SetObjectAsJson(HttpContext.Session, "taikhoan", null);
            //HttpContext.Session.SetString("SessionUser", null);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "GioHang", null);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult LichSu()
        {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSp);

            // Nếu như chưa đăng nhập thì quay về trang đăng nhập trong trường hợp user tự động get URL đến trang lịch sử giao dịch
            //if (Session["taikhoan"] == null)
            if (SessionHelper.GetObjectFromJson<KhachHang>(HttpContext.Session, "taikhoan") == null)
            {
                return RedirectToAction("DangNhap", "User");
            }
            else
            {
                // Lấy thông tin khách hàng đã đăng nhập
                KhachHang kh = (KhachHang)SessionHelper.GetObjectFromJson<KhachHang>(HttpContext.Session, "taikhoan");

                // Kiểm tra xem trong hoá đơn khách hàng đó đã đặt hàng hay chưa, nếu chưa đặt hàng thì sao xem lịch sử dc đúng k
                var checkKH = db.HoaDons.Where(n => n.MaKh.Equals(kh.MaKh)).ToList();
                if (checkKH.Count == 0)
                {
                    TempData["Message"] = "Bạn chưa có giao dịch nào";
                    return RedirectToAction("SanPhamPartial", "SanPham");
                }
                else
                { // Đã có trong hoá đơn => tức là đặt hàng rồi

                    // Kiểm tra và lấy hoá đơn + chi tiết hoá đơn ứng mới mã kh đã đăng nhập
                    var listCTGD = db.ChiTietHoaDons.Where(n => n.MaHdNavigation.MaKh.Equals(kh.MaKh));
                    return View(listCTGD);
                }
            }
        }
        [HttpPost]
        public ActionResult HuyDonHang(int maSP, int maHD)
        {
            ChiTietHoaDon cthd = db.ChiTietHoaDons.Single(n => n.MaSp.Equals(maSP) && n.MaHd.Equals(maHD));
            //db.ChiTietHoaDons.Attach(cthd);
            db.ChiTietHoaDons.Remove(cthd);
            db.SaveChanges();
            HoaDon hd = db.HoaDons.SingleOrDefault(n => n.MaHd.Equals(maHD));
            var cthd2 = db.ChiTietHoaDons.Where(n => n.MaHd.Equals(hd.MaHd)).ToList();
            if (cthd2.Count == 0)
            {
                db.HoaDons.Remove(hd);
                db.SaveChanges();
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSp.Equals(maSP));
            sp.SoLuongTon += cthd.SoLuong;
            db.SaveChanges();
            return RedirectToAction("LichSu", "User");
        }
    }
}