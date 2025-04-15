// File: Areas/Admin/Controllers/OrderController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhanVuBaoMinh.Models; // đường dẫn 
using PhanVuBaoMinh.Data;


namespace PhanVuBaoMinh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.User) // Load thông tin người dùng
                .ToListAsync();
            return View(orders);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order updatedOrder)
        {
            if (ModelState.IsValid)
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.Id == updatedOrder.Id);
                if (order == null) return NotFound();

                order.UserId = updatedOrder.UserId;
                order.OrderDate = updatedOrder.OrderDate;
                order.Total = updatedOrder.Total; // Thay TotalAmount bằng Total
                order.Status = updatedOrder.Status;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(updatedOrder);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}