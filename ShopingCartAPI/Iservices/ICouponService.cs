using ShopingCartAPI.Models.Dto;

namespace ShopingCartAPI.Iservices
{
    public interface ICouponService
    {
        public Task<CouponDto> GetCoupon(string couponCode);

    }
}
