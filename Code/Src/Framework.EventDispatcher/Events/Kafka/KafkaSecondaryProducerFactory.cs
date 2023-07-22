using Confluent.Kafka;

namespace Framework.EventProcessor.Events.Kafka;

public static class KafkaSecondaryProducerFactory<TKey, TMessage>
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

        return new ProducerBuilder<TKey, TMessage>(config).Build();
    }
}