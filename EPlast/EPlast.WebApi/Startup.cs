using EPlast.WebApi.Extensions;
using EPlast.WebApi.StartupExtensions;
using EPlast.WebApi.WebSocketHandlers;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;

namespace EPlast.WebApi
{
    public class Startup
    {
        private string[] _secrets = null;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              IRecurringJobManager recurringJobManager,
                              IServiceProvider serviceProvider)
        {
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
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.MapWebSocketManager("/notifications", serviceProvider.GetService<UserNotificationHandler>());

            app.UseHangfireDashboard();
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

            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", "MyApi");
            });
            app.UseWebSockets();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.ConfigureCustomExceptionMiddleware();
                app.UseHsts();
            }

            serviceProvider.AddRecurringJobsAsync(recurringJobManager, Configuration);

            app.Run(async (context) =>
            {
                foreach (string secret in _secrets)
                {
                    var result = string.IsNullOrEmpty(secret) ? "Null" : "Not Null";
                    await context.Response.WriteAsync($"Secret is {result}");
                }
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices(Configuration);
            services.AddDependency();

            _secrets = new string[]
            {
                Configuration["ConnectionStrings:EPlastDBConnection"],
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
        }
    }
}
