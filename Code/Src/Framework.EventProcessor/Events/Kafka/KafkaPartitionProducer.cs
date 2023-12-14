using Confluent.Kafka;
using Framework.EventProcessor.Configurations;

namespace Framework.EventProcessor.Events.Kafka
{
    internal class KafkaPartitionProducer : MessageProducer
    {
        public KafkaPartitionProducer(IProducer<string, object> producer, ProducerConfiguration configuration) : base(configuration, producer)
        {
        }

        internal override async Task<DeliveryResult<string, object>> ProduceAsync<TMessage>(KafkaConfig kafkaConfig, TMessage message, CancellationToken cancellationToken = default)
        {
            return await Producer.ProduceAsync(new TopicPartition(kafkaConfig.Topic, new Partition(Configuration.PartitionNumber)), new Message<string, object>
            {
                Value = message,
                Key = kafkaConfig.Key
            }, cancellationToken);
        }
    }

}
