using ProductsAPI.DTO;
using ProductsAPI.Models;
using System.Collections.Generic;

namespace ProductsAPI.Interface
{
    public interface IMapper
    {
        ProductOption ToProductOption(ProductOptionDto productOptionDto);
        ProductOptionDto ToProductOptionDto(ProductOption productOption);
        Product ToProduct(ProductDto productDto);
        ProductDto ToProductDto(Product product);
        IList<ProductDto> ToProductDtos(IList<Product> products);
    }
}
