using Confluent.Kafka;
using Framework.Core.Events;

namespace Framework.EventProcessor.Handlers;

public class KafkaTopicHandler : TemplateHandler
{
    private readonly IProducer<string, object> _secondaryProducer;

    public KafkaTopicHandler(IProducer<string, object> secondaryProducer)
    {
        _secondaryProducer = secondaryProducer;
    }
    protected override bool CanHandle(KafkaData data)
    {
        return data.UseOfSpecificPartition == false;
    }

    protected override async Task MessageSend(KafkaData data)
    {
        await _secondaryProducer.ProduceAsync(data.TopicName, new Message<string, object>
        {
            Value = data.Event,
            Key = data.TopicKey
        });
    }
}