using Web.UI.Models;
using Web.UI.Models.Dto;

namespace Web.UI.Iservice
{
    public interface ICouponService
    {
        public Task<ResponseDto> GetCouponAsync(string coupon);
        public Task<ResponseDto> GetAllCouponAsync();
        public Task<ResponseDto> GetCouponByIdAsync(int id);
        public Task<ResponseDto> CreateCouponAsync(Coupon coupon);
        public Task<ResponseDto> UpdateCouponAsync(int id, string coupon);
        public Task<ResponseDto> DeleteCouponAsync(int id);


        
    }
}
