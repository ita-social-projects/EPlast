using EPlast.BLL;
using EPlast.BLL.Services.Jwt;
using EPlast.BLL.Settings;
using EPlast.DataAccess;
using EPlast.DataAccess.Entities;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace EPlast.WebApi.StartupExtensions
{
    public static class AddServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddMvc();
            services.AddAutoMapper();
            services.AddHangFire();
            services.AddHangfireServer();
            services.AddAuthentication(Configuration);
            services.AddDataAccess(Configuration);
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<EPlastDBContext>()
                    .AddDefaultTokenProviders();
            services.AddCors();
            services.AddMediatR(typeof(MediatrEntryPoint));
            services.AddSwagger();
            
            services.AddControllers()
                    .AddNewtonsoftJson();
            services.AddLogging();
            services.Configure<EmailServiceSettings>(Configuration.GetSection("EmailServiceSettings"));
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));
            services.AddAuthorization();
            services.AddLocalization();
            services.AddRequestLocalizationOptions();
            services.AddIdentityOptions();
            services.AddRedisOptionExtenshion(Configuration);
            services.AddDependency();

            return services;
        }
    }
}
