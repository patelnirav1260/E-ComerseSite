using Auth_Api.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Auth_Api.IServices
{
    public interface IAuthService
    {
        public Task<string> Registration(RegistrationRequestDto requestDto);

        public Task<LoginResponseDto> Login(LoginRequestDto requestDto);

        public Task<bool> AssignRole(RoleRequestDto requestDto);
    }
}
