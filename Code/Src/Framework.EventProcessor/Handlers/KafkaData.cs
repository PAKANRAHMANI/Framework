using Framework.Core.Events;

namespace Framework.EventProcessor.Handlers;

public class KafkaData
{
    public bool UseOfSpecificPartition { get; set; }
    public IEvent Event { get; set; }
    public string TopicName { get; set; }
    public byte PartitionNumber { get; set; }
    public string TopicKey { get; set; }

    private KafkaData(IEvent @event, bool useOfSpecificPartition, string topicName, byte partitionNumber, string topicKey)
    {
        UseOfSpecificPartition = useOfSpecificPartition;
        Event = @event;
        TopicName = topicName;
        PartitionNumber = partitionNumber;
        TopicKey = topicKey;
    }

    public static KafkaData Create(IEvent @event, bool useOfSpecificPartition, string topicName, byte partitionNumber, string topicKey)
    {

        return new KafkaData(@event, useOfSpecificPartition, topicName, partitionNumber, topicKey);
    }
}