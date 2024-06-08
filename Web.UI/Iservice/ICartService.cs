using Web.UI.Models.Dto;

namespace Web.UI.Iservice
{
    public interface ICartService
    {
        public Task<ResponseDto> GetCartByUserIdAsync(string userId);
        public Task<ResponseDto> AddToCartAsync(CartDto cart);
        public Task<ResponseDto> RemoveToCartAsync(int cartDetailsId);
        public Task<ResponseDto> ApplyCouponAsync(CartDto cart);
        public Task<ResponseDto> RemoveCouponAsync(CartDto cart);

    }
}
