using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EPlast.WebApi.StartupExtensions
{
    public static class AddAutoMapperExtension
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()
               .Where(x =>
                   x.FullName.Equals("EPlast.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null") ||
                   x.FullName.Equals("EPlast.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")));

            return services;
        }
    }
}
