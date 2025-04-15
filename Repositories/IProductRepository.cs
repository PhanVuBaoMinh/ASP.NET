using PhanVuBaoMinh.Models;
using System.Collections.Generic;

namespace PhanVuBaoMinh.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        Task<Product> GetByIdAsync(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        
        Task AddAsync(Product product);
        Task DeleteAsync(int id);
        Task UpdateAsync(Product product);
    }
}