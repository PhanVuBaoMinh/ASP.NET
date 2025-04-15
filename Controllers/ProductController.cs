using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhanVuBaoMinh.Models;
using PhanVuBaoMinh.Repositories;

namespace PhanVuBaoMinh.Controllers
{

    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        public async Task<IActionResult> AddAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (ModelState.IsValid)
            {
                var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                var imageList = new List<ProductImage>();

                if (product.ProductImages != null && product.ProductImages.Count > 0)
                {
                    foreach (var file in product.ProductImages)
                    {
                        if (file != null && file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(imageFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            if (product.ImageUrl == null)
                            {
                                product.ImageUrl = "/images/" + fileName;
                            }

                            imageList.Add(new ProductImage { Url = "/images/" + fileName });
                        }
                    }
                }

                product.Images = imageList;

                await _productRepository.AddAsync(product);

                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, string? SelectedImageUrl, string[]? ImagesToDelete)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Lấy chỉ số ảnh đại diện do người dùng chọn
                int CoverImageIndex = int.Parse(Request.Form["CoverImageIndex"]);

                var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                var imageList = new List<ProductImage>();

                if (product.ProductImages != null && product.ProductImages.Count > 0)
                {
                    foreach (var file in product.ProductImages)
                    {
                        if (file != null && file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(imageFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            imageList.Add(new ProductImage { Url = "/images/" + fileName });

                            if (string.IsNullOrEmpty(product.ImageUrl))
                            {
                                product.ImageUrl = "/images/" + fileName;
                            }
                        }
                    }
                }

                if (ImagesToDelete != null && ImagesToDelete.Length > 0 && product.Images != null)
                {
                    product.Images = product.Images.Where(img => !ImagesToDelete.Contains(img.Url)).ToList();

                    foreach (var imgUrl in ImagesToDelete)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(SelectedImageUrl))
                {
                    product.ImageUrl = SelectedImageUrl;
                }
                else if (string.IsNullOrEmpty(product.ImageUrl))
                {
                    product.ImageUrl = product.ExistingImageUrl;
                }

                product.Images ??= new List<ProductImage>();
                product.Images.AddRange(imageList);

                await _productRepository.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}