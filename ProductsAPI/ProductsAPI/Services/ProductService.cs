using ProductsAPI.DTO;
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

        public async Task<int> CreateOption(Guid id, ProductOption productOption)
        {
            productOption.ProductId = id;

            return await _productRepository.CreateOption(productOption);
        }

        public async Task<int> CreateProduct(Product product)
        {
            return await _productRepository.CreateProduct(product);
        }

        public async Task<int> DeleteOption(Guid productOptionId)
        {
            return await _productRepository.DeleteOption(productOptionId);
        }

        public async Task<int> DeleteProduct(Guid id)
        {
            var product = _productRepository.GetProductById(id);

            return await _productRepository.DeleteProduct(product);
        }

        public IList<Product> FindProduct(Predicate<Product> predicate)
        {
            var products = _productRepository.GetAllProducts();

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

        public Product GetProductById(Guid id)
        {
            return _productRepository.GetProductById(id);
        }

        public async Task<int> UpdateOption(ProductOption productOption)
        {
            return await _productRepository.UpdateOption(productOption);
        }

        public async Task<int> UpdateProduct(Product product)
        {
            return await _productRepository.UpdateProduct(product);
        }
    }
}
