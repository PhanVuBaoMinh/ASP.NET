using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PhanVuBaoMinh.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100, MinimumLength = 3, ErrorMessage = "Tên sản phẩm phải từ 3 đến 100 ký tự")]
        public required string Name { get; set; }

        [Range(0.01, 10000.00, ErrorMessage = "Giá phải từ 0.01 đến 10000.00")]
        public decimal Price { get; set; }

        [Required, StringLength(500, MinimumLength = 10, ErrorMessage = "Mô tả phải từ 10 đến 500 ký tự")]
        public required string Description { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public string? ImageUrl { get; set; }

        public List<ProductImage>? Images { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm")]
        public int Stock { get; set; }

        public bool IsActive { get; set; } = true;

        [NotMapped]
        public List<IFormFile>? ProductImages { get; set; }

        [NotMapped]
        public string? ExistingImageUrl { get; set; }

        [NotMapped]
        public List<int>? ImagesToDelete { get; set; }

        [NotMapped]
        public string? SelectedImageUrl { get; set; }

        [NotMapped]
        public string CategoryName
        {
            get => Category?.Name ?? "Không có danh mục";
            set
            {
                if (Category != null)
                {
                    Category.Name = value;
                }
            }
        }
    }
}