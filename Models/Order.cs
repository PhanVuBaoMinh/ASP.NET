// File: Models/Order.cs
using Microsoft.AspNetCore.Identity;
using PhanVuBaoMinh.Data;

namespace PhanVuBaoMinh.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; } // Đổi tên từ TotalPrice thành Total để đồng bộ với code trước
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
        public ApplicationUser User { get; set; } // Đổi IdentityUser thành ApplicationUser
        public List<OrderDetail> OrderDetails { get; set; }
        public string Status { get; internal set; }
    }
}