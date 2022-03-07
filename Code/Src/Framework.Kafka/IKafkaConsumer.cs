using System;
using Confluent.Kafka;


namespace Framework.Kafka
{
    public interface IKafkaConsumer<TKey, TMessage>
    {
        void Consume(Action<Message<TKey, TMessage>> action);
        void Consume(Action<Message<TKey, TMessage>> action, int partitionNumber);
    }
}
