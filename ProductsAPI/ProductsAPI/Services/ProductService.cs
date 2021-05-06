using ProductsAPI.DTO;
using ProductsAPI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateOption(Guid id, ProductOptionDto productOptionDto)
        {
            var option = _mapper.ToProductOption(productOptionDto);
            option.ProductId = id;

            return await _productRepository.CreateOption(option);
        }

        public async Task<bool> CreateProduct(ProductDto productDto)
        {
            var product = _mapper.ToProduct(productDto);

            return await _productRepository.CreateProduct(product);
        }

        public async Task<bool> DeleteOption(Guid productOptionId)
        {
            return await _productRepository.DeleteOption(productOptionId);
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var product = await _productRepository.GetProductById(id);

            return await _productRepository.DeleteProduct(product);
        }

        public async Task<IList<ProductDto>> FindProduct(Predicate<ProductDto> predicate)
        {
            var products = await _productRepository.GetAllProducts();

            var productDtos = _mapper.ToProductDtos(products);

            if (predicate == null)
            {
                return productDtos;
            }

            var productDtosByPredicate = new List<ProductDto>();

            foreach(var productDto in productDtos.ToList().FindAll(predicate))
            {
                productDtosByPredicate.Add(productDto);
            }

            return productDtosByPredicate;
        }

        public async Task<ProductDto> GetProductById(Guid id)
        {
            var product = await _productRepository.GetProductById(id);

            return _mapper.ToProductDto(product);
        }

        public async Task<bool> UpdateOption(ProductOptionDto productOptionDto)
        {
            var productOption = _mapper.ToProductOption(productOptionDto);

            return await _productRepository.UpdateOption(productOption);
        }

        public async Task<bool> UpdateProduct(ProductDto productDto)
        {
            var product = _mapper.ToProduct(productDto);

            return await _productRepository.UpdateProduct(product);
        }
    }
}
