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
        [Fact]
        public async void FindProduct_NullPredicate_ReturnAllProducts()
        {
            //Arrange
            var products = new List<Product>()
            {
                new Product(new Guid(), "ProductOne", "ProductOneDescription", (decimal)11.00, (decimal)2.00),
                new Product(new Guid(), "ProductTwo", "ProductTwoDescription", (decimal)22.00, (decimal)1.00)
            };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetAllProducts()).ReturnsAsync(products);

            var service = new ProductService(mockProductRepository.Object);

            //Act
            var result = await service.FindProduct(null);

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

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetAllProducts()).ReturnsAsync(products);

            var service = new ProductService(mockProductRepository.Object);

            //Act
            var result = await service.FindProduct(x => x.Name == "ProductOne");

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(products[0].Name, result[0].Name);
            Assert.Equal(products[0].Description, result[0].Description);
            Assert.Equal(products[0].Price, result[0].Price);
            Assert.Equal(products[0].DeliveryPrice, result[0].DeliveryPrice);
        }
    }
}
