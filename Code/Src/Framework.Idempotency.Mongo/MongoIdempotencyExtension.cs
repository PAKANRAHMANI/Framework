using Framework.Core;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Framework.Idempotency.Mongo
{
    public static class MongoIdempotencyExtension
    {
        public static IServiceCollection UseInboxPatternWithMongo(this IServiceCollection services, Action<MongoConfiguration> config)
        {
            var mongoConfiguration = new MongoConfiguration();

            config.Invoke(mongoConfiguration);

            services.AddSingleton(mongoConfiguration);

            services.AddSingleton<IMongoDatabase>(CreateMongoDb(mongoConfiguration));

            services.AddSingleton<IDuplicateMessageHandler, MongoDuplicateMessageHandler>();

            return services;
        }
        private static IMongoDatabase CreateMongoDb(MongoConfiguration mongoConfiguration)
        {
            var client = new MongoClient(mongoConfiguration.Connection);

            return client.GetDatabase(mongoConfiguration.DatabaseName);
        }
    }
}
