﻿using EPlast.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPlast.WebApi.StartupExtensions
{
    public static class AddDataAccessExtension
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<EPlastDBContext>(o => o.UseSqlServer(configuration.GetConnectionString("EplastDatabase")));

            return services;
        }
    }
}
