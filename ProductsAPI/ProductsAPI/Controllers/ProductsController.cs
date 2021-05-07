using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsAPI.DTO;
using ProductsAPI.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger, IMapper mapper)
        {
            _productService = productService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string name)
        {
            if(name == null)
            {
                var products = _productService.FindProduct(null);

                var productDtos = _mapper.ToProductDtos(products);

                return Ok(productDtos);
            }
            else
            {
                var products = _productService.FindProduct(p => p.Name.Equals(name));

                var productDtos = _mapper.ToProductDtos(products);

                return Ok(productDtos);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var product = _productService.GetProductById(id);

            var productDto = _mapper.ToProductDto(product);

            return Ok(productDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                foreach (var error in errors)
                    _logger.LogError(error.ErrorMessage);

                return BadRequest(ModelState);
            }

            var product = _mapper.ToProduct(productDto);

            var result = await _productService.CreateProduct(product);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                foreach (var error in errors)
                    _logger.LogError(error.ErrorMessage);

                return BadRequest(ModelState);
            }

            productDto.Id = id;

            var product = _mapper.ToProduct(productDto);

            var result = await _productService.UpdateProduct(product);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var product = _productService.GetProductById(id);

            await _productService.CreateProduct(product);

            return NoContent();
        }

        [HttpDelete("{id}/options/{optionId}")]
        public async Task<IActionResult> DeleteOptionAsync(Guid optionId)
        {
            await _productService.DeleteOption(optionId);

            return NoContent();
        }

        [HttpGet("{id}/options")]
        public IActionResult GetOptionsByProductId(Guid id)
        {
            var product = _productService.GetProductById(id);

            var productDto = _mapper.ToProductDto(product);

            return Ok(productDto.ProductOptions);
        }

        [HttpGet("{id}/options/{optionId}")]
        public IActionResult GetOptionsByOptionId(Guid id, Guid optionId)
        {
            var product = _productService.GetProductById(id);

            var productDto = _mapper.ToProductDto(product);

            return Ok(productDto.ProductOptions.Where(p => p.Id == optionId));
        }

        [HttpPost("{id}/options")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOptionAsync(Guid id, [FromBody] ProductOptionDto productOptionDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                foreach (var error in errors)
                    _logger.LogError(error.ErrorMessage);

                return BadRequest(ModelState);
            }

            var productOption = _mapper.ToProductOption(productOptionDto);

            var result = await _productService.CreateOption(id, productOption);

            return Ok(result);
        }

        [HttpPut("{id}/options/{optionId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOptionAsync([FromBody] ProductOptionDto productOptionDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                foreach (var error in errors)
                    _logger.LogError(error.ErrorMessage);

                return BadRequest(ModelState);
            }

            var productOption = _mapper.ToProductOption(productOptionDto);

            var result = await _productService.UpdateOption(productOption);

            return Ok(result);
        }
    }
}
