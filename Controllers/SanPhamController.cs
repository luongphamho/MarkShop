using MarkShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MarkShop.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly QLSHOPTHOITRANGContext db;
        public SanPhamController(QLSHOPTHOITRANGContext Database)
        {
            db = Database;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SanPhamPartial(int page = 1, int pageSize = 12)
        {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSp);
            var dsSanPham = new Product(db);
            var model = dsSanPham.ListAll(page, pageSize);
            return View(model);
        }
        public IActionResult SanPhamTheoLoai(int maLoaiSP)
        {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSp);
            var dsSPTheoLoai = db.SanPhams.Where(sp => sp.MaLoaiSp == maLoaiSP).OrderBy(sp => sp.GiaBan).ToList();
            if (dsSPTheoLoai.Count == 0)
            {
                ViewBag.thongBao = "Sản phẩm đã hết. Xin quý khách thông cảm";
            }
            return View(dsSPTheoLoai);
        }
        public IActionResult SanPham()
        {
            //SanPham = db.SanPhams()
            var listSanPham = db.SanPhams.OrderBy(sp => sp.TenSp).ToList();
            return View(listSanPham);
        }
        public IActionResult XemChiTiet(int masp)
        {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSp);
            SanPham sanPham = db.SanPhams.Single(s => s.MaSp == masp);
            if (sanPham == null)
            {
                return StatusCode(418);

            }
            return View(sanPham);
        }
        public IActionResult SanPhamTuongTu()
        {
            var listSanPham = db.SanPhams.OrderBy(sp => sp.TenSp).ToList();
            return View(listSanPham);
        }
        public IActionResult timKiemSanPham(string tenSP)
        {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSp);
            if (!string.IsNullOrEmpty(tenSP))
            {
                var query = from sp in db.SanPhams where sp.TenSp.Contains(tenSP) || sp.MaLoaiSpNavigation.TenLoaiSp.Contains(tenSP) select sp;
                if (query.Count() == 0)
                {
                    return RedirectToAction("thongBaoRong", "SanPham");
                }
                return View(query);
            }
            return View();
        }
        public IActionResult thongBaoRong()
        {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSp);
            ViewBag.stringEmpty = "Không tìm thấy sản phẩm";
            return View();
        }
    }
}
