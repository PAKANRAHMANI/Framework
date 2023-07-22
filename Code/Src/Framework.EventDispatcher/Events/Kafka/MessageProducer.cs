namespace Framework.EventProcessor.Events.Kafka;
using Confluent.Kafka;

public abstract class MessageProducer
{
    protected readonly ProducerConfiguration Configuration;
    protected readonly IProducer<object, object> Producer;

    protected MessageProducer(ProducerConfiguration configuration, IProducer<object, object> producer)
    {
        Configuration = configuration;
        Producer = producer;
    }

    public abstract void Produce<TKey, TMessage>(TKey key, TMessage message, Action<DeliveryResult<object, object>> action = null);
}