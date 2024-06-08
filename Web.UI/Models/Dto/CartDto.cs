using Web.UI.Models;

namespace Web.UI.Models.Dto
{
    public class CartDto
    {
        public CartHeader CartHeader { get; set; }
        public IEnumerable<CartDetails>? CartDetails { get; set; }
    }
}
