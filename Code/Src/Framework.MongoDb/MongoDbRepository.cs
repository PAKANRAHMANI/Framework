using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Framework.Domain;

namespace Framework.MongoDb
{
    public abstract class MongoDbRepository<TKey, T> : IRepository<TKey, T> where T :  AggregateRoot<TKey>
    {
        protected readonly IMongoDatabase Database;

        protected MongoDbRepository(IMongoDatabase database)
        {
            Database = database;
        }
        public abstract Task<TKey> GetNextId();

        public async Task Create(T aggregate)
        {
            await Database.GetCollection<T>(typeof(T).Name).InsertOneAsync(aggregate);
        }

        public async Task Update(T aggregate)
        {
            var filter = Builders<T>.Filter.Eq(s => s.Id, aggregate.Id);
            await Database.GetCollection<T>(typeof(T).Name).ReplaceOneAsync(filter, aggregate);
        }

        public async Task Remove(T aggregate)
        {
            var filter = Builders<T>.Filter.Eq(s => s.Id, aggregate.Id);
            var update = Builders<T>.Update.Set(a => a.IsDeleted, aggregate.IsDeleted);
            await Database.GetCollection<T>(typeof(T).Name).UpdateOneAsync(filter, update);
        }

        public async Task<T> Get(TKey key)
        {
            var filter = Builders<T>.Filter.Eq("_id", key);
            return await Database.GetCollection<T>(typeof(T).Name).Find(filter).FirstOrDefaultAsync();
        }
    }
}
