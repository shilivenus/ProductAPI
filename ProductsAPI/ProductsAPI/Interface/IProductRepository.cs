using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid id);
        Task<int> CreateProductAsync(Product product);
        Task<int> UpdateProductAsync(Product product);
        Task<int> DeleteProductAsync(Product product);
        Task<int> CreateOptionAsync(ProductOption productOption);
        Task<int> UpdateOptionAsync(ProductOption productOption);
        Task<int> DeleteOptionAsync(Guid productOptionId);
    }
}
