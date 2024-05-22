namespace Framework.DataAccess.CH.Engines;

public interface IKafkaEngineTemplate
{
    Task Create(KafkaEngine kafkaEngine);
}