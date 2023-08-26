using Framework.Core.Events;
using Framework.EventProcessor.DataStore;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Transformation;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Framework.EventProcessor.Services
{
    public class MessageProducerService : TemplateService
    {
        private readonly ILogger<MessageProducerService> _logger;
        private readonly MessageProducer _producer;
        private readonly ProducerConfiguration _producerConfiguration;
        private readonly IKafkaValidator _kafkaValidator;

        public MessageProducerService(
            IEventTypeResolver eventTypeResolver,
            IEventFilter eventFilter,
            IEventTransformerLookUp transformerLookUp,
            ILogger<MessageProducerService> logger,
            IDataStoreObservable dataStore,
            MessageProducer producer,
            ProducerConfiguration producerConfiguration,
            IKafkaValidator kafkaValidator,
            ServiceConfig serviceConfig,
            IOptions<SecondaryProducerConfiguration> secondaryProducerConfiguration,
            Bind<ISecondBus, IPublishEndpoint> secondaryPublishEndpoint
            ) : base(eventTypeResolver, eventFilter, transformerLookUp, logger, dataStore,
            serviceConfig, secondaryProducerConfiguration, secondaryPublishEndpoint)
        {
            _logger = logger;
            _producer = producer;
            _producerConfiguration = producerConfiguration;
            _kafkaValidator = kafkaValidator;
        }


        protected override Task Start()
        {
            if (!_kafkaValidator.TopicIsExist(_producerConfiguration.TopicName))
                _logger.LogWarning("Topic Is Not Exist");

            return Task.CompletedTask;
        }

        protected override async Task Send(IEvent @event)
        {
           await _producer.ProduceAsync(_producerConfiguration.TopicKey, @event);
        }
    }
}
