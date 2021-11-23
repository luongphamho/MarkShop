﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace MarkShop.Models
{
    public class Product
    {
        //QLSHOPTHOITRANGContext db = new QLSHOPTHOITRANGContext();
        private readonly QLSHOPTHOITRANGContext db;
        public Product(QLSHOPTHOITRANGContext Database)
        {
            db = Database;
        }
        public Product()
        { }
        public IEnumerable<SanPham> ListAll(int page, int pageSize)
        {
            return db.SanPhams.OrderByDescending(sp => sp.MaSp).ToPagedList(page, pageSize);
        }
    }
}