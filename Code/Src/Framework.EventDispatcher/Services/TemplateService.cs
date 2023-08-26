using Framework.Core.ChainHandlers;
using Framework.Core.Events;
using Framework.EventProcessor.DataStore;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Serialization;
using Framework.EventProcessor.Transformation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Framework.EventProcessor.Handlers;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace Framework.EventProcessor.Services;

public abstract class TemplateService : BackgroundService, IDataStoreChangeTrackerObserver
{
    private readonly IEventTypeResolver _eventTypeResolver;
    private readonly IEventFilter _eventFilter;
    private readonly IEventTransformerLookUp _transformerLookUp;
    private readonly ILogger<TemplateService> _logger;
    private readonly IDataStoreObservable _dataStore;
    private readonly ServiceConfig _serviceConfig;
    private readonly IOptions<SecondaryProducerConfiguration> _secondaryProducerConfiguration;
    private readonly Bind<ISecondBus, IPublishEndpoint> _secondaryPublishEndpoint;
    private ISubscription _subscription;
    protected TemplateService(
        IEventTypeResolver eventTypeResolver,
        IEventFilter eventFilter,
        IEventTransformerLookUp transformerLookUp,
        ILogger<TemplateService> logger,
        IDataStoreObservable dataStore,
        ServiceConfig serviceConfig,
        IOptions<SecondaryProducerConfiguration> secondaryProducerConfiguration,
        Bind<ISecondBus, IPublishEndpoint> secondaryPublishEndpoint
        )
    {
        _eventTypeResolver = eventTypeResolver;
        _eventFilter = eventFilter;
        _transformerLookUp = transformerLookUp;
        _logger = logger;
        _dataStore = dataStore;
        _serviceConfig = serviceConfig;
        _secondaryProducerConfiguration = secondaryProducerConfiguration;
        _secondaryPublishEndpoint = secondaryPublishEndpoint;
        _dataStore.SetSubscriber(this);

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
                eventToPublish = TransformEvent(eventToPublish, eventItem);

                await Send(eventToPublish);

                SecondarySendMessage(eventToPublish);

                _logger.LogInformation($"Event '{eventItem.EventType}-{eventItem.EventId}' Published on bus.");
            }
            else
            {
                _logger.LogInformation($"Publishing Event '{eventItem.EventType}-{eventItem.EventId}' Skipped because of filter");
            }
        }
    }

    public void SecondarySendMessage(IEvent @event)
    {
        if (!_serviceConfig.EnableSecondarySending) return;

        var handlers =
            new ChainBuilder<SecondaryData>()
                .WithHandler(new KafkaHandler(_secondaryProducerConfiguration))
                .WithHandler(new MassTransitHandler(_secondaryPublishEndpoint))
                .WithEndOfChainHandler()
                .Build();

        handlers.Handle(new SecondaryData(_serviceConfig, @event));
    }
    public override void Dispose()
    {
        _subscription.UnSubscribe();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Start();

                _logger.LogInformation("Event host Service running at: {time}", DateTimeOffset.Now);

                _subscription = _dataStore.SubscribeForChanges();

                _logger.LogInformation("Subscribed to data store");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                _subscription.UnSubscribe();
            }
        }
    }
    protected abstract Task Start();
    protected abstract Task Send(IEvent @event);
    private IEvent TransformEvent(IEvent @event, EventItem item)
    {
        var transformer = _transformerLookUp.LookUpTransformer(@event);

        if (transformer == null) return @event;

        @event = transformer.Transform(@event);

        _logger.LogInformation($"Event '{item.EventType}-{item.EventId}' Transformed");

        return @event;
    }
}