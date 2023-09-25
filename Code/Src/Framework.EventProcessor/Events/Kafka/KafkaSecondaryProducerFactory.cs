using Confluent.Kafka;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.Serialization;

namespace Framework.EventProcessor.Events.Kafka;

public static class KafkaSecondaryProducerFactory<TKey, TMessage> where TMessage : class
{
    public static IProducer<TKey, TMessage> Create(SecondaryProducerConfiguration configuration)
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = configuration.BootstrapServer,
            MessageTimeoutMs = configuration.MessageTimeoutMs,
            Acks = configuration.Acks,
            EnableIdempotence = configuration.EnableIdempotence
        };

        return new ProducerBuilder<TKey, TMessage>(config)
            .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
            .Build();
    }
}