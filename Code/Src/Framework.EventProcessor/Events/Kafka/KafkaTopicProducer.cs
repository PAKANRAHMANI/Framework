namespace Framework.EventProcessor.Events.Kafka;
using Confluent.Kafka;
using Configurations;

public class KafkaTopicProducer : MessageProducer
{
    public KafkaTopicProducer(IProducer<string, object> producer, ProducerConfiguration configuration) : base(configuration, producer)
    {
    }

    internal override async Task<DeliveryResult<string, object>> ProduceAsync<TMessage>(KafkaTopicKey kafkaConfig, TMessage message, CancellationToken cancellationToken = default)
    {
        return await Producer.ProduceAsync(kafkaConfig.Topic, new Message<string, object>
        {
            Value = message,
            Key = kafkaConfig.Key
        }, cancellationToken);
    }
}