using Confluent.Kafka;

namespace Framework.EventProcessor.Configurations
{
    public class ProducerConfiguration
    {
        public string BootstrapServer { get; set; }
        public int MessageTimeoutMs { get; set; }
        public Acks Acks { get; set; }
        public bool EnableIdempotence { get; set; }
        public bool UseOfSpecificPartition { get; set; }
        public string TopicName { get; set; }
        public byte PartitionNumber { get; set; }
        public string TopicKey { get; set; }
        public string SaslUserName { get; set; }
        public string SaslPassword { get; set; }
        public SaslMechanism? SaslMechanism { get; set; }
        public SecurityProtocol? SecurityProtocol { get; set; }
    }
}
