using Automatonymous;
using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.Events.Kafka;

namespace Framework.EventProcessor.Operations;

public class PublishEventToKafka : IOperation<IEvent>
{
    private readonly MessageProducer _producer;
    private readonly ProducerConfiguration _producerConfiguration;

    public PublishEventToKafka(MessageProducer producer, ProducerConfiguration producerConfiguration)
    {
        _producer = producer;
        _producerConfiguration = producerConfiguration;
    }
    public async Task<IEvent> Apply(IEvent input)
    {
        await _producer.ProduceAsync(_producerConfiguration.TopicKey, input);

        return input;
    }
}