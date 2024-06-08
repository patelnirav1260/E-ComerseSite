using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.UI.Models.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; } = 0;
        [Required]
        [Display(Name = "ProductName")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "ProductPrice")]
        public double Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public IFormFile? ImageFile { get; set; }

        public string ImageUrl { get; set; }

        public int? Count { get; set; }

        
    }
}
