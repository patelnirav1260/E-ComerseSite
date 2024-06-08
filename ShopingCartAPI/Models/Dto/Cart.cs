namespace ShopingCartAPI.Models.Dto
{
    public class Cart
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
