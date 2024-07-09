using Framework.EventProcessor.DataStore.ChangeTrackers;

namespace Framework.EventProcessor.Initial;

public sealed class Receiver
{
    private Receiver(ReceiveEventType receiveEventType, IReceiverObserver observer)
    {
        ReceiveEventType = receiveEventType;
        Observer = observer;
    }
    public ReceiveEventType ReceiveEventType { get; set; }
    public IReceiverObserver Observer { get; set; }

    public static Receiver Create(ReceiveEventType receiveEventType, IReceiverObserver observer)
    {
        return new Receiver(receiveEventType, observer);
    }
}