using Framework.Core.Logging;
using Framework.EventProcessor.DataStore;
using Framework.EventProcessor.DataStore.ChangeTrackers;
using Microsoft.Extensions.Hosting;


namespace Framework.EventProcessor.Services;

public class EventWorker : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IDataStoreObservable _dataStore;
    private ISubscription _subscription;

    public EventWorker(
        ILogger logger,
        IDataStoreObservable dataStore,
        IDataStoreChangeTrackerObserver changeTrackerObserver
    )
    {
        _logger = logger;
        _dataStore = dataStore;
        _dataStore.SetSubscriber(changeTrackerObserver);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.Write($"Event host Service running at: {DateTimeOffset.Now}",LogLevel.Information); 

                _subscription = _dataStore.SubscribeForChanges();

                _logger.Write($"Subscribed to data store", LogLevel.Information);
            }
            catch (Exception exception)
            {
                _logger.WriteException(exception);

                _subscription.UnSubscribe();
            }
            finally
            {
                _subscription = _dataStore.SubscribeForChanges();
            }
        }
        else
        {
            _subscription.UnSubscribe();
            await Task.CompletedTask;
        }

    }
    public override void Dispose()
    {
        _subscription.UnSubscribe();
    }
}