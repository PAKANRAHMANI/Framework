using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;


namespace Framework.Kafka
{
    public interface IKafkaConsumer<TKey, TMessage>
    {
        void Consume(Action<ConsumeResult<TKey, TMessage>> action, CancellationToken cancellationToken);
        Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, CancellationToken cancellationToken);
        void Consume(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber, CancellationToken cancellationToken);
        Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber, CancellationToken cancellationToken);
        void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topicName, int partitionNumber, CancellationToken cancellationToken);
        Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topicName, int partitionNumber, CancellationToken cancellationToken);
        void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topicName, CancellationToken cancellationToken);
        Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topicName, CancellationToken cancellationToken);
        void Consume(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber, long offset, CancellationToken cancellationToken);
        void Commit(params TopicPartitionOffset[] topicPartitionOffsets);
        void Commit(ConsumeResult<TKey, TMessage> consumeResult);
    }
}
