using Confluent.Kafka;
using Framework.Core.Logging;
using Framework.Kafka;
using Framework.Sentry;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Text;
using static MassTransit.ValidationResultExtensions;

namespace Framework.Idempotency;

public abstract class KafkaIdempotentConsumer(
    IDuplicateMessageHandler duplicateMessageHandler,
    IKafkaConsumer<string, string> consumer,
    ISentryService sentryService,
    ILogger logger)
    : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {

                await consumer.ConsumeAsync(ReceiveMessage, cancellationToken);

            }
        }
        catch (Exception exp)
        {
            logger.WriteException(exp);

            sentryService.CaptureException(exp);
        }
    }

    private async Task ReceiveMessage(ConsumeResult<string, string> consumeResult)
    {
        try
        {
            if (consumeResult == null)
            {
                logger.Write("Consumed Message is null", LogLevel.Warning);

                return;
            }

            var eventId = TryGetEventIdFromHeaders(consumeResult.Message.Headers);

            if (eventId != null)
            {
                if (await IsMessageProcessedAsync(eventId))
                {
                    logger.Write($"Message with Id {eventId} has already been processed.", LogLevel.Warning);

                    return;
                }

                await ProcessMessageAsync(consumeResult);

                await MarkMessageAsProcessedAsync(eventId);
            }
            else
            {
                await ProcessMessageAsync(consumeResult);
            }
        }
        catch (Exception e)
        {
            consumer.DoSeek(consumeResult);

            logger.WriteException(e);

            sentryService.CaptureException(e);
        }
    }

    protected abstract Task ProcessMessageAsync(ConsumeResult<string, string> consumeResult);

    private async Task<bool> IsMessageProcessedAsync(string eventId)
    {
        try
        {
            return await duplicateMessageHandler.HasMessageBeenProcessedBefore(eventId);
        }
        catch (Exception e)
        {
            logger.WriteException(e);

            sentryService.CaptureException(e);

            return false;
        }
    }

    private async Task MarkMessageAsProcessedAsync(string eventId)
    {
        try
        {
            await duplicateMessageHandler.MarkMessageAsProcessed(eventId);
        }
        catch (Exception e)
        {
            logger.WriteException(e);

            sentryService.CaptureException(e);

            await Task.CompletedTask;
        }
    }

    private string TryGetEventIdFromHeaders(Headers headers)
    {
        try
        {
            headers.TryGetLastBytes("eventid", out var eventIdBytes);

            if (eventIdBytes is null)
                return null;

            if (IsGuid(eventIdBytes))
                return new Guid(eventIdBytes).ToString();

            return Encoding.UTF8.GetString(eventIdBytes);


        }
        catch (Exception e)
        {
            logger.WriteException(e);

            sentryService.CaptureException(e);

            return null;
        }
    }

    bool IsGuid(byte[] byteArray)
    {
        if (byteArray.Length != 16)
            return false;
        return false;
    }
}