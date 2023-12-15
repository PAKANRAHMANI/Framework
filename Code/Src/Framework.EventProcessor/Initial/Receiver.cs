using Framework.Core.Events;

namespace Framework.EventProcessor.Initial;

public sealed class Receiver
{
    private Receiver(ReceiveEventType receiveEventType, IObserver<IEvent> observer)
    {
        ReceiveEventType = receiveEventType;
        Observer = observer;
    }
    public ReceiveEventType ReceiveEventType { get; set; }
    public IObserver<IEvent> Observer { get; set; }

    public static Receiver Create(ReceiveEventType receiveEventType, IObserver<IEvent> observer)
    {
        return new Receiver(receiveEventType, observer);
    }
}