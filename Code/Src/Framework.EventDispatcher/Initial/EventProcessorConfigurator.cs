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
    //TODO:convert to step builder
    public class EventProcessorConfigurator
    {
        private readonly IServiceCollection _services;
        private readonly ServiceConfig _serviceConfig;
        private Action<SecondaryProducerConfiguration> _secondaryProducerConfigurationAction;
        private Action<SecondaryMassTransitConfiguration> _secondaryMassTransitConfiguration;
        public EventProcessorConfigurator(IServiceCollection serviceCollection)
        {
            _services = serviceCollection;
            _serviceConfig = new ServiceConfig();
        }

        public EventProcessorConfigurator ReadFromSqlServer(Action<SqlStoreConfig> config)
        {
            _services.AddSingleton<IDataStoreObservable, SqlDataStore>();

            _services.Configure<SqlStoreConfig>(config);

            return this;
        }
        public EventProcessorConfigurator ReadFromMongoDb(Action<MongoStoreConfig> config)
        {
            _services.AddSingleton<IDataStoreObservable, MongoDbDataStore>();

            _services.Configure<MongoStoreConfig>(config);

            return this;
        }
        public EventProcessorConfigurator PublishWithMassTransit(Action<MassTransitConfig> config)
        {
            _services.AddSingleton<IEventBus, MassTransitEventBusAdapter>();

            _services.Configure<MassTransitConfig>(config);

            _services.AddHostedService<EventPublisherService>();

            return this;
        }
        public EventProcessorConfigurator ProduceMessageWithKafka(Action<ProducerConfiguration> config)
        {
            RegisterMessageProducer(config);

            _services.AddHostedService<MessageProducerService>();

            return this;
        }
        public EventProcessorConfigurator SecondaryDeliveryWithKafka(Action<SecondaryProducerConfiguration> config)
        {
            _serviceConfig.SendWithKafka = true;

            _secondaryProducerConfigurationAction = config;

            return this;
        }
        public EventProcessorConfigurator SecondaryDeliveryWithMassTransit(Action<SecondaryMassTransitConfiguration> config)
        {
            _serviceConfig.SendWithMassTransit = true;

            _secondaryMassTransitConfiguration = config;

            return this;
        }
        public EventProcessorConfigurator EnableSendingMessageToSecondaryBroker()
        {
            _serviceConfig.EnableSecondarySending = true;
            return this;
        }

        public EventProcessorConfigurator WithFilter(IEventFilter filter)
        {
            _services.AddSingleton(filter);
            return this;
        }

        public EventProcessorConfigurator WithNoFilter()
        {
            return WithFilter(new NoFilter());
        }

        public EventProcessorConfigurator UseEventsInAssemblies(params Assembly[] assemblies)
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

        public EventProcessorConfigurator UseEventTransformersInAssemblies(params Assembly[] assemblies)
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

        public EventProcessorConfigurator WithNoEventTransformer()
        {
            return UseEventTransformersInAssemblies();
        }

        //TODO:change return 
        public EventProcessorConfigurator Build()
        {
            _services.AddSingleton<IKafkaValidator, KafkaValidator>();

            RegisterSecondaryProducer(_secondaryProducerConfigurationAction);

            RegisterSecondaryBus(_secondaryMassTransitConfiguration);

            _services.AddSingleton(_serviceConfig);

            return this;
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
