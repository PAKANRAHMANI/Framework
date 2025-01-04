using System;
using Framework.Config;
using Framework.Core;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

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


        public IMongoDatabase CreateMongoDb()
        {
            if (_config.UseUrl)
            {
                var client = new MongoClient(_config.ConnectionString);

                return client.GetDatabase(_config.DatabaseName);
            }
            else
            {
                var setting = new MongoClientSettings();

                var identity = new MongoInternalIdentity(_config.DatabaseName, _config.ClientSettings.UserName);

                var evidence = new PasswordEvidence(_config.ClientSettings.Password);

                setting.Credential = new MongoCredential("SCRAM-SHA-256", identity, evidence);

                setting.MaxConnecting = _config.ClientSettings.MaxConnecting;

                setting.MaxConnectionPoolSize = _config.ClientSettings.MaxConnectionPoolSize;

                setting.ReadConcern = new ReadConcern(_config.ClientSettings.ReadConcern);

                if (_config.ClientSettings is { ReadPreference: > 0 })
                    setting.ReadPreference = new ReadPreference(_config.ClientSettings.ReadPreference.Value, null, TimeSpan.FromSeconds(_config.ClientSettings.MaxStaleness));

                setting.RetryReads = _config.ClientSettings.RetryReads;

                setting.RetryWrites = _config.ClientSettings.RetryWrites;

                setting.Server = new MongoServerAddress(_config.ClientSettings.Host, _config.ClientSettings.Port);

                setting.DirectConnection = _config.ClientSettings.DirectConnection;

                setting.ConnectTimeout = TimeSpan.FromMilliseconds(_config.ClientSettings.ConnectTimeout);

                setting.SocketTimeout = TimeSpan.FromMilliseconds(_config.ClientSettings.SocketTimeout);

                setting.MinConnectionPoolSize = _config.ClientSettings.MinConnectionPoolSize;

                setting.Scheme = ConnectionStringScheme.MongoDB;

                var client = new MongoClient(setting);
                
                return client.GetDatabase(_config.DatabaseName);
            }
        }
    }
}
