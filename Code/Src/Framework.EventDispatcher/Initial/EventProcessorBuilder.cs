using Framework.EventProcessor.DataStore.Sql;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Events.MassTransit;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Transformation;
using Framework.EventProcessor.DataStore;
using Framework.EventProcessor.DataStore.MongoDB;
using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MassTransit;
using MassTransit.MultiBus;

namespace Framework.EventProcessor.Initial
{
    public class EventProcessorBuilder :
        IDataStoreBuilder,
        IEventLookup,
        IEventProcessorFilter,
        IEventTransformer,
        IEventSenderBuilder,
        IEnableSecondSenderBuilder,
        ISecondaryDeliveryEvent,
        IEventProcessor
    {
        private readonly IServiceCollection _services;
        private readonly ServiceConfig _serviceConfig;
        private Action<SecondaryProducerConfiguration> _secondaryProducerConfigurationAction;
        private Action<SecondaryMassTransitConfiguration> _secondaryMassTransitConfiguration;
        private EventProcessorBuilder(IServiceCollection serviceCollection)
        {
            _services = serviceCollection;
            _serviceConfig = new ServiceConfig();
        }
        public static IDataStoreBuilder Setup(IServiceCollection serviceCollection) => new EventProcessorBuilder(serviceCollection);

        public IEventLookup ReadFromSqlServer(Action<SqlStoreConfig> config)
        {
            _services.AddSingleton<IDataStoreObservable, SqlDataStore>();

            _services.Configure<SqlStoreConfig>(config);

            return this;
        }

        public IEventLookup ReadFromMongoDb(Action<MongoStoreConfig> config)
        {
            _services.AddSingleton<IDataStoreObservable, MongoDbDataStore>();

            _services.Configure<MongoStoreConfig>(config);

            return this;
        }

        public IEventTransformer WithFilter(IEventFilter filter)
        {
            _services.AddSingleton(filter);
            return this;
        }

        public IEventTransformer WithNoFilter()
        {
            return WithFilter(new NoFilter());
        }

        public IEventProcessorFilter UseEventsInAssemblies(params Assembly[] assemblies)
        {
            var eventTypeResolver = new EventTypeResolver();
            if (assemblies.Length > 0)
            {
                foreach (var assembly in assemblies)
                    eventTypeResolver.AddTypesFromAssembly(assembly);
            }
            _services.AddSingleton<IEventTypeResolver>(eventTypeResolver);
            return this;
        }

        public IEventSenderBuilder UseEventTransformersInAssemblies(params Assembly[] assemblies)
        {
            var transformerLookUp = new EventTransformerLookUp();
            if (assemblies.Length > 0)
            {
                foreach (var assembly in assemblies)
                    transformerLookUp.AddTypesFromAssembly(assembly);
            }
            _services.AddSingleton<IEventTransformerLookUp>(transformerLookUp);
            return this;
        }

        public IEventSenderBuilder WithNoEventTransformer()
        {
            return UseEventTransformersInAssemblies();
        }

        public IEnableSecondSenderBuilder PublishEventWithMassTransit(Action<MassTransitConfig> config)
        {
            _services.AddSingleton<IEventBus, MassTransitEventBusAdapter>();

            _services.Configure<MassTransitConfig>(config);

            _services.AddHostedService<EventPublisherService>();

            return this;
        }

        public IEnableSecondSenderBuilder ProduceMessageWithKafka(Action<ProducerConfiguration> config)
        {
            RegisterMessageProducer(config);

            _services.AddHostedService<MessageProducerService>();

            return this;
        }

        public ISecondaryDeliveryEvent EnableSendingMessageToSecondaryBroker()
        {
            _serviceConfig.EnableSecondarySending = true;
            return this;
        }

        public IEventProcessor DisableSendingMessageToSecondaryBroker()
        {
            _serviceConfig.EnableSecondarySending = false;
            return this;
        }

        public IEventProcessor SecondaryDeliveryWithKafka(Action<SecondaryProducerConfiguration> config)
        {
            _serviceConfig.SendWithKafka = true;

            _secondaryProducerConfigurationAction = config;

            return this;
        }

        public IEventProcessor SecondaryDeliveryWithMassTransit(Action<SecondaryMassTransitConfiguration> config)
        {
            _serviceConfig.SendWithMassTransit = true;

            _secondaryMassTransitConfiguration = config;

            return this;
        }

        public void Build()
        {
            _services.AddSingleton<IKafkaValidator, KafkaValidator>();

            RegisterSecondaryProducer(_secondaryProducerConfigurationAction);

            RegisterSecondaryBus(_secondaryMassTransitConfiguration);

            _services.AddSingleton(_serviceConfig);
        }
        private void RegisterMessageProducer(Action<ProducerConfiguration> config)
        {
            var producerConfig = new ProducerConfiguration();

            config.Invoke(producerConfig);

            _services.Configure<ProducerConfiguration>(config);

            var kafkaProducer = KafkaProducerFactory<object, object>.Create(producerConfig);

            var producer = MessageProducerFactory.Create(kafkaProducer, producerConfig);

            _services.AddSingleton(producer);
        }
        private void RegisterSecondaryBus(Action<SecondaryMassTransitConfiguration> config)
        {
            _services.Configure<SecondaryMassTransitConfiguration>(config);

            var secondaryMassTransitConfiguration = new SecondaryMassTransitConfiguration();

            config.Invoke(secondaryMassTransitConfiguration);

            _services.AddMassTransit<ISecondBus>(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(secondaryMassTransitConfiguration.RabbitMqConnectionString);
                });
            });
        }
        private void RegisterSecondaryProducer(Action<SecondaryProducerConfiguration> config)
        {
            var producerConfig = new SecondaryProducerConfiguration();

            config.Invoke(producerConfig);

            _services.Configure<SecondaryProducerConfiguration>(config);
        }
    }
}
