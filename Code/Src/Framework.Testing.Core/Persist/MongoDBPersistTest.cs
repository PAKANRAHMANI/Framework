using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

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


        protected MongoDbPersistTest(Action<MongoDbPersistConfiguration> configuration)
        {
            this._config = new MongoDbPersistConfiguration();

            configuration?.Invoke(this._config);

            this._collectionName = this._config.IsPluralCollectionName ? $"{typeof(T).Name}s" : $"{typeof(T).Name}";

            this._client = new MongoClient(this._config.ConnectionString);

            this._mongoDb = this._client.GetDatabase(this._config.DbName);

            this.DbCollection = this._mongoDb.GetCollection<T>(this._collectionName);

            if (this._config.IsUsingTransaction)
            {
                this.Session = this._client.StartSession();

                this.Session.StartTransaction();
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
