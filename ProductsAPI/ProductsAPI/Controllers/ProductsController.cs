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
                products = await _productService.FindProduct(null);
            }
            else
            {
                products = await _productService.FindProduct(p => p.Name.Equals(name));
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
            var product = await _productService.GetProductById(id);

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

            await _productService.CreateProduct(product);

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
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return BadRequest($"Product {id} is not exist");
            }

            productDto.Id = id;

            product = _mapper.ToProduct(productDto);

            var result = await _productService.UpdateProduct(product);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return BadRequest($"Product {id} is not exist");
            }

            await _productService.DeleteProduct(id);

            return NoContent();
        }

        [HttpDelete("{id}/options/{optionId}")]
        public async Task<IActionResult> DeleteOptionAsync(Guid id, Guid optionId)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return BadRequest($"Product {id} is not exist");
            }

            await _productService.DeleteOption(optionId);

            return NoContent();
        }

        [HttpGet("{id}/options")]
        public async Task<IActionResult> GetOptionsByProductIdAsync(Guid id)
        {
            var product = await _productService.GetProductById(id);

            if(product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.ToProductDto(product);

            return Ok(productDto.ProductOptions);
        }

        [HttpGet("{id}/options/{optionId}")]
        public async Task<IActionResult> GetOptionsByOptionIdAsync(Guid id, Guid optionId)
        {
            var product = await _productService.GetProductById(id);

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

        [HttpPost("{id}/options")]
        public async Task<IActionResult> CreateOptionAsync(Guid id, [FromBody] ProductOptionDto productOptionDto)
        {
            var product = await _productService.GetProductById(id);

            if(product == null)
            {
                return BadRequest($"Product {id} is not exist");
            }

            var productOption = _mapper.ToProductOption(productOptionDto);

            await _productService.CreateOption(id, productOption);

            return Created($"{Request?.Scheme}://{Request?.Host}{Request?.PathBase}{Request?.Path}/{id}/options/{productOption?.Id}", productOptionDto);
        }

        [HttpPut("{id}/options/{optionId}")]
        public async Task<IActionResult> UpdateOptionAsync([FromBody] ProductOptionDto productOptionDto)
        {
            var product = await _productService.GetProductById(productOptionDto.ProductId);
            var oldOption = product?.ProductOptions?.Where(p => p.Id == productOptionDto.Id).FirstOrDefault();

            if(oldOption == null)
            {
                return BadRequest($"Product {productOptionDto.Id} is not exist");
            }

            var productOption = _mapper.ToProductOption(productOptionDto);

            var result = await _productService.UpdateOption(productOption);

            return Ok(result);
        }
    }
}
