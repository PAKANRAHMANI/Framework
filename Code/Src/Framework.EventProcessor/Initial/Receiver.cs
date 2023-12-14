using Framework.Core.Events;

namespace Framework.EventProcessor.Initial;

internal sealed class Receiver
{
    public ReceiveEventType ReceiveEventType { get; set; }
    public IObserver<IEvent> Observer { get; set; }
}