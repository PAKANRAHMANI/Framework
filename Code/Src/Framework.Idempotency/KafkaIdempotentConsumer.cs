﻿using System.Text;
using Confluent.Kafka;
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
                await consumer.ConsumeAsync(ReceiveMessage, cancellationToken);
            }
        }
        catch (Exception exp)
        {
            HandleException(exp);
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
        return await duplicateMessageHandler.HasMessageBeenProcessedBefore(eventId);
    }

    private async Task MarkMessageAsProcessedAsync(string eventId)
    {
        await duplicateMessageHandler.MarkMessageAsProcessed(eventId);
    }

    private static string TryGetEventIdFromHeaders(Headers headers)
    {
        return headers.TryGetLastBytes("eventid", out var eventIdBytes) ? Encoding.UTF8.GetString(eventIdBytes) : null;
    }

    private void HandleException(Exception exp)
    {
        logger.WriteException(exp);
    }
}