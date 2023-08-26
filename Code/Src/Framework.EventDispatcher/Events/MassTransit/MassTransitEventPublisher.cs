using Framework.Core.Events;
using MassTransit;

namespace Framework.EventProcessor.Events.MassTransit;

public class MassTransitEventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    public async Task Publish<T>(T @event) where T : IEvent
    {
        await _publishEndpoint.Publish(@event);
    }
}