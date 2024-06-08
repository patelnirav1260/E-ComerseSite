using Web.UI.Iservice;
using Web.UI.Models;
using Web.UI.Models.Dto;

namespace Web.UI.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService baseService;

        public CartService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto> AddToCartAsync(CartDto cart)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.POST,
                Data = cart,
                Url = APIUrl.ShopingCart_Base + "api/cart/CartUpsert"
            });
        }

        public async Task<ResponseDto> ApplyCouponAsync(CartDto cart)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.POST,
                Data = cart,
                Url = APIUrl.ShopingCart_Base + "api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDto> GetCartByUserIdAsync(string userId)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.GET,
                Url = APIUrl.ShopingCart_Base + "api/cart/" + userId
            });
        }

        public async Task<ResponseDto> RemoveCouponAsync(CartDto cart)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.POST,
                Data = cart,
                Url = APIUrl.ShopingCart_Base + "api/cart/RemoveCoupon/"
            });
        }

        public async Task<ResponseDto> RemoveToCartAsync(int CartDetailsId)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.POST,
                Data = CartDetailsId,
                Url = APIUrl.ShopingCart_Base + "api/cart/GetCart/"
            });
        }
    }
}
