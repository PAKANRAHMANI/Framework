using Framework.Core.Events;

namespace Framework.EventProcessor.Events.MassTransit;

internal sealed class MassTransitMultiBusEventPublisher(ISecondBus secondBus) : IEventSecondPublisher
{
    public async Task Publish<T>(T @event) where T : IEvent
    {
        await secondBus.Publish(@event);
    }
}