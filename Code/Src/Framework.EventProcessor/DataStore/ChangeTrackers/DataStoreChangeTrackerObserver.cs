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

internal sealed class DataStoreChangeTrackerObserver(
    IEventTypeResolver eventTypeResolver,
    IEventTransformerLookUp transformerLookUp,
    IEventFilter eventFilter,
    IServiceProvider services,
    Dictionary<int, Type> operations,
    ILogger logger,
    List<Receiver> observers) : EventObservable(observers), IDataStoreChangeTrackerObserver
{
    private readonly IFilter<IEvent> _operation = services.GetFirstOperation(operations);

    public async Task ChangeDetected(List<EventItem> events)
    {
        foreach (var eventItem in events)
        {
            try
            {
                var type = eventTypeResolver.GetType(eventItem.EventType);

                if (type == null)
                {
                    logger.Write($"Type of '{eventItem.EventType}' not found in event types", LogLevel.Information);

                    continue;
                }

                var eventToPublish = EventDeserializer.Deserialize(type, eventItem.Body, logger);

                if (eventToPublish is null)
                {
                    logger.Write("Deserialize event is null", LogLevel.Information);
                    continue;
                }
                if (eventFilter.ShouldPublish(eventToPublish))
                {
                    SendPrimaryEventToAllListeners(eventToPublish);

                    logger.Write("Send Primary Event To All Listeners", LogLevel.Information);

                    eventToPublish = TransformEvent.Transform(transformerLookUp, eventToPublish);

                    SendTransformedEventToAllListeners(eventToPublish);

                    logger.Write("Send Transformed Event To All Listeners", LogLevel.Information);

                    if (type != eventToPublish.GetType())
                        logger.Write($"Event '{eventItem.EventType}-{eventItem.EventId}' Transformed", LogLevel.Information);

                    await _operation.Apply(eventToPublish);

                    logger.Write($"Event '{eventItem.EventType}-{eventItem.EventId}' Published on bus.", LogLevel.Information);
                }
                else
                {
                    logger.Write($"Publishing Event '{eventItem.EventType}-{eventItem.EventId}' Skipped because of filter", LogLevel.Information);
                }
            }
            catch (Exception exception)
            {
                logger.WriteException(exception);
            }
        }
    }
}