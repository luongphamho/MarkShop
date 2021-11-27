using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarkShop.Models;

namespace MarkShop.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly QLSHOPTHOITRANGContext _context;

        public SanPhamsController(QLSHOPTHOITRANGContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var qLSHOPTHOITRANGContext = _context.SanPhams.Include(s => s.MaLoaiSpNavigation).Include(s => s.MaNccNavigation);
            return View(await qLSHOPTHOITRANGContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaLoaiSpNavigation)
                .Include(s => s.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
        public IActionResult Create()
        {
            ViewData["MaLoaiSp"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSp", "MaLoaiSp");
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSp,TenSp,MoTa,GioiTinh,GiaBan,GiaNhap,Anh,MaLoaiSp,MaNcc,SoLuongTon")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiSp"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSp", "MaLoaiSp", sanPham.MaLoaiSp);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", sanPham.MaNcc);
            return View(sanPham);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["MaLoaiSp"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSp", "MaLoaiSp", sanPham.MaLoaiSp);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", sanPham.MaNcc);
            return View(sanPham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSp,TenSp,MoTa,GioiTinh,GiaBan,GiaNhap,Anh,MaLoaiSp,MaNcc,SoLuongTon")] SanPham sanPham)
        {
            if (id != sanPham.MaSp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.MaSp))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiSp"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSp", "MaLoaiSp", sanPham.MaLoaiSp);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", sanPham.MaNcc);
            return View(sanPham);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaLoaiSpNavigation)
                .Include(s => s.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            _context.SanPhams.Remove(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSp == id);
        }
    }
}
