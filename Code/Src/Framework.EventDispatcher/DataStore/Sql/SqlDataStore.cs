using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Timer = System.Timers.Timer;

namespace Framework.EventProcessor.DataStore.Sql;

public class SqlDataStore : IDataStoreObservable
{
    private IDataStoreChangeTrackerObserver _dataStoreChangeTracker;
    private readonly ILogger<SqlDataStore> _logger;
    private readonly SqlStoreConfig _sqlStoreConfig;
    private readonly Timer _timer;
    public SqlDataStore(IOptions<SqlStoreConfig> sqlStoreConfig, ILogger<SqlDataStore> logger)
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

        var cursorPosition = SqlQueries.GetCursorPosition(_sqlStoreConfig.ConnectionString, _sqlStoreConfig.CursorTable);

        var events = SqlQueries.GetEventsFromPosition(_sqlStoreConfig.ConnectionString, cursorPosition, _sqlStoreConfig.EventTable);

        if (events.Any())
        {
            _logger.LogInformation($"{events.Count} Events found in Tables");

            this._dataStoreChangeTracker.ChangeDetected(events).Wait();

            SqlQueries.MoveCursorPosition(_sqlStoreConfig.ConnectionString, events.Last().Id, _sqlStoreConfig.CursorTable);

            _logger.LogInformation($"Cursor moved to position {events.Last().Id}");
        }

        _timer.Start();
    }
}