﻿using MarkShop.Helpers;
using MarkShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly QLSHOPTHOITRANGContext db;
        public AdminController(QLSHOPTHOITRANGContext Database)
        {
            db = Database;
        }
        public IActionResult LayoutAdmin()
        {
            return View();
        }
        public ActionResult TrangPhucNam()
        {
            //SessionHelper.SetObjectAsJson(HttpContext.Session, "product", product);
            //if (Session["Admin"] == null)
            if (HttpContext.Session.GetString("SessionAdmin") == null)
            //HttpContext.Session.SetString(SessionName, "addmin");
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            var listTrangPhucNam = db.SanPhams.OrderBy(sp => sp.MaSp).ToList();
            return View(listTrangPhucNam);
        }
        public ActionResult TrangPhucNu(int page = 1, int pageSize = 12)
        {
            if (HttpContext.Session.GetString("SessionAdmin") == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            var trangPhucNu = new Product();
            var model = trangPhucNu.ListAll(page, pageSize, db);
            return View(model);
        }
        public ActionResult DanhMucCacSanPham(int page = 1, int pageSize = 12)
        {
            if (HttpContext.Session.GetString("SessionAdmin") == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            var sanPham = new Product();
            var model = sanPham.ListAll(page, pageSize, db);
            return View(model);
        }
        public ActionResult DangXuat()
        {
            HttpContext.Session.SetString("SessionAdmin", "");
            return RedirectToAction("GioiThieu", "Home");
        }
    }

}
