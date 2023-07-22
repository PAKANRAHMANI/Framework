using Confluent.Kafka;
using Framework.Core.Streaming;

namespace Framework.EventProcessor.Events.Kafka
{
    public static class KafkaProducerFactory<TKey, TMessage>
    {
        public static IProducer<TKey, TMessage> Create(ProducerConfiguration configuration)
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
}
