using Framework.Core.Events;
using MassTransit;

namespace Framework.EventProcessor.Events.MassTransit;

internal sealed class MassTransitEventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public async Task Publish<T>(T @event) where T : IEvent
    {
        await publishEndpoint.Publish(@event);
    }
}