using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services;
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
using EPlast.WebApi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EPlast.DataAccess.Repositories.Realizations.Base;
using EPlast.BLL.Filters;

namespace EPlast.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()
                .Where(x =>
                    x.FullName.Equals("EPlast.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")));

            services.AddDbContextPool<EPlastDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EPlastDBConnection")));

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<EPlastDBContext>()
                    .AddDefaultTokenProviders();

            services.AddLocalization();
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo { Title = "MyApi", Version = "V1" });
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });
                c.OperationFilter<AuthOperationFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
            services.ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);
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
            services.AddScoped<IGlobalLoggerService, GlobalLoggerService>();
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IClubAdministrationService, ClubAdministrationService>();
            services.AddScoped<IClubMembersService, ClubMembersService>();
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
            services.AddScoped<IUserBlobStorageRepository, UserBlobStorageRepository>();
            services.AddSingleton<IAzureBlobConnectionFactory, AzureBlobConnectionFactory>();
            services.AddLogging();

            services.AddAuthentication();
                /*.AddGoogle(options =>
                {
                    options.ClientId = Configuration.GetSection("GoogleAuthentication:GoogleClientId").Value;
                    options.ClientSecret = Configuration.GetSection("GoogleAuthentication:GoogleClientSecret").Value;
                })
                .AddFacebook(options =>
                {
                    options.AppId = Configuration.GetSection("FacebookAuthentication:FacebookAppId").Value;
                    options.AppSecret = Configuration.GetSection("FacebookAuthentication:FacebookAppSecret").Value;
                });*/


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
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

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
        }
    }
}
