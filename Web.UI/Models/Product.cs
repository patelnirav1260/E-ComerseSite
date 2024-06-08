using System.ComponentModel.DataAnnotations;

namespace Web.UI.Models
{
    public class Product
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
        public string ImageUrl { get; set; }
    }
}
