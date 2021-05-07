﻿using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(Guid id);
        Task<int> CreateProduct(Product product);
        Task<int> UpdateProduct(Product product);
        int DeleteProduct(Product product);
        Task<int> CreateOption(ProductOption productOption);
        Task<int> UpdateOption(ProductOption productOption);
        Task<int> DeleteOption(Guid productOptionId);
    }
}
