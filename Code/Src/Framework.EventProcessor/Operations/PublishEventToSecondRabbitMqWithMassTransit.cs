using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.Core.Logging;
using Framework.EventProcessor.Events;

namespace Framework.EventProcessor.Operations;

public class PublishEventToSecondRabbitMqWithMassTransit : IOperation<IEvent>
{
    private readonly IEventSecondPublisher _publisher;
    private readonly ILogger _logger;

    public PublishEventToSecondRabbitMqWithMassTransit(IEventSecondPublisher publisher,ILogger logger)
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