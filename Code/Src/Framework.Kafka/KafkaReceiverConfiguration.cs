using System.Collections.Generic;
using Confluent.Kafka;

namespace Framework.Kafka
{
    public class KafkaReceiverConfiguration
    {
        public string BootstrapServers { get; set; }

        public string GroupId { get; set; }

        public bool EnableAutoOffsetStore { get; set; }

        public AutoOffsetReset AutoOffsetReset { get; set; }

        public string TopicName { get; set; }
    }
}
