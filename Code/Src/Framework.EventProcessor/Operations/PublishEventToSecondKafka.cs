using Confluent.Kafka;
using Framework.Core.ChainHandlers;
using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Handlers;

namespace Framework.EventProcessor.Operations;
public class PublishEventToSecondKafka : IOperation<IEvent>
{
    private readonly IProducer<string, object> _secondaryProducer;
    private readonly SecondaryProducerConfiguration _producerConfig;
    private readonly ILogger _logger;

    public PublishEventToSecondKafka(SecondaryProducerConfiguration producerConfig,ILogger logger)
    {
        _producerConfig = producerConfig;
        _logger = logger;
        _secondaryProducer = KafkaSecondaryProducerFactory<string, object>.Create(producerConfig, logger);
    }
    public async Task<IEvent> Apply(IEvent input)
    {
        try
        {
            var kafkaData =  KafkaData.Create(input, _producerConfig.UseOfSpecificPartition,_producerConfig.TopicName,_producerConfig.PartitionNumber,_producerConfig.TopicKey);

            var handlers =
                new ChainBuilder<KafkaData>()
                    .WithHandler(new KafkaTopicHandler(_secondaryProducer))
                    .WithHandler(new KafkaPartitionHandler(_secondaryProducer))
                    .WithEndOfChainHandler()
                    .Build();

            handlers.Handle(kafkaData);

            return await Task.FromResult(input);
        }
        catch (Exception exception)
        {
            _logger.WriteException(exception);
            return await Task.FromResult(input);
        }
    }
}