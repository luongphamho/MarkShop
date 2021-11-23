using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;
namespace MarkShop.Controllers
{
    //public string SessionName = "_Name";
    public class DangNhapController : Controller
    {
        //string? SessionAdmin = default;
        public IActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangNhap(string username, string password)
        {
            if ("admin".Equals(username) && "admin".Equals(password))
            {
                //Session["Admin"] = username;
                HttpContext.Session.SetString("SessionAdmin", username);
                return RedirectToAction("DanhMucCacSanPham", "Admin");
            }
            else
            {
                return View();
            }
        }
    }
}
