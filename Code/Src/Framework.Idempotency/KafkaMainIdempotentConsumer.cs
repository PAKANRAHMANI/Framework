using Confluent.Kafka;
using Framework.Core.Logging;
using Framework.Kafka.Configurations;
using Framework.Sentry;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace Framework.Idempotency;

public abstract class KafkaMainIdempotentConsumer(
    IDuplicateMessageHandler duplicateMessageHandler,
    ISentryService sentryService,
    ILogger logger,
    ConsumerConfiguration consumerConfiguration,
    bool useFromInbox = true) : BackgroundService
{
    private readonly IConsumer<string, string> _consumer =
        new ConsumerBuilder<string, string>(new ConsumerConfig
        {
            GroupId = consumerConfiguration.GroupId,
            BootstrapServers = consumerConfiguration.BootstrapServers,
            EnableAutoOffsetStore = consumerConfiguration.EnableAutoOffsetStore,
            AutoOffsetReset = consumerConfiguration.AutoOffsetReset,
            EnableAutoCommit = consumerConfiguration.EnableAutoCommit
        }).Build();
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            _consumer.Subscribe(consumerConfiguration.TopicName);

            while (!cancellationToken.IsCancellationRequested)
            {
                ConsumeResult<string, string> consumeResult = _consumer.Consume(cancellationToken);

                await ReceiveMessage(consumeResult);
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

            if (useFromInbox == false)
            {
                await ProcessMessageAsync(consumeResult);

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
            var newOffset = new TopicPartitionOffset(consumeResult.Topic, consumeResult.Partition,
                consumeResult.Offset);

            _consumer.Seek(newOffset);

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

    private static bool IsGuid(byte[] byteArray)
    {
        return byteArray.Length == 16;
    }
}