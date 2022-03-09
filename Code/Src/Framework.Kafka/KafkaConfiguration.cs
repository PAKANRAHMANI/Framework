using Confluent.Kafka;

namespace Framework.Kafka
{
    public class KafkaConfiguration
    {
        public string BootstrapServers { get; set; }

        public string ProducerTopicName { get; set; }
        public string ConsumerTopicName { get; set; }

        public int MessageTimeoutMs { get; set; }
        
        public Acks Acks { get; set; }

        public string GroupId { get; set; }

        public bool EnableAutoOffsetStore { get; set; }

        public AutoOffsetReset AutoOffsetReset { get; set; }
    }
}