using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;

namespace EPlast.WebApi.StartupExtensions
{
    public static class AddRequestLocalizationOptionsExtension
    {
        public static IServiceCollection AddRequestLocalizationOptions(this IServiceCollection services)
        {
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

            return services;
        }
    }
}
