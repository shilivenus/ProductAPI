using ProductsAPI.DTO;
using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsAPI.Interface
{
    public interface IProductService
    {
        Task<IList<Product>> FindProduct(Predicate<Product> predicate);
        Task<Product> GetProductById(Guid id);
        Task<int> CreateProduct(Product product);
        Task<int> UpdateProduct(Product product);
        Task<int> DeleteProduct(Guid id);
        Task<int> CreateOption(Guid id, ProductOption productOption);
        Task<int> UpdateOption(ProductOption productOption);
        Task<int> DeleteOption(Guid productOptionId);
    }
}
