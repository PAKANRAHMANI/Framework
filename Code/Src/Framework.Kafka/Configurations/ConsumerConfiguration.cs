using Confluent.Kafka;

namespace Framework.Kafka.Configurations;
public class ConsumerConfiguration
{
    public string BootstrapServers { get; set; }
    public string TopicName { get; set; }
    public int MessageTimeoutMs { get; set; }
    public string GroupId { get; set; }
    public bool EnableAutoOffsetStore { get; set; }
    public bool EnableAutoCommit { get; set; }
    public AutoOffsetReset AutoOffsetReset { get; set; }
    public string PersistOffsetFilePath { get; set; }
    public string SaslUserName { get; set; }
    public string SaslPassword { get; set; }
    public SaslMechanism? SaslMechanism { get; set; }
    public SecurityProtocol? SecurityProtocol { get; set; }
    public IsolationLevel? IsolationLevel { get; set; }
    public int? AutoCommitIntervalMs  { get; set; }
    public int? HeartbeatIntervalMs { get; set; }
    public int? FetchErrorBackoffMs { get; set; }
    public int? FetchWaitMaxMs { get; set; }
    public int? SessionTimeoutMs { get; set; }
}