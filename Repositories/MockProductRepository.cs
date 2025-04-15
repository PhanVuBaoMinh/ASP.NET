//using System.Collections.Generic;
//using System.Linq;
//using PhanVuBaoMinh.Models;

//namespace PhanVuBaoMinh.Repositories
//{
//    public class MockProductRepository : IProductRepository
//    {
//        private readonly List<Product> _products;
//        public MockProductRepository()
//        {
//            // Tạo một số5 dữ liệu mẫHu
//            _products = new List<Product>
//{
//        new Product { Id = 1, Name = "Laptop", Price = 1000,Description = "A high-end laptop"},         
//            // Thếm các sa8n phẫ8m khác
//            };
//        }
//        public IEnumerable<Product> GetAll()
//        {
//            return _products;
//        }
//        public Product GetById(int id)
//        {
//            return _products.Where(p => p.Id == id).DefaultIfEmpty(new Product { Id = 0, Name = "Unknown", Price = 0, Description = "No description" }).First();
//        }
//        public void Add(Product product)
//        {
//            product.Id = _products.Max(p => p.Id) + 1;
//            _products.Add(product);
//        }
//        public void Update(Product product)
//        {
//            var index = _products.FindIndex(p => p.Id == product.Id);
//            if (index != -1)
//            {
//                _products[index] = product;
//            }
//        }
//        public void Delete(int id)
//        {
//            var product = _products.FirstOrDefault(p => p.Id == id);
//            if (product != null)
//            {
//                _products.Remove(product);
//            }
//        }
//    }
//}

