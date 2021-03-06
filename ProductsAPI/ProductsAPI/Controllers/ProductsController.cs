using Microsoft.AspNetCore.Mvc;
using ProductsAPI.DTO;
using ProductsAPI.Interface;
using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all products based on qurey string name
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>IList of products and httpstatus</returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string name)
        {
            IList<Product> products;

            if (name == null)
            {
                products = await _productService.FindProductAsync();
            }
            else
            {
                products = await _productService.FindProductAsync(p => p.Name.Equals(name));
            }

            if(products == null)
            {
                return NotFound();
            }

            var productDtos = _mapper.ToProductDtos(products);

            return Ok(productDtos);
        }

        /// <summary>
        /// Get product by Id
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>Product and httpstatus</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.ToProductDto(product);

            return Ok(productDto);
        }

        /// <summary>
        /// Create product and its options
        /// </summary>
        /// <param name="productDto">productDto</param>
        /// <returns>url of product and product details</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProductDto productDto)
        {
            var product = _mapper.ToProduct(productDto);

            await _productService.CreateProductAsync(product);

            return Created($"{Request?.Scheme}://{Request?.Host}{Request?.PathBase}{Request?.Path}/{product?.Id}", productDto);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="id">product id</param>
        /// <param name="productDto">product details</param>
        /// <returns>rows updated and http status</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ProductDto productDto)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            productDto.Id = id;

            var productMappered = _mapper.ToProduct(productDto);

            product.Name = productMappered.Name;
            product.Description = productMappered.Description;
            product.Price = productMappered.Price;
            product.DeliveryPrice = productMappered.DeliveryPrice;
            product.ProductOptions = productMappered.ProductOptions;

            var result = await _productService.UpdateProductAsync(product);

            return Ok(result);
        }

        /// <summary>
        /// Delete Product with given Id
        /// </summary>
        /// <param name="id">product Id</param>
        /// <returns>httpstatus</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Delete option with given option id
        /// </summary>
        /// <param name="id">product id</param>
        /// <param name="optionId">option id</param>
        /// <returns>httpstatus</returns>
        [HttpDelete("{id}/options/{optionId}")]
        public async Task<IActionResult> DeleteOptionAsync(Guid id, Guid optionId)
        {
            var product = await _productService.GetProductByIdAsync(id);
            var option = product?.ProductOptions?.Where(p => p.Id == optionId).FirstOrDefault();

            if (option == null)
            {
                return NotFound();
            }

            product.ProductOptions.Remove(option);

            await _productService.UpdateProductAsync(product);

            return NoContent();
        }

        /// <summary>
        /// Get options by product id
        /// </summary>
        /// <param name="id">product it</param>
        /// <returns>List of options</returns>
        [HttpGet("{id}/options")]
        public async Task<IActionResult> GetOptionsByProductIdAsync(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.ToProductDto(product);

            return Ok(productDto.ProductOptions);
        }

        /// <summary>
        /// Get options by option Id
        /// </summary>
        /// <param name="id">product id</param>
        /// <param name="optionId">option id</param>
        /// <returns>Option and httpstatus</returns>
        [HttpGet("{id}/options/{optionId}")]
        public async Task<IActionResult> GetOptionsByOptionIdAsync(Guid id, Guid optionId)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.ToProductDto(product);

            var productOptionDto = productDto?.ProductOptions?.Where(p => p.Id == optionId).FirstOrDefault();

            if(productOptionDto == null)
            {
                return NotFound();
            }

            return Ok(productOptionDto);
        }

        /// <summary>
        /// Create option
        /// </summary>
        /// <param name="id">product id</param>
        /// <param name="productOptionDto">product option details</param>
        /// <returns>Url and product option details</returns>
        [HttpPost("{id}/options")]
        public async Task<IActionResult> CreateOptionAsync(Guid id, [FromBody] ProductOptionDto productOptionDto)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            var productOption = _mapper.ToProductOption(productOptionDto);

            await _productService.UpdateProductAsync(product, productOption);

            return Created($"{Request?.Scheme}://{Request?.Host}{Request?.PathBase}{Request?.Path}/{productOption?.Id}", productOptionDto);
        }

        /// <summary>
        /// Update option
        /// </summary>
        /// <param name="productOptionDto">product option details</param>
        /// <returns>Rows updated and httpstatus</returns>
        [HttpPut("{id}/options/{optionId}")]
        public async Task<IActionResult> UpdateOptionAsync([FromBody] ProductOptionDto productOptionDto)
        {
            var product = await _productService.GetProductByIdAsync(productOptionDto.ProductId);
            var option = product?.ProductOptions?.Where(p => p.Id == productOptionDto.Id).FirstOrDefault();

            if(option == null)
            {
                return NotFound();
            }

            option.Name = productOptionDto.Name;
            option.Description = productOptionDto.Description;

            var result = await _productService.UpdateProductAsync(product);

            return Ok(result);
        }
    }
}
