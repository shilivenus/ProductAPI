using Moq;
using ProductsAPI.Interface;
using ProductsAPI.Models;
using ProductsAPI.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace ProductsAPITests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();

        [Fact]
        public async void FindProduct_NullPredicate_ReturnAllProducts()
        {
            //Arrange
            var products = new List<Product>()
            {
                new Product(new Guid(), "ProductOne", "ProductOneDescription", (decimal)11.00, (decimal)2.00),
                new Product(new Guid(), "ProductTwo", "ProductTwoDescription", (decimal)22.00, (decimal)1.00)
            };

            _productRepository.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(products);

            var service = new ProductService(_productRepository.Object);

            //Act
            var result = await service.FindProductAsync(null);

            //Assert
            Assert.Equal(products[0].Name, result[0].Name);
            Assert.Equal(products[0].Description, result[0].Description);
            Assert.Equal(products[0].Price, result[0].Price);
            Assert.Equal(products[0].DeliveryPrice, result[0].DeliveryPrice);
            Assert.Equal(products[1].Name, result[1].Name);
            Assert.Equal(products[1].Description, result[1].Description);
            Assert.Equal(products[1].Price, result[1].Price);
            Assert.Equal(products[1].DeliveryPrice, result[1].DeliveryPrice);
        }

        [Fact]
        public async void FindProduct_NamePredicate_ReturnNameMatchedProduct()
        {
            //Arrange
            var products = new List<Product>()
            {
                new Product(new Guid(), "ProductOne", "ProductOneDescription", (decimal)11.00, (decimal)2.00),
                new Product(new Guid(), "ProductTwo", "ProductTwoDescription", (decimal)22.00, (decimal)1.00)
            };

            _productRepository.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(products);

            var service = new ProductService(_productRepository.Object);

            //Act
            var result = await service.FindProductAsync(x => x.Name == "ProductOne");

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(products[0].Name, result[0].Name);
            Assert.Equal(products[0].Description, result[0].Description);
            Assert.Equal(products[0].Price, result[0].Price);
            Assert.Equal(products[0].DeliveryPrice, result[0].DeliveryPrice);
        }

        [Fact]
        public async void CreateOptionAsync_InvalidProductOption_ThrowException()
        {
            //Arrange
            var option = new ProductOption(new Guid(), new Guid(), "test", "test");
            var options = new List<ProductOption>();
            options.Add(option);

            var product = new Product(new Guid(), "ProductOne", "ProductOneDescription", (decimal)11.00, (decimal)2.00, options);

            _productRepository.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);

            var service = new ProductService(_productRepository.Object);

            //Assert
            await Assert.ThrowsAsync<Exception>(() => service.CreateOptionAsync(new Guid(), option));
        }

        [Fact]
        public async void CreateProductAsync_InvalidProduct_ThrowException()
        {
            //Arrange
            _productRepository.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());

            var service = new ProductService(_productRepository.Object);

            //Assert
            await Assert.ThrowsAsync<Exception>(() => service.CreateProductAsync(new Product()));
        }
    }
}
