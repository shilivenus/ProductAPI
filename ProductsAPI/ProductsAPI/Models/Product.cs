using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DeliveryPrice { get; set; }
        public List<ProductOption> ProductOptions { get; set; }

        public Product() { }

        public Product(Guid id, string name, string description, decimal price, decimal deliveryPrice, List<ProductOption> productOptions = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            DeliveryPrice = deliveryPrice;
            ProductOptions = productOptions;
        }
    }
}
