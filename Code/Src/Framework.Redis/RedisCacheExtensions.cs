using System;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Redis
{
    public static class RedisCacheExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, Action<RedisCacheOptions> options)
        {
            services.AddDistributedRedisCache(options);

            services.AddSingleton<IRedisCache, RedisCache>();

            return services;
        }
    }
}
