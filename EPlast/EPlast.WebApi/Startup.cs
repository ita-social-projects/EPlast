using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer;
using EPlast.BussinessLayer.AccessManagers;
using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.BussinessLayer.Interfaces;
using EPlast.BussinessLayer.Interfaces.City;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.BussinessLayer.Interfaces.EventUser;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.BussinessLayer.Services;
using EPlast.BussinessLayer.Services.City;
using EPlast.BussinessLayer.Services.City.CityAccess;
using EPlast.BussinessLayer.Services.Club;
using EPlast.BussinessLayer.Services.Events;
using EPlast.BussinessLayer.Services.EventUser;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.BussinessLayer.Services.UserProfiles;
using EPlast.BussinessLayer.Settings;
using EPlast.DataAccess;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models.ViewModelInitializations;
using EPlast.Models.ViewModelInitializations.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()
                .Where(x =>
                    x.FullName.Equals("EPlast.BussinessLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null") ||
                    x.FullName.Equals("EPlast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")));

            services.AddDbContextPool<EPlastDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EPlastDBConnection")));

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<EPlastDBContext>()
                    .AddDefaultTokenProviders();

            services.AddLocalization();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApi", Version = "v1" });
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
            services.AddScoped<ICItyAdministrationService, CityAdministrationService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IClubAdministrationService, ClubAdministrationService>();
            services.AddScoped<IClubMembersService, ClubMembersService>();
            services.AddScoped<ICreateEventVMInitializer, CreateEventVMInitializer>();
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
            services.AddScoped<IEventAdminManager, EventAdminManager>();
            services.AddScoped<IDateTimeHelper, DateTimeHelper>();
            services.Configure<EmailServiceSettings>(Configuration.GetSection("EmailServiceSettings"));
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API Version 1");
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            //app.UseCors(builder => builder.AllowAnyOrigin());

        }
    }
}
