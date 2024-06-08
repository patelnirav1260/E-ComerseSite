using Web.UI.Iservice;

namespace Web.UI.Service
{
    public class TokenProvide : ITokenProvider
    {
        private readonly IHttpContextAccessor accessor;

        public TokenProvide(IHttpContextAccessor accessor) 
        {
            this.accessor = accessor;
        }
        public void ClearToken()
        {
            accessor.HttpContext?.Response.Cookies.Delete("JwtToken");
        }

        public string GetToken()
        {
            string? token = null;
            bool? hasToken = accessor.HttpContext?.Request.Cookies.TryGetValue("JwtToken", out token);
            return hasToken is true ? token : null;

        }

        public void SetToken(string token)
        {
            accessor.HttpContext?.Response.Cookies.Append("JwtToken", token);

        }
    }
}
