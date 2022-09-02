using System.Threading.Tasks;
using Confluent.Kafka;

namespace Framework.Kafka
{
    public interface IOffsetManager
    {
        TopicPartitionOffset CurrentOffset(string path);
        void Persist(string path, TopicPartitionOffset topicPartitionOffset);
    }
}
