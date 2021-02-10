using EPlast.BLL.Services.Jwt;
using EPlast.BLL.Settings;
using EPlast.DataAccess;
using EPlast.DataAccess.Entities;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPlast.WebApi.StartupExtensions
{
    public static class AddServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(Configuration);
            services.AddAuthorization();
            services.AddAutoMapper();
            services.AddControllers()
                    .AddNewtonsoftJson();
            services.AddCors();
            services.AddDataAccess(Configuration);
            services.AddHangFire();
            services.AddHangfireServer();
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<EPlastDBContext>()
                    .AddDefaultTokenProviders();
            services.AddIdentityOptions();
            services.AddLocalization();
            services.AddLogging();
            services.AddMvc();
            services.AddRequestLocalizationOptions();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "Redis_";
            });
            services.AddSwagger();
            services.Configure<EmailServiceSettings>(Configuration.GetSection("EmailServiceSettings"));
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));

            return services;
        }
    }
}
