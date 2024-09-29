namespace Framework.EventProcessor.Events.Kafka;
using Confluent.Kafka;
using Configurations;

internal class KafkaTopicProducer(IProducer<string, object> producer, ProducerConfiguration configuration) : MessageProducer(configuration, producer)
{
    internal override async Task<DeliveryResult<string, object>> ProduceAsync<TMessage>(KafkaTopicKey kafkaConfig, TMessage message, CancellationToken cancellationToken = default)
    {
        return await Producer.ProduceAsync(kafkaConfig.Topic, new Message<string, object>
        {
            Value = message,
            Key = kafkaConfig.Key,
            Headers = GetHeaders()
        }, cancellationToken);
    }
    private Headers GetHeaders()
    {
        return new Headers
        {
            new Header("eventid", Guid.NewGuid().ToByteArray())
        };
    }
}