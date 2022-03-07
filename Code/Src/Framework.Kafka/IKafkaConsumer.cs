using System;
using Confluent.Kafka;


namespace Framework.Kafka
{
    public interface IKafkaConsumer<TKey, TMessage>
    {
        void Receive(Action<TMessage> action);
        void Receive(Action<TMessage> action, int partitionNumber);
    }
}
