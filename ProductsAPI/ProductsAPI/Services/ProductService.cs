using ProductsAPI.Interface;
using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> CreateOptionAsync(Guid id, ProductOption productOption)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            var option = product?.ProductOptions?.Where(p => p.Id == productOption.Id).FirstOrDefault();

            if(option != null)
            {
                throw new Exception($"Cannot create product option of id {productOption.Id}, it already exist.");
            }

            productOption.ProductId = id;

            return await _productRepository.CreateOptionAsync(productOption);
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            var p = await _productRepository.GetProductByIdAsync(product.Id);

            if (p != null)
            {
                throw new Exception($"Cannot create product of id {product.Id}, it already exist.");
            }

            return await _productRepository.CreateProductAsync(product);
        }

        public async Task<int> DeleteOptionAsync(Guid productOptionId)
        {
            return await _productRepository.DeleteOptionAsync(productOptionId);
        }

        public async Task<int> DeleteProductAsync(Guid id)
        {
            var product = _productRepository.GetProductByIdAsync(id);

            return await _productRepository.DeleteProductAsync(product.Result);
        }

        /// <summary>
        /// Find Products by predicate
        /// </summary>
        /// <param name="predicate">predicate of product</param>
        /// <returns>Ilist of Products</returns>
        public async Task<IList<Product>> FindProductAsync(Predicate<Product> predicate = null)
        {
            var products = await _productRepository.GetAllProductsAsync();

            if (predicate == null)
            {
                return products;
            }

            var productsByPredicate = new List<Product>();

            foreach(var product in products.ToList().FindAll(predicate))
            {
                productsByPredicate.Add(product);
            }

            return productsByPredicate;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<int> UpdateOptionAsync(ProductOption productOption)
        {
            return await _productRepository.UpdateOptionAsync(productOption);
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }
    }
}
