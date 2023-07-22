using Framework.Core.Events;
using Framework.EventProcessor.DataStore;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Transformation;
using Microsoft.Extensions.Logging;
using IEventPublisher = Framework.EventProcessor.Events.IEventPublisher;

namespace Framework.EventProcessor.Services
{
    public class EventPublisherService : TemplateService
    {
        private readonly IEventBus _eventBus;
        private readonly IEventPublisher _publisher;
        public EventPublisherService(
            IEventTypeResolver eventTypeResolver,
            IEventFilter eventFilter,
            IEventTransformerLookUp transformerLookUp,
            ILogger<EventPublisherService> logger,
            IDataStoreObservable dataStore,
            IEventPublisher publisher,
            IEventBus eventBus
            ) : base(eventTypeResolver, eventFilter, transformerLookUp, logger, dataStore)
        {
            _publisher = publisher;
            _eventBus = eventBus;
        }

        protected override async Task Start()
        {
            await _eventBus.Start();
        }

        protected override void Send(IEvent @event)
        {
            _publisher.Publish(@event);
        }
    }
}
