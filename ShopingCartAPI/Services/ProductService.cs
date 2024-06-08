using Newtonsoft.Json;
using ShopingCartAPI.Iservices;
using ShopingCartAPI.Models.Dto;

namespace ShopingCartAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory clientFactory;

        public ProductService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var Apiclient = clientFactory.CreateClient("Product");
            var result = await Apiclient.GetAsync($"");
            var apiContent = await result.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(response.Result));
            }

            return new List<ProductDto>();  
        }
    }
}
