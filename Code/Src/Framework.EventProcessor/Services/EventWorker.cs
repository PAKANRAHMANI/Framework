using Framework.EventProcessor.DataStore;
using Framework.EventProcessor.DataStore.ChangeTrackers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Framework.EventProcessor.Services;

public class EventWorker : BackgroundService
{
    private readonly ILogger<EventWorker> _logger;
    private readonly IDataStoreObservable _dataStore;
    private ISubscription _subscription;

    public EventWorker(
        ILogger<EventWorker> logger,
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
                _logger.LogInformation("Event host Service running at: {time}", DateTimeOffset.Now);

                _subscription = _dataStore.SubscribeForChanges();

                _logger.LogInformation("Subscribed to data store");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                _subscription.UnSubscribe();
            }
            finally
            {
                await Task.CompletedTask;
            }
        }
        else
        {
            await Task.CompletedTask;
        }

    }
    public override void Dispose()
    {
        _subscription.UnSubscribe();
    }
}