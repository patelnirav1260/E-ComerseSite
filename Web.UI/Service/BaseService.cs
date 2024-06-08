using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using Web.UI.Iservice;
using Web.UI.Models.Dto;

namespace Web.UI.Service
{
    public class BaseService : IBaseService
    {
        private readonly HttpClient client;
        private readonly ITokenProvider tokenProvider;

        public BaseService(HttpClient client, ITokenProvider tokenProvider)
        {
            this.client = client;
            this.tokenProvider = tokenProvider;
        }

        public async Task<ResponseDto?> SendAsync(Request request, bool withBearer = true)
        {
            try
            {
                HttpRequestMessage message = new();

                message.Headers.Add("Accept", "application/json");

                message.RequestUri = new Uri(request.Url);

                //token
                if(withBearer)
                {
                    var token = tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }
                if (request.Data != null)
                {
                    var data = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                    message.Content = data;

                }
                HttpResponseMessage? apiResponse = null;

                switch (request.Method)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);


                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);


                if (apiResponse.IsSuccessStatusCode)
                {
                    return new ResponseDto()
                    {
                        Message = responseDto.Message,
                        Result = responseDto.Result,
                        
                    };
                }
                else
                {
                    switch (apiResponse.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return new() { IsSuccess = false, Message = responseDto.Message };
                        case HttpStatusCode.BadRequest:
                            return new() { IsSuccess = false, Message = responseDto.Message };
                        case HttpStatusCode.NoContent:
                            return new() { IsSuccess = false, Message = responseDto.Message };
                        case HttpStatusCode.InternalServerError:
                            return new() { IsSuccess = false, Message = responseDto.Message };
                        case HttpStatusCode.MethodNotAllowed:
                            return new() { IsSuccess = false, Message = responseDto.Message };
                        default:
                            return new() { IsSuccess = false, Message = "Something went wrong" };
                    }
                }

            }
            catch (Exception ex)
            {
                var dto = new ResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
                return dto;
            }

        }
    }
}
