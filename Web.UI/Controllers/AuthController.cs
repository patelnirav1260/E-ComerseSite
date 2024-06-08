using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Web.UI.Iservice;
using Web.UI.Models.Dto;

namespace Web.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly ITokenProvider tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            this.authService = authService;
            this.tokenProvider = tokenProvider;
        }

        [HttpGet]
        public  IActionResult Login()
        {
           
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            var role = new List<SelectListItem>()
            {
                new SelectListItem(text:GetRole.AdminRole,value:GetRole.AdminRole),
                new SelectListItem(text:GetRole.CustomerRole,value:GetRole.CustomerRole)
            };

            ViewBag.role = role;    
            return View();
        }

        [HttpGet]
        public async Task<IActionResult>  LogOut()
        {
            await HttpContext.SignOutAsync();
            tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await authService.LoginAsync(model);
                if(response != null && response.IsSuccess)
                {
                    string? result = Convert.ToString(response.Result);
                    LoginResponseDto loginResponse =  JsonConvert.DeserializeObject<LoginResponseDto>(result);
                  
                    await SignInUser(loginResponse);
                    tokenProvider.SetToken(loginResponse.Token);


                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = response.Message;
                    TempData["loginError"] = response.Message;

                    return View();
                }
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationRequestDto model)
        {

            var role = new List<SelectListItem>()
            {
                new SelectListItem(text:GetRole.AdminRole,value:GetRole.AdminRole),
                new SelectListItem(text:GetRole.CustomerRole,value:GetRole.CustomerRole)
            };

            ViewBag.role = role;

            ResponseDto response = await authService.RegistrationAsync(model);
                if (response.IsSuccess)
                {
                    if(model.Role != null)
                    {
                    AssignRoleRequestDto assignRoleRequestDto = new AssignRoleRequestDto()
                    {
                        UserName = model.Email,
                        RoleName = model.Role
                        };
                         await authService.AssignRoleAsync(assignRoleRequestDto);
                    }
                    TempData["success"] = "you are successfully registred";
                    return RedirectToAction("Login", "Auth");
                }
                
            TempData["error"] = response.Message;
            return View(model);
        }



        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();


            var jwt = handler.ReadJwtToken(model.Token);


            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);


            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name).Value));


            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(x => x.Type == "role").Value));


            var principal = new ClaimsPrincipal(identity);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }
    }
}
