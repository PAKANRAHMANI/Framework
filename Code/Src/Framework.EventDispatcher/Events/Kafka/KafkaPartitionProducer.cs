using Confluent.Kafka;
using Framework.Core.Streaming;

namespace Framework.EventProcessor.Events.Kafka
{
    internal class KafkaPartitionProducer<TKey, TMessage> : KafkaProducer<TKey, TMessage> where TMessage : IStream
    {
        internal KafkaPartitionProducer(ProducerConfiguration configuration) : base(configuration)
        {
        }

        protected override void Produce(TKey key, TMessage message, Action<DeliveryResult<TKey, TMessage>> action = null)
        {
            Producer.Produce(new TopicPartition(Configuration.TopicName, new Partition(Configuration.PartitionNumber)), new Message<TKey, TMessage>
            {
                Value = message,
                Key = key
            }, action);
        }
    }

}
