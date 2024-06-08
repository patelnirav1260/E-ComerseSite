using AutoMapper;
using ShopingCartAPI.Models;
using ShopingCartAPI.Models.Dto;

namespace ShopingCartAPI
{
    public class MappingConfigure: Profile
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>(); 
                config.CreateMap<CartDetails, CartDetailsDto>();
                config.CreateMap<CartHeaderDto, CartHeader>();
                config.CreateMap<CartDetailsDto, CartDetails>();
            });
            return mappingConfig;
        }
    }
}
