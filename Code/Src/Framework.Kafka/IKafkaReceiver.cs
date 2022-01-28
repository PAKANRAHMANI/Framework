using System;
using Confluent.Kafka;


namespace Framework.Kafka
{
    public interface IKafkaReceiver
    {
        void Receive<T>(Action<T> action, int? partitionNumber);
    }
}
