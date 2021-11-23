using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Form()
        {
            return PartialView();
        }

        public IActionResult GioiThieu()
        {
            return View();
        }
    }
}