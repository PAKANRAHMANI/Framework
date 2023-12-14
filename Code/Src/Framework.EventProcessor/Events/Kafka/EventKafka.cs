namespace Framework.EventProcessor.Events.Kafka
{
    public class EventKafka
    {
        internal KafkaConfig KafkaConfig { get; set; }
        public Type EventType { get; set; }
    }
}
