using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services;
using EPlast.BLL.Services.Admin;
using EPlast.BLL.Services.AzureStorage;
using EPlast.BLL.Services.AzureStorage.Base;
using EPlast.BLL.Services.City;
using EPlast.BLL.Services.City.CityAccess;
using EPlast.BLL.Services.Club;
using EPlast.BLL.Services.Events;
using EPlast.BLL.Services.EventUser;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.Logging;
using EPlast.BLL.Services.UserProfiles;
using EPlast.BLL.Settings;
using EPlast.DataAccess;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Realizations.Base;
using EPlast.Models.ViewModelInitializations;
using EPlast.Models.ViewModelInitializations.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()
                .Where(x =>
                    x.FullName.Equals("EPlast.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null") ||
                    x.FullName.Equals("EPlast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")));
            services.AddOptions();
            services.AddDbContextPool<EPlastDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EPlastDBConnection")));
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<EPlastDBContext>()
                    .AddDefaultTokenProviders();

            services.AddLocalization();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Admin");
                    });
            });

            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IEmailConfirmation, EmailConfirmationService>();
            services.AddScoped<IDecisionVmInitializer, DecisionVmInitializer>();
            services.AddScoped<CityAccessSettings, CityAccessSettings>();
            services.AddScoped<ICityAccessService, CityAccessService>();
            services.AddScoped<ICityMembersService, CityMembersService>();
            services.AddScoped<IAnnualReportService, AnnualReportService>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IDecisionService, DecisionService>();
            services.AddScoped<IDirectoryManager, DirectoryManager>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IFileStreamManager, FileStreamManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INationalityService, NationalityService>();
            services.AddScoped<IReligionService, ReligionService>();
            services.AddScoped<IEducationService, EducationService>();
            services.AddScoped<IWorkService, WorkService>();
            services.AddScoped<IGenderService, GenderService>();
            services.AddScoped<IDegreeService, DegreeService>();
            services.AddScoped<IConfirmedUsersService, ConfirmedUsersService>();
            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICityAdministrationService, CityAdministrationService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IAdminTypeService, AdminTypeService>();
            services.AddScoped<IClubAdministrationService, ClubAdministrationService>();
            services.AddScoped<IClubMembersService, ClubMembersService>();
            services.AddScoped<ICreateEventVMInitializer, CreateEventVMInitializer>();
            services.AddScoped<IActionManager, ActionManager>();
            services.AddScoped<IEventCategoryManager, EventCategoryManager>();
            services.AddScoped<IEventTypeManager, EventTypeManager>();
            services.AddScoped<IEventStatusManager, EventStatusManager>();
            services.AddScoped<IEventAdministrationTypeManager, EventAdministrationTypeManager>();
            services.AddScoped<IEventAdmininistrationManager, EventAdministrationManager>();
            services.AddScoped<IParticipantStatusManager, ParticipantStatusManager>();
            services.AddScoped<IParticipantManager, ParticipantManager>();
            services.AddScoped<IEventGalleryManager, EventGalleryManager>();
            services.AddScoped<IEventUserManager, EventUserManager>();
            services.AddScoped<IEventAdminManager, EventAdminManager>();
            services.AddScoped<IDateTimeHelper, DateTimeHelper>();
            services.Configure<EmailServiceSettings>(Configuration.GetSection("EmailServiceSettings"));
            services.AddSingleton<IAzureBlobConnectionFactory, AzureBlobConnectionFactory>();
            services.AddScoped<IUserBlobStorageRepository, UserBlobStorageRepository>();
            services.AddScoped<IDecisionBlobStorageRepository, DecisionBlobStorageRepository>();
            services.AddScoped<ICityBlobStorageRepository, CityBlobStorageRepository>();
            services.AddScoped<IClubBlobStorageRepository, ClubBlobStorageRepository>();
            services.AddScoped<IEventBlobStorageRepository, EventBlobStorageRepository>();
            services.AddSingleton<IAzureBlobConnectionFactory, AzureBlobConnectionFactory>();

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });
            services.AddLogging();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Expiration = TimeSpan.FromDays(5);
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration.GetSection("GoogleAuthentication:GoogleClientId").Value;
                    options.ClientSecret = Configuration.GetSection("GoogleAuthentication:GoogleClientSecret").Value;
                })
                .AddFacebook(options =>
                {
                    options.AppId = Configuration.GetSection("FacebookAuthentication:FacebookAppId").Value;
                    options.AppSecret = Configuration.GetSection("FacebookAuthentication:FacebookAppSecret").Value;
                });

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromHours(3));

            services.Configure<RequestLocalizationOptions>(
             opts =>
             {
                 var supportedCultures = new List<CultureInfo>
                  {
                     new CultureInfo("uk-UA"),
                     new CultureInfo("en-US"),
                     new CultureInfo("en"),
                     new CultureInfo("uk")
                  };

                 opts.DefaultRequestCulture = new RequestCulture("uk-UA");
                 // Formatting numbers, dates, etc.
                 opts.SupportedCultures = supportedCultures;
                 // UI strings that we have localized.
                 opts.SupportedUICultures = supportedCultures;
             });

            services.AddMvc();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roles = new[] { "Admin", "Прихильник", "Пластун", "Голова Пласту","Адміністратор подій", "Голова Куреня","Діловод Куреня",
            "Голова Округу","Діловод Округу","Голова Станиці","Діловод Станиці"};
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
                ImagePath = "default.png",
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStatusCodePagesWithReExecute("/Error/HandleError", "?code={0}");
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
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            CreateRoles(services).Wait();
        }
    }
}