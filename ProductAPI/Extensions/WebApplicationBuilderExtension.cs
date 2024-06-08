using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProductAPI.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder AddAppAuthentica(this WebApplicationBuilder builder) 
        {
            var apiSetting = builder.Configuration.GetSection("ApiSetting");

            var secret = apiSetting.GetValue<string>("Secret");
            var Issure = apiSetting.GetValue<string>("Issuer");
            var Audiance = apiSetting.GetValue<string>("Audiance");

            var key = Encoding.ASCII.GetBytes(secret);


            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Issure,
                    ValidAudience = Audiance,
                    ValidateAudience = true

                };
            });

            return builder;
        }
    }
}
