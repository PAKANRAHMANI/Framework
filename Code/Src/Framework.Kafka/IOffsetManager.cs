using System.Threading.Tasks;
using Confluent.Kafka;

namespace Framework.Kafka
{
    public interface IOffsetManager
    {
        Task<TopicPartitionOffset> CurrentOffset(string path);
        Task Persist(string path, TopicPartitionOffset topicPartitionOffset);
    }
}
