using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public List<ProductOption> ProductOptions { get; set; }
    }
}
