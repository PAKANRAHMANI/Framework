using Framework.EventProcessor.DataStore.Sql;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Timers;
using MongoDB.Driver;
using Timer = System.Timers.Timer;
namespace Framework.EventProcessor.DataStore.MongoDB
{
    public class MongoDBDataStore : IDataStoreObservable
    {
        private readonly MongoStoreConfig _mongoStoreConfig;
        private readonly ILogger<EventWorker> _logger;
        private readonly Timer _timer;
        private IDataStoreChangeTrackerObserver _dataStoreChangeTracker;

        public MongoDBDataStore(IOptions<MongoStoreConfig> mongoStoreConfig, ILogger<EventWorker> logger)
        {
            _mongoStoreConfig = mongoStoreConfig.Value;
            _logger = logger;
            _timer = new Timer(_mongoStoreConfig.PullingInterval);
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            var mongoClient = new MongoClient(_mongoStoreConfig.ConnectionString);

            var database = mongoClient.GetDatabase(_mongoStoreConfig.DatabaseName);

            var cursorPosition = database.GetCursorPosition(_mongoStoreConfig.CursorDocument);

            var events = database.GetEventsFromPosition(cursorPosition, _mongoStoreConfig.EventsDocument);

            if (events.Any())
            {
                _logger.LogInformation($"{events.Count} Events found in Tables");

                this._dataStoreChangeTracker.ChangeDetected(events);

                database.MoveCursorPosition(events.Last().Id, _mongoStoreConfig.CursorDocument);

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
