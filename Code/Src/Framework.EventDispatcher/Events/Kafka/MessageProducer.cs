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

    public abstract Task<DeliveryResult<object, object>> ProduceAsync<TKey, TMessage>(TKey key, TMessage message, CancellationToken cancellationToken = default);
}