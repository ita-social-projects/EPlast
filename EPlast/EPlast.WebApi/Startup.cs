using EPlast.BLL.Services.Jwt;
using EPlast.BLL.Settings;
using EPlast.DataAccess;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Extensions;
using EPlast.WebApi.StartupExtensions;
using EPlast.WebApi.WebSocketHandlers;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;

namespace EPlast.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _secrets = new string[]
            {
                Configuration["StorageConnectionString"],
                Configuration["GoogleAuthentication:GoogleClientSecret"],
                Configuration["GoogleAuthentication:GoogleClientId"],
                Configuration["FacebookAuthentication:FacebookAppSecret"],
                Configuration["FacebookAuthentication:FacebookAppId"],
                Configuration["EmailServiceSettings:SMTPServerPassword"],
                Configuration["EmailServiceSettings:SMTPServerLogin"],
                Configuration["Admin:Password"],
                Configuration["Admin:Email"]
            };
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
            services.AddSwagger();

            services.AddDependency();
            services.AddControllers().AddNewtonsoftJson();
            services.AddLogging();
            services.Configure<EmailServiceSettings>(Configuration.GetSection("EmailServiceSettings"));
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));
            services.AddAuthorization();
            services.AddLocalization();
            services.AddRequestLocalizationOptions();
            services.AddIdentityOptions();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "Redis_";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              IRecurringJobManager recurringJobManager,
                              IServiceProvider serviceProvider)
        {
            serviceProvider.AddRecurringJobsAsync(recurringJobManager, Configuration);
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", "MyApi");
            });

            var supportedCultures = new[]
            {
                new CultureInfo("uk-UA"),
                new CultureInfo("en-US"),
                new CultureInfo("en"),
                new CultureInfo("uk")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("uk-UA"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.ConfigureCustomExceptionMiddleware();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseWebSockets();

            app.MapWebSocketManager("/notifications", serviceProvider.GetService<UserNotificationHandler>());

            //app.UseAntiforgeryTokens();
            app.UseStatusCodePages();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHangfireDashboard();

            app.Run(async (context) =>
            {
                foreach (string secret in _secrets)
                {
                    var result = string.IsNullOrEmpty(secret) ? "Null" : "Not Null";
                    await context.Response.WriteAsync($"Secret is {result}");
                }
            });
        }

        private string[] _secrets = null;
    }
}
