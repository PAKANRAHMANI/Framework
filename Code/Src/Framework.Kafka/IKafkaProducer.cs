using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Framework.Kafka
{
    public interface IKafkaProducer<TKey,TMessage>
    {
        Task<DeliveryResult<TKey,TMessage>> ProduceAsync(TKey key, TMessage message, CancellationToken cancellationToken = default);
        Task<DeliveryResult<TKey,TMessage>> ProduceAsync(TKey key, TMessage message, int partitionNumber, CancellationToken cancellationToken = default);
        void Produce(TKey key, TMessage message, Action<DeliveryResult<TKey, TMessage>> action);
        void Produce(TKey key, TMessage message, int partitionNumber, Action<DeliveryResult<TKey, TMessage>> action);
    }
}