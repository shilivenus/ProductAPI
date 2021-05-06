using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.DTO
{
    public class ProductOptionDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ProductOptionDto(Guid id, Guid productId, string name, string description)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            Description = description;
        }
    }
}
