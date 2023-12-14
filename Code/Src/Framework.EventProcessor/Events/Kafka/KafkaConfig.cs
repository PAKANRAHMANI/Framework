namespace Framework.EventProcessor.Events.Kafka;

internal sealed class KafkaConfig
{
    public string Key { get; set; }
    public string Topic { get; set; }
}