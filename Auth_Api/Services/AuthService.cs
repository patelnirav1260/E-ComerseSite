using Auth_Api.Data;
using Auth_Api.IServices;
using Auth_Api.Models;
using Auth_Api.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth_Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator jwtToken;

        public AuthService(AppDbContext _context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, IJwtTokenGenerator jwtToken)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
            this.jwtToken = jwtToken;
        }

        public async Task<bool> AssignRole(RoleRequestDto requestDto)
        {
            var user =  await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower() == requestDto.UserName.ToLower());

            if (user != null)
            {
                if(!await _roleManager.RoleExistsAsync(requestDto.RoleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(requestDto.RoleName));
                }
                await _userManager.AddToRoleAsync(user, requestDto.RoleName);

                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto requestDto)
        {
           ApplicationUser user =  await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower() == requestDto.Username.ToLower());
           bool isValid = await _userManager.CheckPasswordAsync(user, requestDto.Password);

            if(user == null || isValid == false) 
            { 
                return null;
            }

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };


            var roles = await _userManager.GetRolesAsync(user);
            var token = jwtToken.GenerateToken(user, roles);

            return new LoginResponseDto()
            {
                User = userDto,
                Token = token

            };
        }

        public async Task<string> Registration(RegistrationRequestDto requestDto)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = requestDto.Email,
                Email = requestDto.Email,
                NormalizedEmail = requestDto.Email.ToUpper(),
                Name = requestDto.Name,
                PhoneNumber = requestDto.PhoneNumber,
               
            };

            try 
            {
                var result = await _userManager.CreateAsync(user,requestDto.Password);
                if (result.Succeeded)
                {
                   
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception e) 
            {
                return "something went wrong";
            }
        }
    }
}
