using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhanVuBaoMinh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        // GET: Admin/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: Admin/Add
        [HttpPost]
        public IActionResult Add(string Name)
        {
            TempData["Message"] = $"Đã thêm: {Name}";
            return RedirectToAction("Index");
        }

        // GET: Admin/Edit/5
        public IActionResult Edit(int id)
        {
            ViewBag.Id = id;
            ViewBag.Name = "Dữ liệu mẫu"; // dữ liệu giả
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, string Name)
        {
            TempData["Message"] = $"Đã cập nhật ID {id} thành: {Name}";
            return RedirectToAction("Index");
        }

        // ✅ GET: Admin/Delete/5
        public IActionResult Delete(int id)
        {
            ViewBag.Id = id;
            ViewBag.Name = "Dữ liệu mẫu"; // thay bằng dữ liệu thực
            return View();
        }

        // ✅ POST: Admin/DeleteConfirmed/5 (đổi tên)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            TempData["Message"] = $"Đã xoá ID {id}";
            return RedirectToAction("Index");
        }
    }
}
