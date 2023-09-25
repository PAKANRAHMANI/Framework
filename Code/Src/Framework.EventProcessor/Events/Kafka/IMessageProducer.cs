using Confluent.Kafka;

namespace Framework.EventProcessor.Events.Kafka;

public interface IMessageProducer
{
    void Produce<TKey, TMessage>(TKey key, TMessage message, Action<DeliveryResult<object, object>> action = null);
}