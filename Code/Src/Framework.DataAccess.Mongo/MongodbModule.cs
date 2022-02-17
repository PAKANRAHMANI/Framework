﻿using System;
using Framework.Application.Contracts;
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
            dependencyRegister.RegisterSingleton<IMongoDatabase>(CreateMongoDb);

            if (_config.IsDecorateTransactionForCommands)
            {
                dependencyRegister.RegisterScoped<IUnitOfWork, MongoUnitOfWork>();

                dependencyRegister.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionalCommandHandlerDecorator<>));
            }
        }
        private IMongoDatabase CreateMongoDb()
        {
           var client = new MongoClient(_config.ConnectionString);

            return client.GetDatabase(_config.DatabaseName);
        }
    }
}
