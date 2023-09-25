using Framework.EventProcessor.Configurations;

namespace Framework.EventProcessor.Initial;

public interface IEventSenderBuilder
{
    IEnableSecondSenderBuilder PublishEventWithMassTransit(Action<MassTransitConfig> config);
    IEnableSecondSenderBuilder ProduceMessageWithKafka(Action<ProducerConfiguration> config);
}