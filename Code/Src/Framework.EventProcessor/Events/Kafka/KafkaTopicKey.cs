namespace Framework.EventProcessor.Events.Kafka;

public sealed class KafkaTopicKey
{
    public string Key { get; set; }
    public string Topic { get; set; }
}