using Confluent.Kafka;

namespace Framework.EventProcessor.Events.Kafka
{
    internal class KafkaPartitionProducer : MessageProducer
    {
        public KafkaPartitionProducer(IProducer<object, object> producer, ProducerConfiguration configuration) : base(configuration, producer)
        {
        }
        public override async Task<DeliveryResult<object, object>> ProduceAsync<TKey, TMessage>(TKey key, TMessage message, CancellationToken cancellationToken = default)
        {
            return await Producer.ProduceAsync(new TopicPartition(Configuration.TopicName, new Partition(Configuration.PartitionNumber)), new Message<object, object>
            {
                Value = message,
                Key = key
            }, cancellationToken);
        }
    }

}
