using Confluent.Kafka;
using Framework.Core.ChainHandlers;
using Framework.Core.Events;
using Framework.Core.Filters;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.Events.Kafka;
using Framework.EventProcessor.Handlers;

namespace Framework.EventProcessor.Operations;
//TODO:add async chain to framework
public class PublishEventToSecondKafka : IOperation<IEvent>
{
    private readonly IProducer<string, object> _secondaryProducer;
    private readonly SecondaryProducerConfiguration _producerConfig;
    public PublishEventToSecondKafka(SecondaryProducerConfiguration producerConfig)
    {
        _producerConfig = producerConfig;
        _secondaryProducer = KafkaSecondaryProducerFactory<string, object>.Create(producerConfig);
    }
    public async Task<IEvent> Apply(IEvent input)
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
}