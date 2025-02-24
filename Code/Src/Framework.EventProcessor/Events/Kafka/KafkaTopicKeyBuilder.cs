namespace Framework.EventProcessor.Events.Kafka;

public class KafkaTopicKeyBuilder
{
    private string _key;
    private string _topic;

    public KafkaTopicKeyBuilder WithKey(string key)
    {
        _key = key;
        return this;
    }

    public KafkaTopicKeyBuilder WithTopic(string topic)
    {
        _topic = topic;
        return this;
    }

    public KafkaTopicKey Build()
    {
        return new KafkaTopicKey
        {
            Key = _key,
            Topic = _topic
        };
    }
}