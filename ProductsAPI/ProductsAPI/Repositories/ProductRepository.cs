using Microsoft.EntityFrameworkCore;
using ProductsAPI.DataAccess;
using ProductsAPI.Interface;
using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOptionAsync(ProductOption productOption)
        {
            await _context.ProductOptions.AddAsync(productOption);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteOptionAsync(Guid productOptionId)
        {
            var option = _context.ProductOptions.AsQueryable().Where(p => p.Id == productOptionId).FirstOrDefault();
            
            _context.ProductOptions.Remove(option);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete product and its options
        /// </summary>
        /// <param name="product">product</param>
        /// <returns>rows affected</returns>
        public async Task<int> DeleteProductAsync(Product product)
        {
            if (product.ProductOptions?.Count > 0)
            {
                _context.ProductOptions.RemoveRange(product.ProductOptions);
            }

            _context.Products.RemoveRange(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _context.Products.AsQueryable().ToListAsync();

            var result = new List<Product>();

            foreach (var product in products)
            {
                var options = await _context.ProductOptions.AsQueryable().Where(p => p.ProductId == product.Id).ToListAsync();
                product.ProductOptions = options;
                result.Add(product);
            }

            return result;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products.AsQueryable().Where(p => p.Id == id).FirstOrDefaultAsync();

            var options = await _context.ProductOptions.AsQueryable().Where(p => p.ProductId == id).ToListAsync();

            if(product!=null)
            {
                product.ProductOptions = options;
            }

            return product;
        }

        public async Task<int> UpdateOptionAsync(ProductOption productOption)
        {
            _context.ProductOptions.Update(productOption);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync();
        }
    }
}
