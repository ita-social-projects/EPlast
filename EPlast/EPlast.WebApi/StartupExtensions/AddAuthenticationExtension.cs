﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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
                 .AddJwtBearer(config =>
                 {
                     config.RequireHttpsMetadata = false;
                     config.SaveToken = true;

                     config.TokenValidationParameters = new TokenValidationParameters()
                     {
                         ValidIssuer = configuration["Jwt:Issuer"],
                         ValidAudience = configuration["Jwt:Audience"],
                         ValidateLifetime = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                         ClockSkew = TimeSpan.Zero
                     };
                 });

            return services;
        }
    }
}
