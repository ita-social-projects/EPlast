﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.StartupExtensions
{
    public static class AddRedisOptionsExternshion
    {
        public static IServiceCollection AddRedisOptionExtenshion(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
            return services;
        }
    }
}
