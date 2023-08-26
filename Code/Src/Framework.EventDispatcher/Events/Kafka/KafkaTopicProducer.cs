namespace Framework.EventProcessor.Events.Kafka;
using Confluent.Kafka;

public class KafkaTopicProducer : MessageProducer
{
    public KafkaTopicProducer(IProducer<object, object> producer, ProducerConfiguration configuration) : base(configuration, producer)
    {
    }

    public override async Task<DeliveryResult<object, object>> ProduceAsync<TKey, TMessage>(TKey key, TMessage message, CancellationToken cancellationToken = default)
    {
        return await Producer.ProduceAsync(Configuration.TopicName, new Message<object, object>
        {
            Value = message,
            Key = key
        }, cancellationToken);
    }
}