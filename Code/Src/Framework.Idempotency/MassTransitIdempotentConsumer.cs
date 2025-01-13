using Framework.Core;
using Framework.Core.Events;
using Framework.MassTransit.Message;
using MassTransit;
using Newtonsoft.Json;

namespace Framework.Idempotency;

public abstract class MassTransitIdempotentConsumer<T>(IDuplicateMessageHandler duplicateHandler, bool useFromInbox = true)
    : IConsumer<T>
    where T : class, IEvent
{
    private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

    public async Task Consume(ConsumeContext<T> context)
    {
        await Semaphore.WaitAsync();

        try
        {
            var massTransitData = MassTransitMessageFactory.CreateFromMassTransitContext(context);

            var message = JsonConvert.DeserializeObject<T>(massTransitData.Message.ToString());

            if (message != null)
            {
                if (useFromInbox == false)
                {
                    await ConsumeMessage(message);

                    return;
                }

                if (!await duplicateHandler.HasMessageBeenProcessedBefore(message.EventId.ToString()))
                {
                    await ConsumeMessage(message);

                    await duplicateHandler.MarkMessageAsProcessed(message.EventId.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Semaphore.Release();
        }

    }
    protected abstract Task ConsumeMessage(T message);
}