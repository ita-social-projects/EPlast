using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Settings;
using EPlast.DataAccess;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Extensions;
using EPlast.WebApi.WebSocketHandlers;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace EPlast.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/V1/swagger.json", "MyApi"); });
            }

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
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseWebSockets();
            app.MapWebSocketManager("/notifications", serviceProvider.GetService<UserNotificationHandler>());

            app.UseHangfireDashboard();
            serviceProvider.AddAppRecurringJobs(recurringJobManager, Configuration);

            app.UseHttpsRedirection();
            app.UseHsts();
            app.UseCors();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(b => b.MapControllers());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddNewtonsoftJson();

            services.AddDbContextPool<EPlastDBContext>(o => o.UseSqlServer(
                Configuration.GetConnectionString("EplastDatabase")
            ));

            ConfigureAppSecurity(services);
            Configure3rdPartyServices(services);

            services.AddAppServices();

            services.AddLogging();
            services.Configure<EmailServiceSettings>(Configuration.GetSection("EmailServiceSettings"));

            var supportedCultures = new List<CultureInfo>
            {
                    new CultureInfo("uk-UA"),
                    new CultureInfo("en-US"),
                    new CultureInfo("en"),
                    new CultureInfo("uk")
            };
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(o =>
            {
                o.DefaultRequestCulture = new RequestCulture("uk-UA");
                o.SupportedCultures = supportedCultures;
                o.SupportedUICultures = supportedCultures;
            });
        }

        private void ConfigureAppSecurity(IServiceCollection services)
        {
            services.AddCors(o => o.AddDefaultPolicy(p =>
                {
                    p.WithOrigins(
                        "http://localhost:3000",
                        "https://eplast.westeurope.cloudapp.azure.com",
                        "https://eplastprd.westeurope.cloudapp.azure.com"
                    ); // Frontend URLs
                    p.AllowAnyHeader();
                    p.AllowAnyMethod();
                    p.AllowCredentials();

                    p.SetPreflightMaxAge(TimeSpan.FromDays(1));
                }
            ));

            services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Password.RequiredLength = 8;
                o.SignIn.RequireConfirmedEmail = true;
                o.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<EPlastDBContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(o =>
            {
                o.Cookie.HttpOnly = true;
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                o.Cookie.SameSite = SameSiteMode.Lax;

                o.ExpireTimeSpan = TimeSpan.FromDays(1);

                // This code is to prevent ASP.NET Core Identity from redirecting to /Account/Login page 
                // when Forbiden response is generated (defaut behaviour in MVC projects)
                o.Events.OnRedirectToAccessDenied = ctx =>
                {
                    ctx.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };

                // This code is to prevent ASP.NET Core Identity from redirecting to /Account/Login page 
                // when Unauthorized response is generated (defaut behaviour in MVC projects)
                o.Events.OnRedirectToLogin = ctx =>
                {
                    ctx.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration.GetValue<string>("GoogleAuthentication:GoogleClientId");
                    options.ClientSecret = Configuration.GetValue<string>("GoogleAuthentication:GoogleClientSecret");
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddFacebook(options =>
                {
                    options.AppId = Configuration.GetValue<string>("FacebookAuthentication:FacebookAppId");
                    options.AppSecret = Configuration.GetValue<string>("FacebookAuthentication:FacebookAppSecret");
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                // WARN: JWT bearer authentication should be removed, because we should not use both
                // Cookie authentication and JWT authentication. However it is rendered impossible to 
                // remove JWT right now because of how JWT is used everywhere on frontend
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;

                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtIssuerSigningKey")))
                    };
                });
        }

        private void Configure3rdPartyServices(IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(BLL.Mapping.User.RegisterDtoUser),
                typeof(Mapping.User.UserProfile)
            );

            services.AddHangfire(o =>
                o.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseDefaultTypeSerializer()
                    .UseMemoryStorage()
            );
            services.AddHangfireServer();

            services.AddMediatR(typeof(MediatrEntryPoint));

            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("V1", new OpenApiInfo { Title = "MyApi", Version = "V1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                o.IncludeXmlComments(xmlPath);

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.\nExample: 'Bearer {your token}'"
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }] = Array.Empty<string>()
                });

                o.CustomSchemaIds(x => x.FullName);
            });

            // Redis
            ConfigurationOptions options = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { Configuration.GetConnectionString("Redis") }
            };
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(options));
        }
    }
}
