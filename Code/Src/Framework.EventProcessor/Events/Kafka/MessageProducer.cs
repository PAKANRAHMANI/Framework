namespace Framework.EventProcessor.Events.Kafka;
using Confluent.Kafka;
using Configurations;

public abstract class MessageProducer
{
    protected readonly ProducerConfiguration Configuration;
    protected readonly IProducer<string, object> Producer;

    protected MessageProducer(ProducerConfiguration configuration, IProducer<string, object> producer)
    {
        Configuration = configuration;
        Producer = producer;
    }

    internal abstract Task<DeliveryResult<string, object>> ProduceAsync<TMessage>(KafkaConfig kafkaConfig, TMessage message, CancellationToken cancellationToken = default) where TMessage : class;
}