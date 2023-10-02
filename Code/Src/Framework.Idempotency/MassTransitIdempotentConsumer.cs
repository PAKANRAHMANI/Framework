using Framework.Core;
using Framework.Core.Events;
using Framework.MassTransit;
using MassTransit;
using Newtonsoft.Json;

namespace Framework.Idempotency;

public abstract class MassTransitIdempotentConsumer<T> : IConsumer<T> where T : class, IEvent
{
    private readonly IDuplicateMessageHandler _duplicateHandler;
    private readonly IUnitOfWork _unitOfWork;

    protected MassTransitIdempotentConsumer(
        IDuplicateMessageHandler duplicateHandler,
        IUnitOfWork unitOfWork
        )
    {
        _duplicateHandler = duplicateHandler;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<T> context)
    {
        var massTransitData = MassTransitMessageFactory.CreateFromMassTransitContext(context);
        try
        {
            var message = JsonConvert.DeserializeObject<T>(massTransitData.Message.ToString());

            if (message != null)
            {
                await _unitOfWork.Begin();

                if (!await _duplicateHandler.HasMessageBeenProcessedBefore(message.EventId))
                {
                    await ConsumeMessage(message);

                    await _duplicateHandler.MarkMessageAsProcessed(message.EventId, DateTime.UtcNow);

                    await _unitOfWork.Commit();
                }
            }
        }
        catch (Exception)
        {
            await _unitOfWork.RollBack();
            throw;
        }

    }
    protected abstract Task ConsumeMessage(T message);
}