using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductsAPI.Controllers;
using ProductsAPI.DTO;
using ProductsAPI.Interface;
using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace ProductsAPITests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productService = new Mock<IProductService>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        [Fact]
        public async void GetAsync_ReturnAllProductsAndStatusOk()
        {
            //Arrange
            var productIdOne = new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9");
            var productIdTwo = new Guid("A21D5777-A655-4020-B431-624BB331E9A2");

            var productsDto = GetProductDtos(productIdOne, productIdTwo);

            var products = new List<Product>();

            _productService.Setup(s => s.FindProductAsync(null)).ReturnsAsync(products);
            _mapper.Setup(m => m.ToProductDtos(It.IsAny<IList<Product>>())).Returns(productsDto);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.GetAsync(null);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var resultproductDtos = (IList<ProductDto>)okResult.Value;
            Assert.Equal(2, resultproductDtos.Count);
            Assert.Equal(2, resultproductDtos[0].ProductOptions.Count);
            Assert.Equal(2, resultproductDtos[1].ProductOptions.Count);
            Assert.Equal(productIdOne, resultproductDtos[0].Id);
            Assert.Equal(productIdOne, resultproductDtos[0].ProductOptions[1].ProductId);
            Assert.Equal(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), resultproductDtos[0].ProductOptions[0].Id);
            Assert.Equal(productIdTwo, resultproductDtos[1].Id);
            Assert.Equal(productIdTwo, resultproductDtos[1].ProductOptions[1].ProductId);
            Assert.Equal(new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6"), resultproductDtos[1].ProductOptions[0].Id);
        }

        [Fact]
        public async void GetAsync_ProductName_ReturnAllProductsWithThisNameAndStatusOk()
        {
            //Arrange
            var productIdOne = new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9");
            var productIdTwo = new Guid("A21D5777-A655-4020-B431-624BB331E9A2");

            var productsDto = GetProductDtos(productIdOne, productIdTwo);
            var productDto = new List<ProductDto>
            {
                productsDto[0]
            };

            var products = new List<Product>();            

            _productService.Setup(s => s.FindProductAsync(It.IsAny<Predicate<Product>>())).ReturnsAsync(products);
            _mapper.Setup(m => m.ToProductDtos(It.IsAny<IList<Product>>())).Returns(productDto);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.GetAsync("test");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var resultproductDtos = (IList<ProductDto>)okResult.Value;
            Assert.Equal(1, resultproductDtos.Count);
            Assert.Equal(2, resultproductDtos[0].ProductOptions.Count);
            Assert.Equal(productIdOne, resultproductDtos[0].Id);
            Assert.Equal(productIdOne, resultproductDtos[0].ProductOptions[1].ProductId);
            Assert.Equal(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), resultproductDtos[0].ProductOptions[0].Id);
        }

        [Fact]
        public async void GetAsync_InvalidName_ReturnStatusNotFound()
        {
            //Arrange
            var productIdOne = new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9");
            var productIdTwo = new Guid("A21D5777-A655-4020-B431-624BB331E9A2");

            var productsDto = GetProductDtos(productIdOne, productIdTwo);
            var productDto = new List<ProductDto>
            {
                productsDto[0]
            };

            var products = new List<Product>();

            _productService.Setup(s => s.FindProductAsync(It.IsAny<Predicate<Product>>())).ReturnsAsync((IList<Product>)null);
            _mapper.Setup(m => m.ToProductDtos(It.IsAny<IList<Product>>())).Returns(productDto);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.GetAsync("test");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void GetByIdAsync_InvalidId_ReturnStatusNotFound()
        {
            //Arrange
            var productIdOne = new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9");
            var productIdTwo = new Guid("A21D5777-A655-4020-B431-624BB331E9A2");

            var productsDto = GetProductDtos(productIdOne, productIdTwo);
            var productDto = new List<ProductDto>
            {
                productsDto[0]
            };

            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);
            _mapper.Setup(m => m.ToProductDtos(It.IsAny<IList<Product>>())).Returns(productDto);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.GetByIdAsync(new Guid());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void GetByIdAsync_ValidId_ReturnProductAndStatusOk()
        {
            //Arrange
            var productIdOne = new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9");
            var productIdTwo = new Guid("A21D5777-A655-4020-B431-624BB331E9A2");

            var productsDto = GetProductDtos(productIdOne, productIdTwo);

            var product = new Product();

            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);
            _mapper.Setup(m => m.ToProductDto(It.IsAny<Product>())).Returns(productsDto[0]);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.GetByIdAsync(new Guid());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var resultproductDto = (ProductDto)okResult.Value;
            Assert.Equal(2, resultproductDto.ProductOptions.Count);
            Assert.Equal(productIdOne, resultproductDto.Id);
            Assert.Equal(productIdOne, resultproductDto.ProductOptions[1].ProductId);
            Assert.Equal(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), resultproductDto.ProductOptions[0].Id);
        }

        [Fact]
        public async void CreateAsync_ProductDto_ReturnUrlPathProductDtoAndCreatedStatus()
        {
            //Arrange
            var productDto = new ProductDto(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), "ProductOne", "ProductOneDescription", (decimal)11.00, (decimal)2.00);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.CreateAsync(productDto);

            //Assert
            var createResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createResult.StatusCode);
            Assert.Equal(":///", createResult.Location);
            var resultproductDto = (ProductDto)createResult.Value;
            Assert.Equal(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), resultproductDto.Id);
            Assert.Equal("ProductOne", resultproductDto.Name);
            Assert.Equal((decimal)11.00, resultproductDto.Price);
        }

        [Fact]
        public async void UpdateAsync_IdAndProductDto_ReturnRowsInsertedAndOkStatus()
        {
            //Arrange
            var productDto = new ProductDto(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), "ProductOne", "ProductOneDescription", (decimal)11.00, (decimal)2.00);

            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());
            _mapper.Setup(m => m.ToProduct(It.IsAny<ProductDto>())).Returns(new Product());
            _productService.Setup(s => s.UpdateProductAsync(It.IsAny<Product>(), null)).ReturnsAsync(1);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.UpdateAsync(new Guid(), productDto);

            //Assert
            var putResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, putResult.StatusCode);
            Assert.Equal(1, putResult.Value);
        }

        [Fact]
        public async void DeleteAsync_InvalidId_ReturnNotFoundStatus()
        {
            //Arrange
            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.DeleteAsync(new Guid());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_ValidId_ReturnNoContentStatus()
        {
            //Arrange
            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.DeleteAsync(new Guid());

            //Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async void DeleteOptionAsync_InvalidId_ReturnNotFoundStatus()
        {
            //Arrange
            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.DeleteOptionAsync(new Guid(), new Guid());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void DeleteOptionAsync_ValidId_ReturnNoContentStatus()
        {
            //Arrange
            var product = new Product
            {
                ProductOptions = new List<ProductOption>() { new ProductOption() }
            };
            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);
            _productService.Setup(s => s.UpdateProductAsync(It.IsAny<Product>(), null)).ReturnsAsync(1);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.DeleteOptionAsync(new Guid(), new Guid());

            //Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async void GetOptionsByProductIdAsync_ValidId_ReturnOptionAndOkStatus()
        {
            //Arrange
            var productIdOne = new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9");
            var productIdTwo = new Guid("A21D5777-A655-4020-B431-624BB331E9A2");

            var productsDto = GetProductDtos(productIdOne, productIdTwo);

            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());
            _mapper.Setup(m => m.ToProductDto(It.IsAny<Product>())).Returns(productsDto[0]);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.GetOptionsByProductIdAsync(new Guid());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var resultproductOptionDtos = (IList<ProductOptionDto>)okResult.Value;
            Assert.Equal(2, resultproductOptionDtos.Count);
            Assert.Equal(productIdOne, resultproductOptionDtos[0].ProductId);
            Assert.Equal(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), resultproductOptionDtos[0].Id);
            Assert.Equal(productIdOne, resultproductOptionDtos[1].ProductId);
            Assert.Equal(new Guid("2FA85F64-5717-4562-B3FC-2C963F66AFA6"), resultproductOptionDtos[1].Id);
        }

        [Fact]
        public async void GetOptionsByOptionIdAsync_ValidIdWithInvalidOptionId_ReturnNotFoundStatus()
        {
            //Arrange
            var productIdOne = new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9");
            var productIdTwo = new Guid("A21D5777-A655-4020-B431-624BB331E9A2");

            var productsDto = GetProductDtos(productIdOne, productIdTwo);

            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());
            _mapper.Setup(m => m.ToProductDto(It.IsAny<Product>())).Returns(productsDto[0]);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.GetOptionsByOptionIdAsync(new Guid(), new Guid());

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void GetOptionsByOptionIdAsync_ValidId_ReturnOptionAndOkStatus()
        {
            //Arrange
            var productIdOne = new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9");
            var productIdTwo = new Guid("A21D5777-A655-4020-B431-624BB331E9A2");

            var productsDto = GetProductDtos(productIdOne, productIdTwo);

            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());
            _mapper.Setup(m => m.ToProductDto(It.IsAny<Product>())).Returns(productsDto[0]);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.GetOptionsByOptionIdAsync(new Guid(), new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"));

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var resultproductOptionDto = (ProductOptionDto)okResult.Value;
            Assert.Equal(productIdOne, resultproductOptionDto.ProductId);
            Assert.Equal(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), resultproductOptionDto.Id);
        }

        [Fact]
        public async void CreateOptionAsync_ValidIdAndProductOptionDto_ReturnUrlPathProductOptionDtoAndCreatedStatus()
        {
            //Arrange
            var productOptionDto = new ProductOptionDto(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9"), "OptionOneOne", "OptionOneOneDescription");

            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.CreateOptionAsync(new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9"), productOptionDto);

            //Assert
            var createResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createResult.StatusCode);
            Assert.Equal(":///", createResult.Location);
            var resultproductOptionDto = (ProductOptionDto)createResult.Value;
            Assert.Equal(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), resultproductOptionDto.Id);
            Assert.Equal(new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9"), resultproductOptionDto.ProductId);
            Assert.Equal("OptionOneOne", resultproductOptionDto.Name);
        }

        [Fact]
        public async void UpdateOptionAsync_InvalidOption_ReturnBadRequestStatus()
        {
            //Arrange
            var productOptionDto = new ProductOptionDto(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9"), "OptionOneOne", "OptionOneOneDescription");

            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.UpdateOptionAsync(productOptionDto);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void UpdateOptionAsync_ValidOption_ReturnRowsInsertedAndOkStatus()
        {
            //Arrange
            var productOptionDto = new ProductOptionDto(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9"), "OptionOneOne", "OptionOneOneDescription");
            var productOptions = new List<ProductOption>()
            {
                new ProductOption(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9"), "OptionOneOne", "OptionOneOneDescription")
            };
            var product = new Product(new Guid("0643CCF0-AB00-4862-B3C5-40E2731ABCC9"), "ProductOne", "ProductOneDescription", (decimal)11.00, (decimal)2.00, productOptions);

            _productService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);
            _productService.Setup(s => s.UpdateProductAsync(It.IsAny<Product>(), null)).ReturnsAsync(1);

            var productsController = new ProductsController(_productService.Object, _mapper.Object);

            //Act
            var result = await productsController.UpdateOptionAsync(productOptionDto);

            //Assert
            var putResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, putResult.StatusCode);
            Assert.Equal(1, putResult.Value);
        }

        private List<ProductDto> GetProductDtos(Guid productIdOne, Guid productIdTwo)
        {
            var productOptionsOneDto = new List<ProductOptionDto>()
            {
                new ProductOptionDto(new Guid("1FA85F64-5717-4562-B3FC-2C963F66AFA6"), productIdOne, "OptionOneOne", "OptionOneOneDescription"),
                new ProductOptionDto(new Guid("2FA85F64-5717-4562-B3FC-2C963F66AFA6"), productIdOne, "OptionOneTwo", "OptionOneTwoDescription")
            };

            var productOptionsTwoDto = new List<ProductOptionDto>()
            {
                new ProductOptionDto(new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6"), productIdTwo, "OptionTwoOne", "OptionTwoOneDescription"),
                new ProductOptionDto(new Guid("4FA85F64-5717-4562-B3FC-2C963F66AFA6"), productIdTwo, "OptionTwoTwo", "OptionTwoTwoDescription")
            };

            var productsDto = new List<ProductDto>()
            {
                new ProductDto(productIdOne, "ProductOne", "ProductOneDescription", (decimal)11.00, (decimal)2.00, productOptionsOneDto),
                new ProductDto(productIdTwo, "ProductTwo", "ProductTwoDescription", (decimal)22.00, (decimal)1.00, productOptionsTwoDto)
            };

            return productsDto;
        }
    }
}
