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

        public async Task<int> CreateOption(ProductOption productOption)
        {
            await _context.ProductOptions.AddAsync(productOption);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteOption(Guid productOptionId)
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
        public async Task<int> DeleteProduct(Product product)
        {
            if (product.ProductOptions?.Count > 0)
            {
                _context.ProductOptions.RemoveRange(product.ProductOptions);
            }

            _context.Products.RemoveRange(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProducts()
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

        public async Task<Product> GetProductById(Guid id)
        {
            var product = await _context.Products.AsQueryable().Where(p => p.Id == id).FirstOrDefaultAsync();

            var options = await _context.ProductOptions.AsQueryable().Where(p => p.ProductId == id).ToListAsync();

            product.ProductOptions = options;

            return product;
        }

        public async Task<int> UpdateOption(ProductOption productOption)
        {
            var productOptionEntity = await _context.ProductOptions.AsQueryable().Where(p => p.Id == productOption.Id).FirstOrDefaultAsync();

            if(productOptionEntity == null)
            {
                throw new Exception($"{productOption.Id} cannot be found in db");
            }

            productOptionEntity.Name = productOption.Name;
            productOptionEntity.Description = productOption.Description;

            _context.ProductOptions.Update(productOptionEntity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateProduct(Product product)
        {
            var productEntity = await GetProductById(product.Id);

            if(productEntity == null)
            {
                throw new Exception($"{product.Id} cannot be found in db");
            }
            
            productEntity.Name = product.Name;
            productEntity.Description = product.Description;
            productEntity.Price = product.Price;
            productEntity.DeliveryPrice = product.DeliveryPrice;
            productEntity.ProductOptions = product.ProductOptions;

            _context.Products.Update(productEntity);
            return await _context.SaveChangesAsync();
        }
    }
}
