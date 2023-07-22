﻿using Confluent.Kafka;

namespace Framework.EventProcessor.Events.Kafka
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
        public object TopicKey { get; set; }
    }
}
