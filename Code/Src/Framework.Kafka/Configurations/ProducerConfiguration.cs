using Confluent.Kafka;

namespace Framework.Kafka.Configurations;
public class ProducerConfiguration
{
    public string BootstrapServers { get; set; }
    public string TopicName { get; set; }
    public string SaslUserName { get; set; }
    public string SaslPassword { get; set; }
    public SaslMechanism? SaslMechanism { get; set; }
    public SecurityProtocol? SecurityProtocol { get; set; }
    public int MessageTimeoutMs { get; set; }
    public Acks Acks { get; set; }
}
