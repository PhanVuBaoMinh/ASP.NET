// File: Areas/Admin/Controllers/CategoryController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhanVuBaoMinh.Data;
using PhanVuBaoMinh.Models;

namespace PhanVuBaoMinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context; // Inject DbContext
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync(); // Lấy danh sách danh mục
            return View(categories);
        }

        public IActionResult Add()
        {
            return View(); // Trả về form thêm danh mục
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    }
}