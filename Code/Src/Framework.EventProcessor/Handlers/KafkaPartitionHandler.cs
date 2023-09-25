using Confluent.Kafka;
using Framework.Core.Events;

namespace Framework.EventProcessor.Handlers;

public class KafkaPartitionHandler : TemplateHandler
{
    private readonly IProducer<string, object> _secondaryProducer;

    public KafkaPartitionHandler(IProducer<string, object> secondaryProducer)
    {
        _secondaryProducer = secondaryProducer;
    }
    protected override bool CanHandle(KafkaData data)
    {
        return data.UseOfSpecificPartition;
    }

    protected override async Task MessageSend(KafkaData data)
    {
        await _secondaryProducer.ProduceAsync(new TopicPartition(data.TopicName, new Partition(data.PartitionNumber)), new Message<string, object>
        {
            Value = data.Event,
            Key = data.TopicKey
        });
    }
}