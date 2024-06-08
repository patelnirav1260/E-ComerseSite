using AutoMapper;
using ProductAPI.Models.Dto;
using ProductAPI.Models;

namespace ProductAPI
{
    public class MappingConfigure: Profile
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>(); 
                config.CreateMap<ProductDto, Product>();
            });
            return mappingConfig;
        }
    }
}
