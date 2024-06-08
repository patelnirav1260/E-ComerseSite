using Web.UI.Iservice;
using Web.UI.Models.Dto;

namespace Web.UI.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService baseService;

        public AuthService(IBaseService baseService)
        {
            this.baseService = baseService;
        }


        public async Task<ResponseDto> AssignRoleAsync(AssignRoleRequestDto requestDto)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.POST,
                Url = APIUrl.Auth_Base + "assign_role",
                Data = requestDto

            });
        }

        public async  Task<ResponseDto> LoginAsync(LoginRequestDto requestDto)
        {

            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.POST,
                Url = APIUrl.Auth_Base + "login",
                Data = requestDto

            }, withBearer: false);
        }

        public async Task<ResponseDto> RegistrationAsync(RegistrationRequestDto requestDto)
        {
            return await baseService.SendAsync(new Request()
            {
                Method = ApiType.POST,
                Url = APIUrl.Auth_Base + "registration",
                Data = requestDto

            }, withBearer: false);
        }
    }
}
