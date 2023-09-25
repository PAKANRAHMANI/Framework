using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.EventProcessor.Events;

namespace Framework.EventProcessor.Operations;

public class PublishEventToSecondRabbitMqWithMassTransit : IOperation<IEvent>
{
    private readonly IEventSecondPublisher _publisher;

    public PublishEventToSecondRabbitMqWithMassTransit(IEventSecondPublisher publisher)
    {
        _publisher = publisher;
    }
    public async Task<IEvent> Apply(IEvent input)
    {
        await _publisher.Publish(input);

        return input;
    }
}