using Framework.Core;
using Framework.Core.Events;
using Framework.MassTransit.Message;
using MassTransit;
using Newtonsoft.Json;

namespace Framework.Idempotency;

public abstract class MassTransitIdempotentConsumer<T> : IConsumer<T> where T : class, IEvent
{
    private readonly IDuplicateMessageHandler _duplicateHandler;

    protected MassTransitIdempotentConsumer(
        IDuplicateMessageHandler duplicateHandler
    )
    {
        _duplicateHandler = duplicateHandler;
    }
    public async Task Consume(ConsumeContext<T> context)
    {
        var massTransitData = MassTransitMessageFactory.CreateFromMassTransitContext(context);
        try
        {
            var message = JsonConvert.DeserializeObject<T>(massTransitData.Message.ToString());

            if (message != null)
            {
                if (!await _duplicateHandler.HasMessageBeenProcessedBefore(message.EventId))
                {
                    await ConsumeMessage(message);

                    await _duplicateHandler.MarkMessageAsProcessed(message.EventId, DateTime.UtcNow);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    protected abstract Task ConsumeMessage(T message);
}