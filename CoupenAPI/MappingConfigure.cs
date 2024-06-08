using AutoMapper;
using CoupenAPI.Models.Dto;
using CoupenAPI.Models;

namespace CoupenAPI
{
    public class MappingConfigure: Profile
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDto>(); 
                config.CreateMap<CouponDto, Coupon>();
            });
            return mappingConfig;
        }
    }
}
