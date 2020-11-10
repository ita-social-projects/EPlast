using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.Region;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
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
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              IRecurringJobManager recurringJobManager,
                              IServiceProvider serviceProvider)
        {

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
            recurringJobManager.AddOrUpdate("Run every day",
                () => serviceProvider.GetService<IPlastDegreeService>().GetDergeesAsync(),
             "59 23 * * *",
             TimeZoneInfo.Local
             );
            recurringJobManager.AddOrUpdate("Check and change event status",
               () => serviceProvider.GetService<IActionManager>().CheckEventsStatusesAsync(),
            "59 23 * * *",
            TimeZoneInfo.Local
            );
            recurringJobManager.AddOrUpdate("Remove roles from previous admins",
                () => serviceProvider.GetService<ICityAdministrationService>().CheckPreviousAdministratorsToDelete(),
            "59 23 * * *",
            TimeZoneInfo.Local
            );
            recurringJobManager.AddOrUpdate("Changes status of region admins when the date expires",
              () => serviceProvider.GetService<IRegionService>().EndAdminsDueToDate(),
           Cron.Daily(),
           TimeZoneInfo.Local
           );

          CreateRoles(serviceProvider).Wait();
            recurringJobManager.AddOrUpdate("Remove roles from previous admins",
                () => serviceProvider.GetService<IClubAdministrationService>().CheckPreviousAdministratorsToDelete(),
            "59 23 * * *",
            TimeZoneInfo.Local
            );
            CreateRoles(serviceProvider).Wait();
        }
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roles = new[] { "Admin", "Прихильник", "Пластун", "Голова Пласту","Адміністратор подій", "Голова Куреня","Діловод Куреня",
            "Голова Округу","Діловод Округу","Голова Станиці","Діловод Станиці", "Колишній член пласту"};
            foreach (var role in roles)
            {
                if (!(await roleManager.RoleExistsAsync(role)))
                {
                    var idRole = new IdentityRole
                    {
                        Name = role
                    };

                    var res = await roleManager.CreateAsync(idRole);
                }
            }
            var admin = Configuration.GetSection("Admin");
            var profile = new User
            {
                Email = admin["Email"],
                UserName = admin["Email"],
                FirstName = "Admin",
                LastName = "Admin",
                EmailConfirmed = true,
                ImagePath = "default_user_image.png",
                UserProfile = new UserProfile(),
                RegistredOn = DateTime.Now
            };
            if (await userManager.FindByEmailAsync(admin["Email"]) == null)
            {
                var res = await userManager.CreateAsync(profile, admin["Password"]);
                if (res.Succeeded)
                    await userManager.AddToRoleAsync(profile, "Admin");
            }
            else if (!await userManager.IsInRoleAsync(userManager.Users.First(item => item.Email == profile.Email), "Admin"))
            {
                var user = userManager.Users.First(item => item.Email == profile.Email);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
