using Microsoft.Extensions.DependencyInjection;

namespace Framework.Idempotency.Mongo
{
    public static class SqlIdempotencyExtension
    {
        public static IServiceCollection UseInboxPatternWithMongo(this IServiceCollection services, Action<MongoConfiguration> config)
        {
            var sqlConfiguration = new MongoConfiguration();

            config.Invoke(sqlConfiguration);

            services.AddSingleton(sqlConfiguration);

            services.AddSingleton<IDuplicateMessageHandler, MongoDuplicateMessageHandler>();

            return services;
        }
    }
}
