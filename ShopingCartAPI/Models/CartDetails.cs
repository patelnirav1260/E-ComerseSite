using System.ComponentModel.DataAnnotations.Schema;
using ShopingCartAPI.Models.Dto;

namespace ShopingCartAPI.Models
{
    public class CartDetails
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set;}
        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set;}
        public int ProductId { get; set;}
        [NotMapped]
        public ProductDto product { get; set;}
        public int Count { get; set;}   

    }
}
