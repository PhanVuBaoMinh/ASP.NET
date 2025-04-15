using Microsoft.AspNetCore.Identity;

namespace PhanVuBaoMinh.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } // Họ
        public string LastName { get; set; }  // Tên
        public int Age { get; set; }          // Tuổi
        public string Address { get; set; }   // Địa chỉ
        public string? AvatarUrl { get; set; } // URL ảnh đại diện
    }
}