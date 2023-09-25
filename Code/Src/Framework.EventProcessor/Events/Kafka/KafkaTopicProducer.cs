namespace Framework.EventProcessor.Events.Kafka;
using Confluent.Kafka;
using Configurations;

public class KafkaTopicProducer : MessageProducer
{
    public KafkaTopicProducer(IProducer<string, object> producer, ProducerConfiguration configuration) : base(configuration, producer)
    {
    }

    public override async Task<DeliveryResult<string, object>> ProduceAsync<TMessage>(string key, TMessage message, CancellationToken cancellationToken = default)
    {
        return await Producer.ProduceAsync(Configuration.TopicName, new Message<string, object>
        {
            Value = message,
            Key = key
        }, cancellationToken);
    }
}