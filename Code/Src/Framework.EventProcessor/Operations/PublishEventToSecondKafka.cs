using Confluent.Kafka;
using Framework.Core.ChainHandlers;
using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Handlers;

namespace Framework.EventProcessor.Operations;
internal sealed class PublishEventToSecondKafka(SecondaryProducerConfiguration producerConfig, ILogger logger) : IOperation<IEvent>
{
    private readonly IProducer<string, object> _secondaryProducer = KafkaSecondaryProducerFactory<string, object>.Create(producerConfig, logger);

    public async Task<IEvent> Apply(IEvent input)
    {
        try
        {
            var kafkaData =  KafkaData.Create(input, producerConfig.UseOfSpecificPartition,producerConfig.TopicName,producerConfig.PartitionNumber,producerConfig.TopicKey);

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
            logger.WriteException(exception);
            return await Task.FromResult(input);
        }
    }
}