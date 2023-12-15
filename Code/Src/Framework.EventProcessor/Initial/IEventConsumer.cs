using Framework.Core.Events;

namespace Framework.EventProcessor.Initial;

public interface IEventConsumer
{
    IEventProcessor EnableReceiveEvent(params Receiver[] receivers);
    IEventProcessor DisableReceiveEvent();
}