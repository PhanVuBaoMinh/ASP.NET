using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhanVuBaoMinh.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }  // 👈 Khóa chính

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public string Url { get; set; } = string.Empty; // 👈 Thêm đường dẫn ảnh
    }
}
