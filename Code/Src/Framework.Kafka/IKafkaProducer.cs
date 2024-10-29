using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Framework.Kafka
{
    public interface IKafkaProducer<TKey, TMessage>
    {
        Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, string topicName, int partitionNumber, CancellationToken cancellationToken = default, string eventId = null);
        Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, CancellationToken cancellationToken = default, string eventId = null);
        Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, KeyValuePair<string, string>[] headers, CancellationToken cancellationToken = default, string eventId = null);
        Task<DeliveryResult<TKey, TMessage>> ProduceAsync(TKey key, TMessage message, int partitionNumber, CancellationToken cancellationToken = default, string eventId = null);
        void Produce(TKey key, TMessage message, Action<DeliveryResult<TKey, TMessage>> action = null, string eventId = null);
        void Produce(TKey key, TMessage message, int partitionNumber, Action<DeliveryResult<TKey, TMessage>> action = null, string eventId = null);
        void Produce(TKey key, TMessage message, string topicName, int partitionNumber, Action<DeliveryResult<TKey, TMessage>> action = null, string eventId = null);
        void Produce(TKey key, TMessage message, string topicName, Action<DeliveryResult<TKey, TMessage>> action = null, string eventId = null);
    }
}