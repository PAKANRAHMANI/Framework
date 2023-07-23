using Framework.Core.Events;
using Framework.EventProcessor.DataStore;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Transformation;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            IEventBus eventBus,
            ServiceConfig serviceConfig,
            IOptions<SecondaryProducerConfiguration> secondaryProducerConfiguration,
            Bind<ISecondBus, IPublishEndpoint> secondaryPublishEndpoint
            ) : base(eventTypeResolver, eventFilter, transformerLookUp, logger, dataStore,
            serviceConfig, secondaryProducerConfiguration, secondaryPublishEndpoint)
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
