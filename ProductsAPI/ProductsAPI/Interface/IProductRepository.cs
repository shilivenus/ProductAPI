using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsAPI.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid id);
        Task<int> CreateProductAsync(Product product);
        Task<int> UpdateProductAsync(Product product, ProductOption productOption = null);
        Task<int> DeleteProductAsync(Product product);
    }
}
