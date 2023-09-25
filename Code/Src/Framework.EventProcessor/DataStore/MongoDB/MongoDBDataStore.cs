using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.DataStore.ChangeTrackers;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Timers;
using Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;
using Timer = System.Timers.Timer;

namespace Framework.EventProcessor.DataStore.MongoDB
{
    public class MongoDbDataStore : IDataStoreObservable
    {
        private readonly MongoStoreConfig _mongoStoreConfig;
        private readonly ILogger<MongoDbDataStore> _logger;
        private readonly Timer _timer;
        private IDataStoreChangeTrackerObserver _dataStoreChangeTracker;
        private readonly IMongoDbEventHandling _mongoDbEvent;

        public MongoDbDataStore(MongoStoreConfig mongoStoreConfig, IMongoDatabase database, ILogger<MongoDbDataStore> logger)
        {
            _mongoStoreConfig = mongoStoreConfig;
            _logger = logger;
            _mongoDbEvent = MongoEventFactory.Create(mongoStoreConfig, database);
            _timer = new Timer(_mongoStoreConfig.PullingInterval);
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            var events = _mongoDbEvent.GetEvents(_mongoStoreConfig.EventsCollectionName);

            if (events.Any())
            {
                _logger.LogInformation($"{events.Count} Events found in Tables");

                this._dataStoreChangeTracker.ChangeDetected(events).Wait();

                var collectionName = _mongoStoreConfig.IsUsedCursor
                    ? _mongoStoreConfig.CursorCollectionName
                    : _mongoStoreConfig.EventsCollectionName;

                _mongoDbEvent.UpdateEvents(collectionName, events);

                _logger.LogInformation($"Cursor moved to position {events.Last().Id}");
            }


            _timer.Start();
        }
        public void SetSubscriber(IDataStoreChangeTrackerObserver changeTracker)
        {
            this._dataStoreChangeTracker = changeTracker;
        }

        public ISubscription SubscribeForChanges()
        {
            _timer.Start();
            return new EndSubscription(_timer.Stop);
        }
    }
}
