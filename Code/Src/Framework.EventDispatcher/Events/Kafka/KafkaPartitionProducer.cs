using Confluent.Kafka;
using Framework.Core.Streaming;

namespace Framework.EventProcessor.Events.Kafka
{
    internal class KafkaPartitionProducer : MessageProducer
    {
        public KafkaPartitionProducer(IProducer<object, object> producer, ProducerConfiguration configuration) : base(configuration, producer)
        {
        }

        public override void Produce<TKey, TMessage>(TKey key, TMessage message, Action<DeliveryResult<object, object>> action = null)
        {
            Producer.Produce(new TopicPartition(Configuration.TopicName, new Partition(Configuration.PartitionNumber)), new Message<object, object>
            {
                Value = message,
                Key = key
            }, action);
        }
    }

}
