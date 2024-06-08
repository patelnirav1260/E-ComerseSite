using Web.UI.Iservice;
using Web.UI.Models;
using Web.UI.Models.Dto;

namespace Web.UI.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService baseService;

        public ProductService(IBaseService baseService)
        {
            this.baseService = baseService;
        }



        public async Task<ResponseDto> CreateProductAsync(Product product)
            {
                return await baseService.SendAsync(new Request()
                {
                    Method = ApiType.POST,
                    Data = product,
                    Url = APIUrl.Product_Base
                });
            }

            public async Task<ResponseDto> DeleteProductAsync(int id)
            {
                return await baseService.SendAsync(new Request()
                {
                    Method = ApiType.DELETE,
                    Url = APIUrl.Product_Base + id
                });
            }

            public async Task<ResponseDto> GetAllProductAsync()
            {
                return await baseService.SendAsync(new Request()
                {
                    Method = ApiType.GET,
                    Url = APIUrl.Product_Base
                });
            }


            public async Task<ResponseDto> GetProductByIdAsync(int id)
            {
                return await baseService.SendAsync(new Request()
                {
                    Method = ApiType.GET,
                    Url = APIUrl.Product_Base + id
                });
            }

            public async Task<ResponseDto> UpdateProductAsync(Product product, int id)
            {
                return await baseService.SendAsync(new Request()
                {
                    Method = ApiType.PUT,
                    Data = product,
                    Url = APIUrl.Product_Base + id
                });
            }
        }
}
