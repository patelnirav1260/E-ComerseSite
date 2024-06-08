using ShopingCartAPI.Models.Dto;

namespace ShopingCartAPI.Iservices
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}
