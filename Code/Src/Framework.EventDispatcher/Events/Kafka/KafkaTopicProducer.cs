using Framework.Core.Streaming;
namespace Framework.EventProcessor.Events.Kafka;
using Confluent.Kafka;

internal class KafkaTopicProducer<TKey, TMessage> : KafkaProducer<TKey, TMessage> where TMessage : IStream
{
    internal KafkaTopicProducer(ProducerConfiguration configuration) : base(configuration)
    {
    }

    protected override void Produce(TKey key, TMessage message, Action<DeliveryResult<TKey, TMessage>> action = null)
    {
        Producer.Produce(Configuration.TopicName, new Message<TKey, TMessage>
        {
            Value = message,
            Key = key
        }, action);
    }
}