namespace Framework.EventProcessor.Events.Kafka
{
    public sealed class KafkaEvent
    {
        public List<KafkaTopicKey> KafkaTopicKey { get; set; } = [];
        public Type EventType { get; set; }
    }
}
