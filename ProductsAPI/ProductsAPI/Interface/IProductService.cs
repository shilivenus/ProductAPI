using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Interface
{
    public interface IProductService
    {
        Task<IList<Product>> FindProduct(Predicate<Product> predicate);
        Task<Product> GetProductById(Guid id);
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Guid id);
    }
}
