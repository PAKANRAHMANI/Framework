using System.Timers;
using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.DataStore.ChangeTrackers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Timer = System.Timers.Timer;

namespace Framework.EventProcessor.DataStore.Sql;

internal sealed class SqlDataStore : IDataStoreObservable
{
    private IDataStoreChangeTrackerObserver _dataStoreChangeTracker;
    private readonly ILogger _logger;
    private readonly IUpdateCursorPosition _updateCursorPosition;
    private readonly SqlStoreConfig _sqlStoreConfig;
    private readonly Timer _timer;
    private static readonly object LockObject = new();
    public SqlDataStore(IOptions<SqlStoreConfig> sqlStoreConfig,
        ILogger logger,
        [FromKeyedServices(Constants.MoveSqlCursorPosition)] IUpdateCursorPosition updateCursorPosition)
    {
        _logger = logger;
        _updateCursorPosition = updateCursorPosition;
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
        lock (LockObject)
        {
            try
            {
                _timer.Stop();

                var cursorPosition =
                    SqlQueries.GetCursorPosition(_sqlStoreConfig.ConnectionString, _sqlStoreConfig.CursorTable);

                var events = SqlQueries.GetEventsFromPosition(_sqlStoreConfig.ConnectionString, cursorPosition,
                    _sqlStoreConfig.EventTable);

                if (events is not null && events.Any())
                {
                    _logger.Write($"{events.Count} Events found in Tables", LogLevel.Debug);

                    this._dataStoreChangeTracker.ChangeDetected(events, _updateCursorPosition).Wait();

                    _logger.Write($"Cursor moved to position {events.Last().Id}", LogLevel.Debug);
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
}