using Auth_Api.Data;
using Auth_Api.IServices;
using Auth_Api.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth_Api.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ResponseDto responseDto;

        public AuthAPIController(IAuthService authService, ResponseDto responseDto)
        {
            this.authService = authService;
            this.responseDto = responseDto;
        }


        [HttpPost("/registration")]
        public async Task<IActionResult> Registration(RegistrationRequestDto requestDto)
        {
            var result = await authService.Registration(requestDto);

            if (!result.IsNullOrEmpty())
            {
                responseDto.IsSuccess = false;
                responseDto.Message = result;

                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var result = await authService.Login(loginRequestDto);

            if(result == null)
            {
               responseDto.IsSuccess=false;
                responseDto.Message = "invalid username or password";
                return BadRequest(responseDto);
            }
            responseDto.Result = result;
            return Ok(responseDto);
        }


        [HttpPost("/assign_role")]
        public async Task<IActionResult> AssignRole(RoleRequestDto requestDto)
        {
            var result = await authService.AssignRole(requestDto);

            if (result)
            {
                responseDto.Message = "your role assigned successfully";
                return Ok(responseDto);
            }
            responseDto.Message = "faild to assign role";
            responseDto.IsSuccess = true;
            return BadRequest(responseDto);
        }
    }
}
