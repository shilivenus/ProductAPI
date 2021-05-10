using ProductsAPI.DTO;
using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsAPI.Interface
{
    public interface IProductService
    {
        Task<IList<Product>> FindProductAsync(Predicate<Product> predicate = null);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<int> CreateProductAsync(Product product);
        Task<int> UpdateProductAsync(Product product, ProductOption productOption = null);
        Task<int> DeleteProductAsync(Guid id);
    }
}
