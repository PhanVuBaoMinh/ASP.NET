using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhanVuBaoMinh.Data;
using PhanVuBaoMinh.Models;
using PhanVuBaoMinh.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhanVuBaoMinh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context; // Thêm biến _context

        public ProductController(IProductRepository productRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context; // Tiêm ApplicationDbContext
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Add(product);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Update(product);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}