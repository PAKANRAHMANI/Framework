using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Domain;
using Framework.Domain.Events;
using MongoDB.Driver;

namespace Framework.DataAccess.Mongo
{
    public abstract class MongoDbRepository<TKey, T> : IRepository<TKey, T> where T : AggregateRoot<TKey>
    {
        protected readonly IMongoDatabase Database;
        private readonly IClientSessionHandle _session;
        private readonly MongoDbConfig _config;

        protected MongoDbRepository(IMongoDatabase database, IMongoContext mongoContext, MongoDbConfig config)
        {
            Database = database;
            _session = mongoContext.GetSession();
            _config = config;
        }
        public abstract Task<TKey> GetNextId();

        public async Task Create(T aggregate)
        {
            if (_config.IsDecorateTransactionForCommands)
            {
                await Database.GetCollection<T>(typeof(T).Name).InsertOneAsync(_session, aggregate);

                await Database.GetCollection<DomainEventStructure>("DomainEvents").InsertManyAsync(GetDomainEvents(aggregate));
            }

            else
                await Database.GetCollection<T>(typeof(T).Name).InsertOneAsync(aggregate);
        }

        public async Task Update(T aggregate)
        {
            var filter = Builders<T>.Filter.Eq(s => s.Id, aggregate.Id);

            if (_config.IsDecorateTransactionForCommands)
            {
                await Database.GetCollection<T>(typeof(T).Name).ReplaceOneAsync(_session, filter, aggregate);

                await Database.GetCollection<DomainEventStructure>("DomainEvents").InsertManyAsync(GetDomainEvents(aggregate));
            }

            else
                await Database.GetCollection<T>(typeof(T).Name).ReplaceOneAsync(filter, aggregate);
        }

        public async Task Remove(T aggregate)
        {
            var filter = Builders<T>.Filter.Eq(s => s.Id, aggregate.Id);

            var update = Builders<T>.Update.Set(a => a.IsDeleted, aggregate.IsDeleted);

            if (_config.IsDecorateTransactionForCommands)
            {
                await Database.GetCollection<T>(typeof(T).Name).UpdateOneAsync(_session, filter, update);

                await Database.GetCollection<DomainEventStructure>("DomainEvents").InsertManyAsync(GetDomainEvents(aggregate));
            }

            else
                await Database.GetCollection<T>(typeof(T).Name).UpdateOneAsync(filter, update);
        }

        public async Task<T> Get(TKey key)
        {
            var filter = Builders<T>.Filter.Eq("_id", key);

            return await Database.GetCollection<T>(typeof(T).Name).Find(filter).FirstOrDefaultAsync();
        }

        private List<DomainEventStructure> GetDomainEvents(T aggregate)
        {
            var domainEvents = aggregate.GetEvents();

            return DomainEventStructureFactory.Create(domainEvents);
        }
    }
}
