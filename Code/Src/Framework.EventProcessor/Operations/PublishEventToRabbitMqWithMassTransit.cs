using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.Core.Logging;
using IEventPublisher = Framework.EventProcessor.Events.IEventPublisher;

namespace Framework.EventProcessor.Operations;

internal sealed class PublishEventToRabbitMqWithMassTransit(IEventPublisher publisher, ILogger logger) : IOperation<IEvent>
{
    public async Task<IEvent> Apply(IEvent input)
    {
        try
        {
            await publisher.Publish(input);

            return input;
        }
        catch (Exception e)
        {
            logger.WriteException(e);
            return input;
        }
    }
}