using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EventProcessor.Events.Kafka
{
    public class ProducerConfiguration
    {
        public string BootstrapServer { get; set; }
        public int MessageTimeoutMs { get; set; }
        public Acks Acks { get; set; }
        public string TopicName { get; set; }
        public bool EnableIdempotence { get; set; }
        public bool UseOfSpecificPartition { get; set; }
        public byte PartitionNumber { get; set; }
    }
}
