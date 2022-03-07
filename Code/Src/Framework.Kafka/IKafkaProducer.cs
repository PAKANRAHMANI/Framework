using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Framework.Kafka
{
    public interface IKafkaProducer<TKey,TMessage>
    {
        Task<DeliveryResult<TKey,TMessage>> Send(TKey tKey, TMessage message, CancellationToken cancellationToken = default);
    }
}