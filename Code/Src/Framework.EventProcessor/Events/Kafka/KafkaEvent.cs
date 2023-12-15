namespace Framework.EventProcessor.Events.Kafka
{
    public class KafkaEvent
    {
        public KafkaTopicKey KafkaTopicKey { get; set; }
        public Type EventType { get; set; }
    }
}
