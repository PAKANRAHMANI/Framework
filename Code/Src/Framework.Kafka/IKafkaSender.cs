using System.Threading;
using System.Threading.Tasks;

namespace Framework.Kafka
{
    public interface IKafkaSender
    {
        Task Send<T>(T message, KafkaSenderConfiguration configuration, CancellationToken cancellationToken = default);
    }
}