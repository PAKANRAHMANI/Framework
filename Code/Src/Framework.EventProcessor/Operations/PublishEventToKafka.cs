﻿using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.Events.Kafka;

namespace Framework.EventProcessor.Operations;

internal sealed class PublishEventToKafka(
    MessageProducer producer,
    ProducerConfiguration producerConfiguration,
    IReadOnlyDictionary<Type, KafkaTopicKey> kafkaKeys,
    ILogger logger) : IOperation<IEvent>
{
    public async Task<IEvent> Apply(IEvent input)
    {
        try
        {
            var eventType = input.GetType();

            if (kafkaKeys.TryGetValue(eventType, out var kafkaTopicKey))
            {
                await producer.ProduceAsync(kafkaTopicKey, input);
            }
            else
                await producer.ProduceAsync(new KafkaTopicKey { Key = producerConfiguration.TopicKey, Topic = producerConfiguration.TopicName }, input);

            return input;
        }
        catch (Exception e)
        {
            logger.WriteException(e);
            return input;
        }
    }
}