using Confluent.Kafka;

namespace Framework.Kafka
{
    public class KafkaConfigurations
    {
        public ConsumerConfiguration ConsumerConfiguration { get; set; }
        public ProducerConfiguration ProducerConfiguration { get; set; }
    }

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
}