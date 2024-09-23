using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.DataStore.ChangeTrackers;
using MongoDB.Driver;
using System.Timers;
using Framework.Core.Logging;
using Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;
using Microsoft.Extensions.DependencyInjection;
using Timer = System.Timers.Timer;

namespace Framework.EventProcessor.DataStore.MongoDB
{
    internal sealed class MongoDbDataStore : IDataStoreObservable
    {
        private readonly MongoStoreConfig _mongoStoreConfig;
        private readonly IUpdateCursorPosition _updateCursorPosition;
        private readonly ILogger _logger;
        private readonly Timer _timer;
        private IDataStoreChangeTrackerObserver _dataStoreChangeTracker;
        private readonly IMongoDbEventHandling _mongoDbEvent;
        private static readonly object LockObject = new();
        public MongoDbDataStore(MongoStoreConfig mongoStoreConfig, IMongoDatabase database,
            ILogger logger,
            [FromKeyedServices(Constants.MoveMongoCursorPosition)] IUpdateCursorPosition updateCursorPosition)
        {
            _mongoStoreConfig = mongoStoreConfig;
            _logger = logger;
            _updateCursorPosition = updateCursorPosition;
            _mongoDbEvent = MongoEventFactory.Create(mongoStoreConfig, database, logger);
            _timer = new Timer(_mongoStoreConfig.PullingInterval);
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            lock (LockObject)
            {
                try
                {

                    var events = _mongoDbEvent.GetEvents(_mongoStoreConfig.EventsCollectionName);

                    if (events is not null && events.Any())
                    {
                        _logger.Write($"{events.Count} Events found in Tables", LogLevel.Debug);

                        this._dataStoreChangeTracker.ChangeDetected(events, _updateCursorPosition).Wait();

                    }

                }
                catch (Exception exception)
                {
                    _logger.WriteException(exception);
                }
                finally
                {
                    _timer.Start();
                }
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
