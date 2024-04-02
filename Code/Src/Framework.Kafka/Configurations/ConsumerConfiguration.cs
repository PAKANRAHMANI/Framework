using Confluent.Kafka;

namespace Framework.Kafka.Configurations;
public class ConsumerConfiguration
{
    public string BootstrapServers { get; set; }
    public string TopicName { get; set; }
    public string SaslUserName { get; set; }
    public string SaslPassword { get; set; }
    public SaslMechanism? SaslMechanism { get; set; }
    public SecurityProtocol? SecurityProtocol { get; set; }
    public int MessageTimeoutMs { get; set; }
    public string GroupId { get; set; }
    public bool EnableAutoOffsetStore { get; set; }
    public bool EnableAutoCommit { get; set; }
    public AutoOffsetReset AutoOffsetReset { get; set; }
    public string PersistOffsetFilePath { get; set; }
}