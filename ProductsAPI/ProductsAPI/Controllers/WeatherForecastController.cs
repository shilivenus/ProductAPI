using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductsAPI.Interface;
using ProductsAPI.Models;
using ProductsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IProductRepository _productRepository;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration, IProductRepository productRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _productRepository = productRepository;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet]
        public Product GetProducts()
        {
            //var Items = new List<Product>();
            //var conn = new SqliteConnection(_configuration.GetConnectionString("ProductDB"));
            //conn.Open();
            //var cmd = conn.CreateCommand();
            //cmd.CommandText = $"select id from Products";

            //var rdr = cmd.ExecuteReader();
            //while (rdr.Read())
            //{
            //    var id = Guid.Parse(rdr.GetString(0));
            //    var product = new Product();
            //    product.Id = id;
            //    Items.Add(product);
            //}

            //conn.Close();
            //var repo = new ProductRepository(_configuration);
            var guid = new Guid();
            var productOptions = new List<ProductOption>();
            var productOption = new ProductOption() { Id = new Guid(), ProductId = guid };
            productOptions.Add(productOption);

            var product = new Product() { Id = guid, Name = "test", Description = "test", Price = (decimal)33.00, DeliveryPrice = (decimal)33.00, ProductOptions = productOptions };
            _productRepository.CreateProduct(product);
            var result = _productRepository.GetProductById(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"));

            return result.Result;
        }
    }
}
