using System.ComponentModel.DataAnnotations.Schema;

namespace ShopingCartAPI.Models.Dto
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeader? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? product { get; set; }
        public int Count { get; set; }
    }
}
