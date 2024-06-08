using Newtonsoft.Json;
using ShopingCartAPI.Iservices;
using ShopingCartAPI.Models.Dto;

namespace ShopingCartAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory clientFactory;

        public CouponService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }


        public async Task<CouponDto> GetCoupon(string Code)
        {
            var client = clientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"code/{Code}");
            var apiContent = await response.Content.ReadAsStringAsync();

            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            
            if(resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result)); 
            }

            return new CouponDto();
        }
    }
}
