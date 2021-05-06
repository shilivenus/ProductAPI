using ProductsAPI.DTO;
using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsAPI.Interface
{
    public interface IProductService
    {
        Task<IList<ProductDto>> FindProduct(Predicate<ProductDto> predicate);
        Task<ProductDto> GetProductById(Guid id);
        Task<bool> CreateProduct(ProductDto productDto);
        Task<bool> UpdateProduct(ProductDto productDto);
        Task<bool> DeleteProduct(Guid id);
        Task<bool> CreateOption(Guid id, ProductOptionDto productOptionDto);
        Task<bool> UpdateOption(ProductOptionDto productOptionDto);
        Task<bool> DeleteOption(Guid productOptionId);
    }
}
