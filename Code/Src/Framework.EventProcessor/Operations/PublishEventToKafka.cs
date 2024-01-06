using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.Events.Kafka;

namespace Framework.EventProcessor.Operations;

public class PublishEventToKafka : IOperation<IEvent>
{
    private readonly MessageProducer _producer;
    private readonly ProducerConfiguration _producerConfiguration;
    private readonly Dictionary<Type, KafkaTopicKey> _kafkaKeys;
    private readonly ILogger _logger;

    public PublishEventToKafka(MessageProducer producer, ProducerConfiguration producerConfiguration, Dictionary<Type, KafkaTopicKey> kafkaKeys,ILogger logger)
    {
        _producer = producer;
        _producerConfiguration = producerConfiguration;
        _kafkaKeys = kafkaKeys;
        _logger = logger;
    }
    public async Task<IEvent> Apply(IEvent input)
    {
        try
        {
            var eventType = input.GetType();

            if (_kafkaKeys.ContainsKey(eventType))
            {
                var kafkaKey = _kafkaKeys[input.GetType()];

                await _producer.ProduceAsync(kafkaKey, input);
            }
            else
                await _producer.ProduceAsync(new KafkaTopicKey { Key = _producerConfiguration.TopicKey, Topic = _producerConfiguration.TopicName }, input);

            return input;
        }
        catch (Exception e)
        {
            _logger.WriteException(e);
            return input;
        }
    }
}