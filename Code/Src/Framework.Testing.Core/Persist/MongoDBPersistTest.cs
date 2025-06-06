﻿using MongoDB.Driver;
using System;
using System.Linq;
using Humanizer;
using Framework.Testing.Core.Persist.MongoDb;
using System.Reflection;
using Framework.DataAccess.Mongo;

namespace Framework.Testing.Core.Persist
{
    public abstract class MongoDbPersistTest<T> : IDisposable
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _mongoDb;
        private readonly string _collectionName;
        private readonly MongoDbPersistConfiguration _config;
        protected IClientSessionHandle Session { get; }
        protected IMongoCollection<T> DbCollection { get; }


        protected MongoDbPersistTest(Action<MongoDbPersistConfiguration> configuration, Assembly mappingAssembly)
        {
            ApplyMapping(mappingAssembly);

            this._config = new MongoDbPersistConfiguration();

            configuration?.Invoke(this._config);

            this._collectionName = this._config.IsPluralCollectionName ? typeof(T).Name.Pluralize() : typeof(T).Name;

            this._client = new MongoClient(this._config.ConnectionString);

            this._mongoDb = this._client.GetDatabase(this._config.DbName);

            this.DbCollection = this._mongoDb.GetCollection<T>(this._collectionName);

            if (this._config.IsUsingTransaction)
            {
                this.Session = this._client.StartSession();

                this.Session.StartTransaction();
            }
        }

        private static void ApplyMapping(Assembly mappingAssembly)
        {
            var mapperTypes = mappingAssembly.GetTypes().Where(a => typeof(IBsonMapping).IsAssignableFrom(a)).ToList();
            foreach (var instanceOfMapper in mapperTypes.Select(mapper => (IBsonMapping)Activator.CreateInstance(mapper)))
            {
                instanceOfMapper?.Register();
            }
        }

        protected abstract object DocumentId { get; set; }
        public void Dispose()
        {
            var deleteFilter = Builders<T>.Filter.Eq("_id", DocumentId);

            this.DbCollection.DeleteOne(deleteFilter);

            if (this._config.IsUsingTransaction)
                this.Session.Dispose();
        }
    }
}
