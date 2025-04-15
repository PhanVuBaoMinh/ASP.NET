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
            Console.WriteLine("Add action called"); // Thêm log
            if (ModelState.IsValid)
            {
                // Xử lý hình ảnh nếu có
                if (product.ProductImages != null && product.ProductImages.Any())
                {
                    var file = product.ProductImages.First();
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        product.ImageUrl = "/images/" + fileName;
                    }
                }

                _productRepository.Add(product);
                return RedirectToAction("Index");
            }
            Console.WriteLine("ModelState is invalid");
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            Console.WriteLine("Edit action called"); 
            if (ModelState.IsValid)
            {
                // Xử lý hình ảnh nếu có
                if (product.ProductImages != null && product.ProductImages.Any())
                {
                    var file = product.ProductImages.First();
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        product.ImageUrl = "/images/" + fileName;
                    }
                }

                _productRepository.Update(product);
                return RedirectToAction("Index");
            }
            Console.WriteLine("ModelState is invalid"); // Thêm log
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
            Console.WriteLine($"DeleteConfirmed action called with id: {id}"); // T
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}