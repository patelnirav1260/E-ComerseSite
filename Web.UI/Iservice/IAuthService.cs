using Web.UI.Models.Dto;

namespace Web.UI.Iservice
{
    public interface IAuthService
    {
        public Task<ResponseDto> RegistrationAsync(RegistrationRequestDto requestDto);
        public Task<ResponseDto> LoginAsync(LoginRequestDto requestDto);
        public Task<ResponseDto> AssignRoleAsync(AssignRoleRequestDto requestDto);


    }
}
