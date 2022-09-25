using System.Timers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Timer = System.Timers.Timer;

namespace Framework.EventProcessor.DataStore.Sql;

public class SqlDataStore : IDataStoreObservable
{
    private IDataStoreChangeTrackerObserver _dataStoreChangeTracker;
    private readonly ILogger<EventWorker> _logger;
    private readonly SqlStoreConfig _sqlStoreConfig;
    private readonly Timer _timer;
    public SqlDataStore(IOptions<SqlStoreConfig> sqlStoreConfig, ILogger<EventWorker> logger)
    {
        _logger = logger;
        _sqlStoreConfig = sqlStoreConfig.Value;
        _timer = new Timer(_sqlStoreConfig.PullingInterval);
        _timer.Elapsed += TimerOnElapsed;
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

    private void TimerOnElapsed(object sender, ElapsedEventArgs e)
    {
        _timer.Stop();

        using (var sqlConnection = new SqlConnection(_sqlStoreConfig.ConnectionString))
        {
            var cursorPosition = sqlConnection.GetCursorPosition(_sqlStoreConfig.CursorTable);

            var events = sqlConnection.GetEventsFromPosition(cursorPosition, _sqlStoreConfig.EventTable);

            if (events.Any())
            {
                _logger.LogInformation($"{events.Count} Events found in Tables");

                this._dataStoreChangeTracker.ChangeDetected(events);

                sqlConnection.MoveCursorPosition(events.Last().Id, _sqlStoreConfig.CursorTable);

                _logger.LogInformation($"Cursor moved to position {events.Last().Id}");
            }
        }

        _timer.Start();
    }
}