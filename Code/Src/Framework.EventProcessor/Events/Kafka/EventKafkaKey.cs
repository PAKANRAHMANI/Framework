namespace Framework.EventProcessor.Events.Kafka
{
    public class EventKafkaKey
    {
        public string KafkaKey { get; set; }
        public Type EventType { get; set; }
    }
}
