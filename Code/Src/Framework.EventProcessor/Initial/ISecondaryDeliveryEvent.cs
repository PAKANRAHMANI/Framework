using Framework.EventProcessor.Configurations;

namespace Framework.EventProcessor.Initial;

public interface ISecondaryDeliveryEvent
{
    IEventProcessor SecondaryDeliveryWithKafka(Action<SecondaryProducerConfiguration> config);
    IEventProcessor SecondaryDeliveryWithMassTransit(Action<SecondaryMassTransitConfiguration> config);
}