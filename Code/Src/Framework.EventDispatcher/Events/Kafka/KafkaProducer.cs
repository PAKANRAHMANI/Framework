using Confluent.Kafka;
using Framework.Core.Streaming;

namespace Framework.EventProcessor.Events.Kafka;

internal abstract class KafkaProducer<TKey, TMessage> where TMessage : IStream
{
    protected readonly ProducerConfiguration Configuration;
    protected readonly IProducer<TKey, TMessage> Producer;
    public event EventHandler<ErrorEventArgs> Error;

    protected KafkaProducer(ProducerConfiguration configuration)
    {
        Configuration = configuration;
        var config = new ProducerConfig()
        {
            BootstrapServers = configuration.BootstrapServer,
            MessageTimeoutMs = configuration.MessageTimeoutMs,
            Acks = configuration.Acks,
            EnableIdempotence = configuration.EnableIdempotence
        };

        var producerBuilder = new ProducerBuilder<TKey, TMessage>(config);

        producerBuilder.SetErrorHandler(OnError);

        Producer = new ProducerBuilder<TKey, TMessage>(config).Build();
    }
    protected abstract void Produce(TKey key, TMessage message, Action<DeliveryResult<TKey, TMessage>> action = null);

    private void OnError(IProducer<TKey, TMessage> producer, Error error)
    {
        Error?.Invoke(this, new ErrorEventArgs(new KafkaException(error)));
    }
}