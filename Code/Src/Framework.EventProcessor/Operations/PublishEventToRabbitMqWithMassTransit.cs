using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.Core.Logging;
using IEventPublisher = Framework.EventProcessor.Events.IEventPublisher;

namespace Framework.EventProcessor.Operations;

public class PublishEventToRabbitMqWithMassTransit : IOperation<IEvent>
{
    private readonly IEventPublisher _publisher;
    private readonly ILogger _logger;

    public PublishEventToRabbitMqWithMassTransit(IEventPublisher publisher,ILogger logger)
    {
        _publisher = publisher;
        _logger = logger;
    }
    public async Task<IEvent> Apply(IEvent input)
    {
        try
        {
            await _publisher.Publish(input);

            return input;
        }
        catch (Exception e)
        {
            _logger.WriteException(e);
            return input;
        }
    }
}