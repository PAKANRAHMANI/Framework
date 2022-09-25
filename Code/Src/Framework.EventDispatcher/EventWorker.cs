using Framework.Core.Events;
using Framework.EventProcessor.DataStore;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Serialization;
using Framework.EventProcessor.Transformation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Framework.EventProcessor
{
    public class EventWorker : BackgroundService, IDataStoreChangeTrackerObserver
    {
        private readonly ILogger<EventWorker> _logger;
        private readonly IDataStoreObservable _dataStore;
        private readonly IEventBus _eventBus;
        private readonly IEventTypeResolver _eventTypeResolver;
        private readonly IEventFilter _eventFilter;
        private readonly IEventTransformerLookUp _transformerLookUp;

        public EventWorker(
            ILogger<EventWorker> logger,
            IDataStoreObservable dataStore,
            IEventBus eventBus,
            IEventTypeResolver eventTypeResolver,
            IEventFilter eventFilter,
            IEventTransformerLookUp transformerLookUp
            )
        {
            _logger = logger;
            _dataStore = dataStore;
            _eventBus = eventBus;
            _eventTypeResolver = eventTypeResolver;
            _eventFilter = eventFilter;
            _transformerLookUp = transformerLookUp;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _eventBus.Start();

                _logger.LogInformation("Event hOst Service running at: {time}", DateTimeOffset.Now);

                _dataStore.SubscribeForChanges();
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

                    _eventBus.Publish(eventToPublish);

                    _logger.LogInformation($"Event '{eventItem.EventType}-{eventItem.EventId}' Published on bus.");
                }
                else
                {
                    _logger.LogInformation($"Publishing Event '{eventItem.EventType}-{eventItem.EventId}' Skipped because of filter");
                }
            }
        }

        private IEvent TransformEvent(IEvent @event, EventItem item)
        {
            var transformer = _transformerLookUp.LookUpTransformer(@event);

            if (transformer == null) return @event;

            @event = transformer.Transform(@event);

            _logger.LogInformation($"Event '{item.EventType}-{item.EventId}' Transformed");

            return @event;
        }
    }
}
