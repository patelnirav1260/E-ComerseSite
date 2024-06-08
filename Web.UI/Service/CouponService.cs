using Web.UI.Iservice;
using Web.UI.Models;
using Web.UI.Models.Dto;

namespace Web.UI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService baseService;

        public CouponService(IBaseService baseService) 
        {
            this.baseService = baseService;
        }



        public async Task<ResponseDto> CreateCouponAsync(Coupon coupon)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.POST,
                Data = coupon,
                Url = APIUrl.CouponBase
            }) ;
        }

        public async Task<ResponseDto> DeleteCouponAsync(int id)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.DELETE,
                Url = APIUrl.CouponBase + id    
            });
        }

        public async Task<ResponseDto> GetAllCouponAsync()
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.GET,
                Url = APIUrl.CouponBase
            });
        }

        public async Task<ResponseDto> GetCouponAsync(string coupon)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.GET,
                Url = APIUrl.CouponBase,
                Data = coupon
            });
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.GET,
                Url = APIUrl.CouponBase + id
            });
        }

        public async Task<ResponseDto> UpdateCouponAsync(int id, string coupon)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.PUT,
                Data = coupon,
                Url = APIUrl.CouponBase + id
            });
        }
    }
}
