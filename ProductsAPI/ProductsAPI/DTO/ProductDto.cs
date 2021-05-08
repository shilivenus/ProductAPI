using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.DTO
{
    public class ProductDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [RegularExpression(@"^(?!^0\.00$)(([1-9][\d]{0,6})|([0]))\.[\d]{2}$")]
        public decimal Price { get; set; }

        [RegularExpression(@"^(?!^0\.00$)(([1-9][\d]{0,6})|([0]))\.[\d]{2}$")]
        public decimal DeliveryPrice { get; set; }
        public IList<ProductOptionDto> ProductOptions { get; set; }

        public ProductDto(Guid id, string name, string description, decimal price, decimal deliveryPrice, IList<ProductOptionDto> productOptions = null)
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
