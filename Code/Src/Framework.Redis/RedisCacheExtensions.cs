using System;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Redis
{
    public static class RedisCacheExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, Action<RedisCacheConfiguration> options)
        {
            var redisCacheConfiguration = new RedisCacheConfiguration();

            options.Invoke(redisCacheConfiguration);

            services.AddSingleton(redisCacheConfiguration);

            services.AddSingleton<IRedisCache, RedisCache>();

            services.AddSingleton<IRedisHelper, RedisHelper>();

            return services;
        }
    }
}
