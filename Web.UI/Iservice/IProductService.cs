using Web.UI.Models;
using Web.UI.Models.Dto;

namespace Web.UI.Iservice
{
    public interface IProductService
    {
        public Task<ResponseDto> GetAllProductAsync();
        public Task<ResponseDto> GetProductByIdAsync(int id);
        public Task<ResponseDto> CreateProductAsync(Product product);
        public Task<ResponseDto> UpdateProductAsync(Product product, int id);
        public Task<ResponseDto> DeleteProductAsync(int id);
    }
}
