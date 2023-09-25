using Confluent.Kafka;
using Framework.Core.Events;
using Framework.Core.Streaming;
using Framework.EventProcessor.Configurations;

namespace Framework.EventProcessor.Events.Kafka;

internal static class MessageProducerFactory
{
    internal static MessageProducer Create(IProducer<string, object> producer, ProducerConfiguration configuration)
    {
        if (configuration.UseOfSpecificPartition)
            return new KafkaPartitionProducer(producer, configuration);

        return new KafkaTopicProducer(producer, configuration);
    }
}