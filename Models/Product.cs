// File: Models/Product.cs
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
        public required string Name { get; set; } // Tên sản phẩm

        [Range(0.01, 10000.00, ErrorMessage = "Giá phải từ 0.01 đến 10000.00")]
        public decimal Price { get; set; } // Giá sản phẩm

        [Required, StringLength(500, MinimumLength = 10, ErrorMessage = "Mô tả phải từ 10 đến 500 ký tự")]
        public required string Description { get; set; } // Mô tả sản phẩm

        public int CategoryId { get; set; } // ID danh mục

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; } // Liên kết với danh mục

        public string? ImageUrl { get; set; } // URL hình ảnh chính

        public List<ProductImage>? Images { get; set; } // Danh sách hình ảnh phụ

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm")]
        public int Stock { get; set; } // Số lượng tồn kho

        public bool IsActive { get; set; } = true; // Trạng thái sản phẩm (mặc định là đang bán)

        [NotMapped]
        public List<IFormFile>? ProductImages { get; set; } // File upload hình ảnh

        [NotMapped]
        public string? ExistingImageUrl { get; set; } // URL hình ảnh hiện tại khi chỉnh sửa

        [NotMapped]
        public List<int>? ImagesToDelete { get; set; } // ID hình ảnh cần xóa

        [NotMapped]
        public string? SelectedImageUrl { get; set; } // Hình ảnh được chọn

        [NotMapped]
        public string CategoryName
        {
            get => Category?.Name ?? "Không có danh mục"; // Lấy giá trị
            set
            {
                if (Category != null)
                {
                    Category.Name = value; // Gán giá trị
                }
            }
        }
    }
}