namespace Framework.Kafka
{
    public class KafkaSenderConfiguration
    {
        public string BootstrapServers { get; set; }

        public string TopicName { get; set; }

        public int MessageTimeoutMs { get; set; }
    }
}