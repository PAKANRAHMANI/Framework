using System;
using System.Threading.Tasks;
using Confluent.Kafka;


namespace Framework.Kafka
{
    public interface IKafkaConsumer<TKey, TMessage>
    {
        void Consume(Action<ConsumeResult<TKey, TMessage>> action);
        Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action);
        void Consume(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber);
        Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber);
        void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topicName, int partitionNumber);
        Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topicName, int partitionNumber);
        void Consume(Action<ConsumeResult<TKey, TMessage>> action, string topicName);
        Task ConsumeAsync(Action<ConsumeResult<TKey, TMessage>> action, string topicName);
    }
}
