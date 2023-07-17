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
    public Task Publish<T>(T @event) where T : IEvent
    {
        return _publishEndpoint.Publish(@event);
    }
}