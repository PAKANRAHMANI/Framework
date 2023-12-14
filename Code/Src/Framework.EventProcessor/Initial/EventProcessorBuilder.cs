using Framework.Core.Events;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.DataStore;
using Framework.EventProcessor.DataStore.ChangeTrackers;
using Framework.EventProcessor.DataStore.MongoDB;
using Framework.EventProcessor.DataStore.Sql;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Events.MassTransit;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Operations;
using Framework.EventProcessor.Services;
using Framework.EventProcessor.Transformation;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Reflection;
using IEventPublisher = Framework.EventProcessor.Events.IEventPublisher;

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
        private readonly Dictionary<int, Type> _operations = new();
        private Dictionary<Type, KafkaConfig> _kafkaKeys = new();
        private int _operationPriority = 0;
        private EventProcessorBuilder(IServiceCollection serviceCollection)
        {
            _services = serviceCollection;
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

            var mongoStoreConfig = new MongoStoreConfig();

            config.Invoke(mongoStoreConfig);

            _services.AddSingleton<MongoStoreConfig>(mongoStoreConfig);

            _services.AddSingleton<IMongoDatabase>(CreateMongoDb(mongoStoreConfig));

            return this;
        }

        public IEventTransformer WithFilter(IEventFilter filter)
        {
            _services.AddSingleton(filter);
            return this;
        }

        public IEventTransformer WithNoFilter()
        {
            return WithFilter(new NoEventFilter());
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
            var massTransitConfig = new MassTransitConfig();

            config.Invoke(massTransitConfig);

            _services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((_, rabbitMqBusFactoryConfigurator) =>
                {
                    rabbitMqBusFactoryConfigurator.Host(massTransitConfig.RabbitMqConnectionString);
                });
            });

            _services.AddSingleton<IEventPublisher, MassTransitEventPublisher>();

            _services.AddSingleton(massTransitConfig);

            _operations.Add(++_operationPriority, typeof(PublishEventToRabbitMqWithMassTransit));

            return this;
        }

        public IEnableSecondSenderBuilder ProduceMessageWithKafka(Action<ProducerConfiguration> config, List<EventKafka> kafkaKeys)
        {
            RegisterMessageProducer(config);

            _operations.Add(++_operationPriority, typeof(PublishEventToKafka));

            _kafkaKeys = kafkaKeys.ToDictionary(a => a.EventType, b => b.KafkaConfig);

            _services.AddSingleton(_kafkaKeys);

            return this;
        }
        public ISecondaryDeliveryEvent EnableSendingMessageToSecondaryBroker()
        {
            return this;
        }

        public IEventProcessor DisableSendingMessageToSecondaryBroker()
        {
            return this;
        }

        public IEventProcessor SecondaryDeliveryWithKafka(Action<SecondaryProducerConfiguration> config)
        {
            RegisterSecondaryProducer(config);

            _operations.Add(++_operationPriority, typeof(PublishEventToSecondKafka));

            return this;
        }

        public IEventProcessor SecondaryDeliveryWithMassTransit(Action<SecondaryMassTransitConfiguration> config)
        {

            RegisterSecondaryBus(config);

            _operations.Add(++_operationPriority, typeof(PublishEventToSecondRabbitMqWithMassTransit));

            return this;
        }

        public void Build()
        {
            foreach (var operation in _operations)
            {
                _services.AddSingleton(operation.Value);
            }
            _services.AddSingleton(_operations);

            _services.AddSingleton<IDataStoreChangeTrackerObserver, DataStoreChangeTrackerObserver>();

            _services.AddHostedService<EventWorker>();
        }
        private void RegisterMessageProducer(Action<ProducerConfiguration> config)
        {
            var producerConfig = new ProducerConfiguration();

            config.Invoke(producerConfig);

            _services.AddSingleton(producerConfig);

            var kafkaProducer = KafkaProducerFactory<string, object>.Create(producerConfig);

            var producer = MessageProducerFactory.Create(kafkaProducer, producerConfig);

            _services.AddSingleton(producer);
        }
        private void RegisterSecondaryBus(Action<SecondaryMassTransitConfiguration> config)
        {
            var secondaryMassTransitConfiguration = new SecondaryMassTransitConfiguration();

            config.Invoke(secondaryMassTransitConfiguration);

            _services.AddSingleton(secondaryMassTransitConfiguration);

            _services.AddMassTransit<ISecondBus>(x =>
            {
                x.UsingRabbitMq((_, rabbitMqBusFactoryConfigurator) =>
                {
                    rabbitMqBusFactoryConfigurator.Host(secondaryMassTransitConfiguration.RabbitMqConnectionString);
                });
            });

            _services.AddSingleton<IEventSecondPublisher, MassTransitMultiBusEventPublisher>();

        }
        private void RegisterSecondaryProducer(Action<SecondaryProducerConfiguration> config)
        {
            var producerConfig = new SecondaryProducerConfiguration();

            config.Invoke(producerConfig);

            _services.AddSingleton(producerConfig);
        }
        private IMongoDatabase CreateMongoDb(MongoStoreConfig mongoStoreConfig)
        {
            var client = new MongoClient(mongoStoreConfig.ConnectionString);

            return client.GetDatabase(mongoStoreConfig.DatabaseName);
        }
    }
}
