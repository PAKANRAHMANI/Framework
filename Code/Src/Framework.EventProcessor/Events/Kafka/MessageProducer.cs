using Framework.Core.Events;

namespace Framework.EventProcessor.Events.Kafka;
using Confluent.Kafka;
using Framework.EventProcessor.Configurations;

public abstract class MessageProducer
{
    protected readonly ProducerConfiguration Configuration;
    protected readonly IProducer<string, object> Producer;

    protected MessageProducer(ProducerConfiguration configuration, IProducer<string, object> producer)
    {
        Configuration = configuration;
        Producer = producer;
    }

    public abstract Task<DeliveryResult<string, object>> ProduceAsync<TMessage>(string key, TMessage message, CancellationToken cancellationToken = default) where TMessage : class;
}