using Confluent.Kafka;
using Framework.Core.Events;
using Framework.EventProcessor.Configurations;

namespace Framework.EventProcessor.Events.Kafka
{
    internal class KafkaPartitionProducer : MessageProducer
    {
        public KafkaPartitionProducer(IProducer<string, object> producer, ProducerConfiguration configuration) : base(configuration, producer)
        {
        }

        public override async Task<DeliveryResult<string, object>> ProduceAsync<TMessage>(string key, TMessage message, CancellationToken cancellationToken = default)
        {
            return await Producer.ProduceAsync(new TopicPartition(Configuration.TopicName, new Partition(Configuration.PartitionNumber)), new Message<string, object>
            {
                Value = message,
                Key = key
            }, cancellationToken);
        }
    }

}
