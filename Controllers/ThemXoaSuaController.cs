using MarkShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MarkShop.Controllers
{
    public class ThemXoaSuaController : Controller
    {
        private readonly QLSHOPTHOITRANGContext db;
        public ThemXoaSuaController(QLSHOPTHOITRANGContext Database)
        {
            db = Database;
        }

        [HttpGet]
        public IActionResult ThemSanPham()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSp), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNcc), "MaNCC", "TenNCC");
            return View();
        }

        [HttpPost]
        public IActionResult ThemSanPham(SanPham sp, IFormFile fUpload)
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSp), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNcc), "MaNCC", "TenNCC");
            if (fUpload != null) // Đã chọn hình ảnh rồi
            {
                if (fUpload.Length > 0)
                {
                    var fileName = Path.GetFileName(fUpload.FileName); // Lấy tên hình ảnh
                    //var path = Path.Combine(Server.MapPath("/Images"), fileName); // Tìm đến thư mục Images trong MarkShop
                    string path = System.IO.Directory.GetCurrentDirectory();
                    if (System.IO.File.Exists(path)) // Nếu trong thư mục Images có hình ảnh đó rồi thì xuất thông báo
                    {
                        TempData["UploadFail"] = "Hình ảnh này đã tồn tại!";
                        return View();
                    }
                    else  // Chưa có hình ảnh trong thư mục Images
                    {
                        using (var file = new FileStream(path, FileMode.Create))
                        {
                            fUpload.CopyTo(file);
                        }    

                            //fUpload.SaveAs(path); // Lưu hình vừa thêm vào thư mục Images
                        sp.Anh = fUpload.FileName; // Cập nhật trong database
                    }
                }
            }
            else
            { // Chưa chọn hình ảnh
                TempData["UploadFail"] = "Vui lòng chọn hình ảnh!";
                return View();
            }
            db.SanPhams.Add(sp);
            db.SaveChanges();
            ModelState.Clear();
            TempData["Added"] = "Thêm sản phẩm thành công";
            return RedirectToAction("DanhMucCacSanPham", "Admin");
        }

        public IActionResult XoaSanPham(int maSP)
        {
            if (HttpContext.Session.GetString("SessionAdmin") == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            SanPham sanPham = db.SanPhams.Single(ma => ma.MaSp == maSP);
            if (sanPham == null)
            {
                return StatusCode(418);
            }

            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("DanhMucCacSanPham", "Admin");
        }

        public IActionResult ChiTietSanPham(int maSP)
        {
            if (HttpContext.Session.GetString("SessionAdmin") == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            SanPham sanPham = db.SanPhams.Single(ma => ma.MaSp == maSP);
            if (sanPham == null)
            {
                return StatusCode(418);

            }
            else
            {
                return View(sanPham);
            }
        }

        [HttpGet]
        public IActionResult SuaSanPham(int id)
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSp), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNcc), "MaNCC", "TenNCC");
            SanPham product = db.SanPhams.SingleOrDefault(n => n.MaSp.Equals(id));
            if (product == null)
            {
                return StatusCode(418);

            }
            return View(product);
        }
        [HttpPost]
        public IActionResult SuaSanPham(SanPham sp, IFormFile fUpload)
        {
            SanPham product = db.SanPhams.SingleOrDefault(n => n.MaSp.Equals(sp.MaSp));
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSp), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNcc), "MaNCC", "TenNCC");
            if (product == null)
            {
                return StatusCode(418);

            }
            product.TenSp = sp.TenSp;
            product.MoTa = sp.MoTa;
            product.GioiTinh = sp.GioiTinh;
            product.GiaBan = sp.GiaBan;
            product.GiaNhap = sp.GiaNhap;
            if (fUpload != null) // Đã chọn hình ảnh rồi
            {
                if (fUpload != null)
                {
                    //var filename = Path.GetFileName(fUpload.FileName);
                    //var path = Path.Combine(Server.MapPath("/Images"), filename);
                    //fUpload.SaveAs(path);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fUpload.FileName);
                    //copy file vao path
                    using (var file = new FileStream(fullPath, FileMode.Create))
                    {
                        fUpload.CopyTo(file);
                    }
                    product.Anh = fUpload.FileName;

                }
            }
            else // Chưa chọn hình cần sửa
            {
                TempData["UploadFail"] = "Vui lòng chọn hình ảnh!";
                return View();
            }
            product.MaLoaiSp = sp.MaLoaiSp;
            product.MaNcc = sp.MaNcc;
            product.SoLuongTon = sp.SoLuongTon;
            db.SaveChanges();
            TempData["Edited"] = "Sửa thông tin sản phẩm thành công";
            return RedirectToAction("DanhMucCacSanPham", "Admin");
        }

        public ActionResult timKiemSanPham(string tenSP)
        {
            if (HttpContext.Session.GetString("SessionAdmin") == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            if (!string.IsNullOrEmpty(tenSP))
            {
                var query = from sp in db.SanPhams where sp.TenSp.Contains(tenSP) || sp.MaLoaiSpNavigation.TenLoaiSp.Contains(tenSP) select sp;
                if (query.Count() == 0)
                {
                    return RedirectToAction("thongBaoRong", "ThemXoaSua");
                }
                return View(query);
            }
            return View();
        }

        public IActionResult thongBaoRong()
        {
            if (HttpContext.Session.GetString("SessionAdmin") == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            ViewBag.stringEmpty = "Không tìm thấy sản phẩm";
            return View();
        }

        private decimal TongDoanhThu()
        {
            decimal TongDoanhThu = 0;
            var check = db.ChiTietHoaDons.Count();
            if (check == 0)
            {
                return TongDoanhThu;
            }
            var cthh = db.ChiTietHoaDons.ToList();
            foreach (var item in cthh)
            {
                TongDoanhThu += (decimal)(item.SoLuong * item.DonGia);
            }
            return TongDoanhThu;
        }

        public IActionResult QuanLiDonHang()
        {
            if (HttpContext.Session.GetString("SessionAdmin") == null)
            {
                return RedirectToAction("DangNhap", "User");
            }
            var loadData = db.ChiTietHoaDons;
            var check = db.ChiTietHoaDons.Count();
            ViewBag.TongDoanhThu = TongDoanhThu();
            return View(loadData);
        }
        public IActionResult DuyetDonHang(int maHD)
        {
            HoaDon hd = db.HoaDons.SingleOrDefault(n => n.MaHd.Equals(maHD));
            hd.TinhTrang = true;
            db.SaveChanges();
            return RedirectToAction("QuanLiDonHang", "ThemXoaSua");
        }
        public IActionResult HuyDH(int maHD)
        {
            HoaDon hd = db.HoaDons.SingleOrDefault(n => n.MaHd.Equals(maHD));
            db.HoaDons.Remove(hd);
            db.SaveChanges();
            return RedirectToAction("QuanLiDonHang", "ThemXoaSua");
        }

        public IActionResult QuanLiKhachHang()
        {
            ViewBag.GetList = from a in db.HoaDons
                              join b in db.KhachHangs
                              on a.MaKh equals b.MaKh
                              select new HDKhachHangModel
                              {
                                  MaKH = b.MaKh,
                                  TenKH = b.TenKh,
                                  TaiKhoan = b.TaiKhoan,
                                  MatKhau = b.MatKhau,
                                  SoDienThoai = b.Sdt,
                                  MaHD = a.MaHd,
                                  TinhTrang = (bool)a.TinhTrang,
                              };
            return View(ViewBag.GetList);
        }
        [HttpPost]
        public IActionResult XoaTaiKhoan(int maKH)
        {
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKh.Equals(maKH));
            db.KhachHangs.Remove(kh);
            //db.Entry(kh).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("QuanLiKhachHang", "ThemXoaSua");
        }
    }
}