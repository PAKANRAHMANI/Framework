using Framework.Core.Events;

namespace Framework.EventProcessor.Events.MassTransit;

public class MassTransitMultiBusEventPublisher : IEventSecondPublisher
{
    private readonly ISecondBus _secondBus;

    public MassTransitMultiBusEventPublisher(ISecondBus secondBus)
    {
        _secondBus = secondBus;
    }
    public async Task Publish<T>(T @event) where T : IEvent
    {
        await _secondBus.Publish(@event);
    }
}