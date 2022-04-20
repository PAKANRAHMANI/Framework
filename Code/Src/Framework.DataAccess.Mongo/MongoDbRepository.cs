using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.Events;
using Framework.Domain;
using Framework.Domain.Events;
using Humanizer;
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
                await Database.GetCollection<T>(typeof(T).Name.Pluralize()).InsertOneAsync(_session, aggregate);

                await PersistEvents(aggregate);

            }

            else
                await Database.GetCollection<T>(typeof(T).Name.Pluralize()).InsertOneAsync(aggregate);
        }

        public async Task Update(T aggregate)
        {
            var filter = Builders<T>.Filter.Eq(s => s.Id, aggregate.Id);

            if (_config.IsDecorateTransactionForCommands)
            {
                await Database.GetCollection<T>(typeof(T).Name.Pluralize()).ReplaceOneAsync(_session, filter, aggregate);

                await PersistEvents(aggregate);
            }

            else
                await Database.GetCollection<T>(typeof(T).Name.Pluralize()).ReplaceOneAsync(filter, aggregate);
        }

        public async Task Remove(T aggregate)
        {
            var filter = Builders<T>.Filter.Eq(s => s.Id, aggregate.Id);

            var update = Builders<T>.Update.Set(a => a.IsDeleted, aggregate.IsDeleted);

            if (_config.IsDecorateTransactionForCommands)
            {
                await Database.GetCollection<T>(typeof(T).Name.Pluralize()).UpdateOneAsync(_session, filter, update);

                await PersistEvents(aggregate);
            }

            else
                await Database.GetCollection<T>(typeof(T).Name.Pluralize()).UpdateOneAsync(filter, update);
        }

        public async Task<T> Get(TKey key)
        {
            var filter = Builders<T>.Filter.Eq("_id", key);

            return await Database.GetCollection<T>(typeof(T).Name.Pluralize()).Find(filter).FirstOrDefaultAsync();
        }

        protected async Task PersistEvents(T aggregate)
        {
            var domainEvent = GetDomainEvents(aggregate);
            var distributedEvent = GetDistributedEvents(aggregate);

            if (domainEvent.Any())
            {
                await Database.GetCollection<DomainEventStructure>("DomainEvents").InsertManyAsync(domainEvent);
                aggregate.ClearEvents();
            }


            if (distributedEvent.Any())
            {
                await Database.GetCollection<DistributedEventStructure>("DistributedEvents").InsertManyAsync(distributedEvent);
                aggregate.ClearEvents();
            }
                
        }

        private List<DomainEventStructure> GetDomainEvents(T aggregate)
        {
            var domainEvents = aggregate.GetEvents();

            return DomainEventStructureFactory.Create(domainEvents);
        }

        private List<DistributedEventStructure> GetDistributedEvents(T aggregate)
        {
            var distributedEvents = aggregate.GetDistributedEvents();

            return DistributedEventStructureFactory.Create(distributedEvents);
        }
    }
}
