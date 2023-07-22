using Confluent.Kafka;
using Framework.Core.Streaming;

namespace Framework.EventProcessor.Events.Kafka;

internal static class MessageProducerFactory
{
    internal static MessageProducer Create(IProducer<object, object> producer, ProducerConfiguration configuration)
    {
        if (configuration.UseOfSpecificPartition)
            return new KafkaPartitionProducer(producer, configuration);

        return new KafkaTopicProducer(producer, configuration);
    }
}