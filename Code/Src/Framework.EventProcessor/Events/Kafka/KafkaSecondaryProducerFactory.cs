using Confluent.Kafka;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.Serialization;
using Framework.Core.Logging;

namespace Framework.EventProcessor.Events.Kafka;

public static class KafkaSecondaryProducerFactory<TKey, TMessage> where TMessage : class
{
    public static IProducer<TKey, TMessage> Create(SecondaryProducerConfiguration configuration, ILogger logger)
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = configuration.BootstrapServer,
            MessageTimeoutMs = configuration.MessageTimeoutMs,
            Acks = configuration.Acks,
            EnableIdempotence = configuration.EnableIdempotence,
            SaslUsername = configuration.SaslUserName,
            SaslPassword = configuration.SaslPassword,
            SaslMechanism = configuration.SaslMechanism,
            SecurityProtocol = configuration.SecurityProtocol,
            CompressionType = CompressionType.Snappy,
        };

        return new ProducerBuilder<TKey, TMessage>(config)
            .SetValueSerializer(new KafkaJsonSerializer<TMessage>(logger))
            .SetLogHandler((producer, logMessage) => logger.Write($"{producer.Name} : {logMessage.Message} - Kafka Log Level Is : {logMessage.Level}", LogLevel.Information))
            .SetErrorHandler((producer, error) => logger.WriteException(new KafkaException(error)))
            .Build();
    }
}