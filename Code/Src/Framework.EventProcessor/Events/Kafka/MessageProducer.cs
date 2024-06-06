namespace Framework.EventProcessor.Events.Kafka;
using Confluent.Kafka;
using Configurations;

internal abstract class MessageProducer(ProducerConfiguration configuration, IProducer<string, object> producer)
{
    protected readonly ProducerConfiguration Configuration = configuration;
    protected readonly IProducer<string, object> Producer = producer;

    internal abstract Task<DeliveryResult<string, object>> ProduceAsync<TMessage>(KafkaTopicKey kafkaConfig, TMessage message, CancellationToken cancellationToken = default) where TMessage : class;
}