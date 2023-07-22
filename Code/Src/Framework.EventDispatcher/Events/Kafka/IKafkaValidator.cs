namespace Framework.EventProcessor.Events.Kafka;

public interface IKafkaValidator
{
    bool TopicIsExist(string topic);
}