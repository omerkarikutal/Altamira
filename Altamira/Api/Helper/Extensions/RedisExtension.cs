using AutoMapper.Configuration;
using Core.RedisManager;
using Core.Settings.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helper.Extensions
{
    public static class RedisExtension
    {
        public static void AddRedisConfigToServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {

            var redisSettings = new RedisSettings();
            configuration.GetSection(nameof(RedisSettings)).Bind(redisSettings);

            if (!redisSettings.Enabled)
                return;

            services.AddStackExchangeRedisCache(options => options.Configuration = redisSettings.ConnectionString);
            services.AddSingleton<ICacheService, CacheService>();
        }
    }
}
