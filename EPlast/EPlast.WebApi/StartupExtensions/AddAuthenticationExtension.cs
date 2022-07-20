using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EPlast.WebApi.StartupExtensions
{
    public static class AddAuthenticationExtension
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration )
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddGoogle(options =>
                {
                    options.ClientId = configuration.GetSection("GoogleAuthentication:GoogleClientId").Value;
                    options.ClientSecret = configuration.GetSection("GoogleAuthentication:GoogleClientSecret").Value;
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddFacebook(options =>
                {
                    options.AppId = configuration.GetSection("FacebookAuthentication:FacebookAppId").Value;
                    options.AppSecret = configuration.GetSection("FacebookAuthentication:FacebookAppSecret").Value;
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                // WARN: JWT bearer authentication should be removed, because we use Cookie authentication
                // However it is rendered impossible because of how JWT is used everywhere on frontend
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;

                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtIssuerSigningKey")))
                    };
                }); // END WARN

            return services;
        }
    }
}
