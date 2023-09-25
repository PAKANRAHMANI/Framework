using Framework.Core.Events;
using Framework.Core.Filters;
using IEventPublisher = Framework.EventProcessor.Events.IEventPublisher;

namespace Framework.EventProcessor.Operations;

public class PublishEventToRabbitMqWithMassTransit : IOperation<IEvent>
{
    private readonly IEventPublisher _publisher;

    public PublishEventToRabbitMqWithMassTransit(IEventPublisher publisher)
    {
        _publisher = publisher;
    }
    public async Task<IEvent> Apply(IEvent input)
    {
        await _publisher.Publish(input);

        return input;
    }
}