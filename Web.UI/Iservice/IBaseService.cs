using Web.UI.Models.Dto;

namespace Web.UI.Iservice
{
    public interface IBaseService
    {
        public Task<ResponseDto?> SendAsync(Request request, bool withBearer = true);
    }
}
