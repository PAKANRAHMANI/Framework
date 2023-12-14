using Framework.Core.Events;

namespace Framework.EventProcessor.Initial;

internal interface IEventConsumer
{
    IEventProcessor EnableReceiveEvent(params Receiver[] receivers);
}