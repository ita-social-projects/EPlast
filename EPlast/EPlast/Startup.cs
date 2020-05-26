using AutoMapper;
using EPlast.BussinessLayer;
using EPlast.BussinessLayer.AccessManagers;
using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.BussinessLayer.Interfaces;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.BussinessLayer.Services;
using EPlast.BussinessLayer.Services.Events;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.BussinessLayer.Settings;
using EPlast.DataAccess;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models.ViewModelInitializations;
using EPlast.Models.ViewModelInitializations.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using EPlast.BussinessLayer.Interfaces.EventUser;
using EPlast.BussinessLayer.Services.EventUser;

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
                    x.FullName.Equals("EPlast.BussinessLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null") ||
                    x.FullName.Equals("EPlast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")));
            services.AddOptions();
            services.AddDbContextPool<EPlastDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EPlastDBConnection")));
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<EPlastDBContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Admin");
                    });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IEmailConfirmation, EmailConfirmation>();
            services.AddScoped<IAnnualReportVMInitializer, AnnualReportVMInitializer>();
            services.AddScoped<IViewAnnualReportsVMInitializer, ViewAnnualReportsVMInitializer>();
            services.AddScoped<IDecisionVmInitializer, DecisionVmInitializer>();

            services.AddScoped<IPDFService, PDFService>();
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
            services.AddScoped<ICItyAdministrationService, CityAdministrationService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<ICreateEventVMInitializer, CreateEventVMInitializer>();
            services.AddScoped<ICityAccessManagerSettings, CityAccessManagerSettings>();
            services.AddScoped<ICityAccessManager, CityAccessManager>();
            services.AddScoped<IUserAccessManagerSettings, UserAccessManagerSettings>();
            services.AddScoped<IUserAccessManager, UserAccessManager>();
            services.AddScoped<IActionManager, ActionManager>();
            services.AddScoped<IEventCategoryManager, EventCategoryManager>();
            services.AddScoped<IEventTypeManager, EventTypeManager>();
            services.AddScoped<IEventStatusManager, EventStatusManager>();
            services.AddScoped<IParticipantStatusManager, ParticipantStatusManager>();
            services.AddScoped<IParticipantManager, ParticipantManager>();
            services.AddScoped<IEventGalleryManager, EventGalleryManager>();
            services.AddScoped<IEventUserManager, EventUserManager>();
            services.AddScoped<IDateTimeHelper, DateTimeHelper>();
            services.Configure<EmailServiceSettings>(Configuration.GetSection("EmailServiceSettings"));
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
            services.AddAuthentication()
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

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(5);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
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
                UserProfile = new DataAccess.Entities.UserProfile(),
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
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
                new CultureInfo("uk-UA")
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
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            CreateRoles(services).Wait();
        }
    }
}