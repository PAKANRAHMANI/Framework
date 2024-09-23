﻿using Confluent.Kafka;
using Framework.Core.Logging;
using Framework.Kafka;
using Microsoft.Extensions.Hosting;

namespace Framework.Idempotency;

public abstract class KafkaIdempotentConsumer(
    IDuplicateMessageHandler duplicateMessageHandler,
    IKafkaConsumer<string, string> consumer,
    ILogger logger)
    : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await consumer.ConsumeAsync(ReceiveMessage, cancellationToken);
                }
                catch (Exception exp)
                {
                    logger.WriteException(exp);
                }
            }
        }
        catch (Exception exp)
        {
            logger.WriteException(exp);
        }
    }

    private async Task ReceiveMessage(ConsumeResult<string, string> consumeResult)
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

            await Task.CompletedTask;
        }
    }

    private string TryGetEventIdFromHeaders(Headers headers)
    {
        try
        {
            return headers.TryGetLastBytes("eventid", out var eventIdBytes) ? new Guid(eventIdBytes).ToString() : null;
        }
        catch (Exception e)
        {
            logger.WriteException(e);

            return null;
        }
    }
}