using System;
using Confluent.Kafka;


namespace Framework.Kafka
{
    public interface IKafkaConsumer<TKey, TMessage>
    {
        void Consume(Action<ConsumeResult<TKey, TMessage>> action);
        void Consume(Action<ConsumeResult<TKey, TMessage>> action, int partitionNumber);
    }
}
