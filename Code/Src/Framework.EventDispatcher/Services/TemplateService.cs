using Framework.Core.Events;
using Framework.EventProcessor.DataStore;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Serialization;
using Framework.EventProcessor.Transformation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Framework.EventProcessor.Services;

public abstract class TemplateService : BackgroundService, IDataStoreChangeTrackerObserver
{
    private readonly IEventTypeResolver _eventTypeResolver;
    private readonly IEventFilter _eventFilter;
    private readonly IEventTransformerLookUp _transformerLookUp;
    private readonly ILogger<TemplateService> _logger;
    private readonly IDataStoreObservable _dataStore;

    protected TemplateService(
        IEventTypeResolver eventTypeResolver,
        IEventFilter eventFilter,
        IEventTransformerLookUp transformerLookUp,
        ILogger<TemplateService> logger,
        IDataStoreObservable dataStore
        )
    {
        _eventTypeResolver = eventTypeResolver;
        _eventFilter = eventFilter;
        _transformerLookUp = transformerLookUp;
        _logger = logger;
        _dataStore = dataStore;
        _dataStore.SetSubscriber(this);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Start();

                _logger.LogInformation("Event host Service running at: {time}", DateTimeOffset.Now);

                _dataStore.SubscribeForChanges();

                _logger.LogInformation("Subscribed to data store");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }
    }
    public void ChangeDetected(List<EventItem> events)
    {
        foreach (var eventItem in events)
        {
            var type = _eventTypeResolver.GetType(eventItem.EventType);

            if (type == null)
            {
                _logger.LogError($"Type of '{eventItem.EventType}' not found in event types");

                continue;
            }

            var eventToPublish = EventDeserializer.Deserialize(type, eventItem.Body);

            if (_eventFilter.ShouldPublish(eventToPublish))
            {
                eventToPublish = TransformEvent(eventToPublish, eventItem);

                Send(eventToPublish);

                _logger.LogInformation($"Event '{eventItem.EventType}-{eventItem.EventId}' Published on bus.");
            }
            else
            {
                _logger.LogInformation($"Publishing Event '{eventItem.EventType}-{eventItem.EventId}' Skipped because of filter");
            }
        }
    }
    protected abstract Task Start();
    protected abstract void Send(IEvent @event);
    private IEvent TransformEvent(IEvent @event, EventItem item)
    {
        var transformer = _transformerLookUp.LookUpTransformer(@event);

        if (transformer == null) return @event;

        @event = transformer.Transform(@event);

        _logger.LogInformation($"Event '{item.EventType}-{item.EventId}' Transformed");

        return @event;
    }
}