using System;
using Framework.Config;
using Framework.Core;
using MongoDB.Driver;

namespace Framework.DataAccess.Mongo
{
    public class MongodbModule : IFrameworkModule
    {
        private readonly MongoDbConfig _config;

        public MongodbModule(Action<MongoDbConfig> configuration)
        {
            _config = new MongoDbConfig();

            configuration.Invoke(_config);
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped(typeof(MongoDbConfig), _config);

            dependencyRegister.RegisterSingleton<IMongoDatabase>(CreateMongoDb);

            dependencyRegister.RegisterScoped<IMongoContext, MongoContext>();

            dependencyRegister.RegisterScoped<IUnitOfWork, MongoUnitOfWork>();

        }


        private IMongoDatabase CreateMongoDb()
        {
            var client = new MongoClient(_config.ConnectionString);

            return client.GetDatabase(_config.DatabaseName);
        }
    }
}
