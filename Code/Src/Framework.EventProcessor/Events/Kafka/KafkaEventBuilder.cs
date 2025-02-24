namespace Framework.EventProcessor.Events.Kafka;

public class KafkaEventBuilder
{
    private Type _eventType;
    private List<KafkaTopicKey> _kafkaTopicKeys;

    public KafkaEventBuilder WithType(Type eventType)
    {
        _eventType = eventType;
        return this;
    }
    public KafkaEventBuilder WithKafkaTopicKey(params KafkaTopicKey[] kafkaTopicKeys)
    {
        _kafkaTopicKeys = kafkaTopicKeys.ToList();
        return this;
    }
}