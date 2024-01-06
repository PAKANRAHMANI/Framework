using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.DataStore.ChangeTrackers;
using MongoDB.Driver;
using System.Timers;
using Framework.Core.Logging;
using Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;
using Timer = System.Timers.Timer;

namespace Framework.EventProcessor.DataStore.MongoDB
{
    public class MongoDbDataStore : IDataStoreObservable
    {
        private readonly MongoStoreConfig _mongoStoreConfig;
        private readonly ILogger _logger;
        private readonly Timer _timer;
        private IDataStoreChangeTrackerObserver _dataStoreChangeTracker;
        private readonly IMongoDbEventHandling _mongoDbEvent;

        public MongoDbDataStore(MongoStoreConfig mongoStoreConfig, IMongoDatabase database, ILogger logger)
        {
            _mongoStoreConfig = mongoStoreConfig;
            _logger = logger;
            _mongoDbEvent = MongoEventFactory.Create(mongoStoreConfig, database, logger);
            _timer = new Timer(_mongoStoreConfig.PullingInterval);
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();

                var events = _mongoDbEvent.GetEvents(_mongoStoreConfig.EventsCollectionName);

                if (events is not null && events.Any())
                {
                    _logger.Write($"{events.Count} Events found in Tables",LogLevel.Information);

                    this._dataStoreChangeTracker.ChangeDetected(events).Wait();

                    var collectionName = _mongoStoreConfig.IsUsedCursor
                        ? _mongoStoreConfig.CursorCollectionName
                        : _mongoStoreConfig.EventsCollectionName;

                    _mongoDbEvent.UpdateEvents(collectionName, events);

                    _logger.Write($"Cursor moved to position {events.Last().Id}",LogLevel.Information);
                }


                _timer.Start();
            }
            catch (Exception exception)
            {
               _logger.WriteException(exception);
            }
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
