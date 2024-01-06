using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.Core.Logging;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Extensions;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Initial;
using Framework.EventProcessor.Serialization;
using Framework.EventProcessor.Transformation;

namespace Framework.EventProcessor.DataStore.ChangeTrackers;

public class DataStoreChangeTrackerObserver : EventObservable, IDataStoreChangeTrackerObserver
{
    private readonly IEventTypeResolver _eventTypeResolver;
    private readonly IEventTransformerLookUp _transformerLookUp;
    private readonly IEventFilter _eventFilter;
    private readonly IFilter<IEvent> _operation;
    private readonly ILogger _logger;

    public DataStoreChangeTrackerObserver(
        IEventTypeResolver eventTypeResolver,
        IEventTransformerLookUp transformerLookUp,
        IEventFilter eventFilter,
        IServiceProvider services,
        Dictionary<int, Type> operations,
        ILogger logger,
        List<Receiver> observers
    ) : base(observers)
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
            try
            {
                var type = _eventTypeResolver.GetType(eventItem.EventType);

                if (type == null)
                {
                    _logger.Write($"Type of '{eventItem.EventType}' not found in event types", LogLevel.Information);

                    continue;
                }

                var eventToPublish = EventDeserializer.Deserialize(type, eventItem.Body, _logger);

                if (eventToPublish is null)
                {
                    _logger.Write("Deserialize event is null", LogLevel.Information);
                    continue;
                }
                if (_eventFilter.ShouldPublish(eventToPublish))
                {
                    SendPrimaryEventToAllListeners(eventToPublish);

                    _logger.Write("Send Primary Event To All Listeners", LogLevel.Information);

                    eventToPublish = TransformEvent.Transform(_transformerLookUp, eventToPublish);

                    SendTransformedEventToAllListeners(eventToPublish);

                    _logger.Write("Send Transformed Event To All Listeners", LogLevel.Information);

                    if (type != eventToPublish.GetType())
                        _logger.Write($"Event '{eventItem.EventType}-{eventItem.EventId}' Transformed", LogLevel.Information);

                    await _operation.Apply(eventToPublish);

                    _logger.Write($"Event '{eventItem.EventType}-{eventItem.EventId}' Published on bus.", LogLevel.Information);
                }
                else
                {
                    _logger.Write($"Publishing Event '{eventItem.EventType}-{eventItem.EventId}' Skipped because of filter", LogLevel.Information);
                }
            }
            catch (Exception exception)
            {
                _logger.WriteException(exception);
            }
        }
    }
}