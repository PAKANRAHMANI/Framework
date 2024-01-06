using Confluent.Kafka;
using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.Serialization;

namespace Framework.EventProcessor.Events.Kafka
{
    public static class KafkaProducerFactory<TKey, TMessage> where TMessage : class
    {
        public static IProducer<TKey, TMessage> Create(ProducerConfiguration configuration, ILogger logger)
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = configuration.BootstrapServer,
                MessageTimeoutMs = configuration.MessageTimeoutMs,
                Acks = configuration.Acks,
                EnableIdempotence = configuration.EnableIdempotence,
                CompressionType = CompressionType.Snappy
            };

            return new ProducerBuilder<TKey, TMessage>(config)
                .SetValueSerializer(new KafkaJsonSerializer<TMessage>(logger))
                .SetLogHandler((producer, logMessage) => logger.Write($"{producer.Name} : {logMessage.Message} - Kafka Log Level Is : {logMessage.Level}", LogLevel.Information))
                .SetErrorHandler((producer, error) => logger.WriteException(new KafkaException(error)))
                .Build();
        }
    }
}
