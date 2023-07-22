namespace Framework.EventProcessor.Events.Kafka;
using Confluent.Kafka;

public class KafkaTopicProducer : MessageProducer
{
    public KafkaTopicProducer(IProducer<object, object> producer, ProducerConfiguration configuration) : base(configuration, producer)
    {
    }

    public override void Produce<TKey, TMessage>(TKey key, TMessage message, Action<DeliveryResult<object, object>> action = null)
    {
        Producer.Produce(Configuration.TopicName, new Message<object, object>
        {
            Value = message,
            Key = key
        }, action);
    }
}