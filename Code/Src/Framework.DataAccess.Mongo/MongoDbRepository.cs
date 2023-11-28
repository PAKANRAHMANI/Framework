using Framework.Core.Exceptions;
using Framework.Domain;
using Framework.Domain.Events;
using Humanizer;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.DataAccess.Mongo
{
    public abstract class MongoDbRepository<TKey, T> : IRepository<TKey, T> where T : AggregateRoot<TKey>
    {
        protected readonly IMongoDatabase Database;
        protected readonly IAggregateRootConfigurator Configurator;
        private readonly IClientSessionHandle _session;
        private readonly MongoDbConfig _config;

        protected MongoDbRepository(IMongoDatabase database, IMongoContext mongoContext, IAggregateRootConfigurator configurator, MongoDbConfig config)
        {
            Database = database;
            Configurator = configurator;
            _session = mongoContext.GetSession();
            _config = config;
        }
        public abstract Task<TKey> GetNextId();

        public async Task Create(T aggregate)
        {
            aggregate.MarkAsRowVersion();

            if (_config.UseTransaction)
            {
                await Database.GetCollection<T>(typeof(T).Name.Pluralize()).InsertOneAsync(_session, aggregate);

                await PersistEvents(aggregate);
            }
            else
                await Database.GetCollection<T>(typeof(T).Name.Pluralize()).InsertOneAsync(aggregate);
        }

        public async Task Update(T aggregate)
        {
            var currentRowVersion = aggregate.RowVersion;

            aggregate.MarkAsRowVersion();

            var filter = Builders<T>.Filter.Eq(s => s.Id, aggregate.Id) &
                         Builders<T>.Filter.Eq(r => r.RowVersion, currentRowVersion);

            ReplaceOneResult result;

            if (_config.UseTransaction)
            {
                result = await Database.GetCollection<T>(typeof(T).Name.Pluralize()).ReplaceOneAsync(_session, filter, aggregate);

                await PersistEvents(aggregate);
            }

            else
                result = await Database.GetCollection<T>(typeof(T).Name.Pluralize()).ReplaceOneAsync(filter, aggregate);

            if (!result.IsAcknowledged)
            {
                Throw.Infrastructure.MongoDbNotAcknowledged();
            }

            if (result.ModifiedCount == 0 && await Database.GetCollection<T>(typeof(T).Name.Pluralize()).CountDocumentsAsync(r => r.Id.Equals(aggregate.Id)) == 1)
            {
                Throw.Infrastructure.MongoDbConcurrencyReplaceOneFail();
            }
        }

        public async Task Remove(T aggregate)
        {
            var filter = Builders<T>.Filter.Eq(s => s.Id, aggregate.Id) &
                         Builders<T>.Filter.Eq(r => r.RowVersion, aggregate.RowVersion);

            var update = Builders<T>.Update.Set(a => a.IsDeleted, aggregate.IsDeleted);

            UpdateResult result;

            if (_config.UseTransaction)
            {
                result = await Database.GetCollection<T>(typeof(T).Name.Pluralize()).UpdateOneAsync(_session, filter, update);

                await PersistEvents(aggregate);
            }

            else
                result = await Database.GetCollection<T>(typeof(T).Name.Pluralize()).UpdateOneAsync(filter, update);

            if (!result.IsAcknowledged)
            {
                Throw.Infrastructure.MongoDbNotAcknowledged();
            }
            if (result.ModifiedCount == 0 && await Database.GetCollection<T>(typeof(T).Name.Pluralize()).CountDocumentsAsync(r => r.Id.Equals(aggregate.Id)) == 1)
            {
                Throw.Infrastructure.MongoDbConcurrencyReplaceOneFail();
            }
        }

        public async Task<T> Get(TKey key)
        {
            var filter = Builders<T>.Filter.Eq("_id", key);

            var aggregate = await Database.GetCollection<T>(typeof(T).Name.Pluralize()).Find(filter).FirstOrDefaultAsync();

            return Configurator.Config(aggregate);
        }
        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            var aggregate = await Database.GetCollection<T>(typeof(T).Name.Pluralize()).Find(predicate).FirstOrDefaultAsync();

            return Configurator.Config(aggregate);
        }

        protected async Task PersistEvents(T aggregate)
        {
            var domainEvent = GetDomainEvents(aggregate);
            var distributedEvent = GetDistributedEvents(aggregate);

            var sequenceHelper = new MongoSequenceHelper(Database);

            if (domainEvent.Any())
            {
                foreach (var @event in domainEvent)
                {
                    @event.Id = sequenceHelper.GetNextId(_config.SequenceCollectionName);
                }
                await Database.GetCollection<DomainEventStructure>("DomainEvents").InsertManyAsync(domainEvent);
                aggregate.ClearEvents();
            }


            if (distributedEvent.Any())
            {
                foreach (var @event in distributedEvent)
                {
                    @event.Id = sequenceHelper.GetNextId(_config.SequenceCollectionName);
                }
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
