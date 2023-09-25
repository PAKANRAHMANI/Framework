using Framework.Core.Events;
using Framework.EventProcessor.Initial;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace Framework.EventProcessor.Events.MassTransit;

public class MassTransitMultiBusEventPublisher : IEventSecondPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitMultiBusEventPublisher(Bind<ISecondBus, IPublishEndpoint> publishEndpoint)
    {
        _publishEndpoint = publishEndpoint.Value;
    }
    public async Task Publish<T>(T @event) where T : IEvent
    {
        await _publishEndpoint.Publish(@event);
    }
}