using ProductsAPI.DTO;
using ProductsAPI.Interface;
using ProductsAPI.Models;
using System.Collections.Generic;

namespace ProductsAPI.Mapper
{
    public class ProductMapper : IMapper
    {
        public ProductOption ToProductOption(ProductOptionDto productOptionDto)
        {
            return new ProductOption(productOptionDto.Id, productOptionDto.ProductId, productOptionDto.Name, productOptionDto.Description);
        }

        public ProductOptionDto ToProductOptionDto(ProductOption productOption)
        {
            return new ProductOptionDto(productOption.Id, productOption.ProductId, productOption.Name, productOption.Description);
        }

        public Product ToProduct(ProductDto productDto)
        {
            var productOptions = new List<ProductOption>();

            if (productDto.ProductOptions?.Count > 0)
            {
                foreach (var optionDto in productDto.ProductOptions)
                {
                    productOptions.Add(ToProductOption(optionDto));
                }
            }

            return new Product(productDto.Id, productDto.Name, productDto.Description, productDto.Price, productDto.DeliveryPrice, productOptions);
        }

        public ProductDto ToProductDto(Product product)
        {
            var productOptionsDto = new List<ProductOptionDto>();

            if (product.ProductOptions?.Count > 0)
            {
                foreach (var option in product.ProductOptions)
                {
                    productOptionsDto.Add(ToProductOptionDto(option));
                }
            }

            return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.DeliveryPrice, productOptionsDto);
        }

        public IList<ProductDto> ToProductDtos(IList<Product> products)
        {
            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                productDtos.Add(ToProductDto(product));
            }

            return productDtos;
        }
    }
}
