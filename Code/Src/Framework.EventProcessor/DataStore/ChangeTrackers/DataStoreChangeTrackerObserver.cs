using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Extensions;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Initial;
using Framework.EventProcessor.Serialization;
using Framework.EventProcessor.Transformation;
using Microsoft.Extensions.Logging;

namespace Framework.EventProcessor.DataStore.ChangeTrackers;

public class DataStoreChangeTrackerObserver : EventObservable, IDataStoreChangeTrackerObserver
{
    private readonly IEventTypeResolver _eventTypeResolver;
    private readonly IEventTransformerLookUp _transformerLookUp;
    private readonly IEventFilter _eventFilter;
    private readonly IFilter<IEvent> _operation;
    private readonly ILogger<DataStoreChangeTrackerObserver> _logger;

    internal DataStoreChangeTrackerObserver(
        IEventTypeResolver eventTypeResolver,
        IEventTransformerLookUp transformerLookUp,
        IEventFilter eventFilter,
        IServiceProvider services,
        Dictionary<int, Type> operations,
        ILogger<DataStoreChangeTrackerObserver> logger,
        IEnumerable<Receiver> observers
    ) : base(observers.ToList())
    {
        _eventTypeResolver = eventTypeResolver;
        _transformerLookUp = transformerLookUp;
        _eventFilter = eventFilter;
        _logger = logger;
        _operation = services.GetFirstOperation(operations);
    }
    public async Task ChangeDetected(List<EventItem> events)
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
                SendPrimaryEventToAllListeners(eventToPublish);

                eventToPublish = TransformEvent.Transform(_transformerLookUp, eventToPublish);

                SendTransformedEventToAllListeners(eventToPublish);

                _logger.LogInformation($"Event '{eventItem.EventType}-{eventItem.EventId}' Transformed");

                await _operation.Apply(eventToPublish);

                _logger.LogInformation($"Event '{eventItem.EventType}-{eventItem.EventId}' Published on bus.");
            }
            else
            {
                _logger.LogInformation($"Publishing Event '{eventItem.EventType}-{eventItem.EventId}' Skipped because of filter");
            }
        }
    }
}