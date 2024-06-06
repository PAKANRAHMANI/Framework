using Framework.Core.Events;
using Framework.EventProcessor.Initial;

namespace Framework.EventProcessor.DataStore.ChangeTrackers;

internal class EventObservable : IObservable<IEvent>
{
    private readonly List<Receiver> _receivers;
    private readonly List<IObserver<IEvent>> _observers;


    internal EventObservable(List<Receiver> receivers)
    {
        _receivers = receivers;
        _observers = receivers.Select(receiver => receiver.Observer).ToList();
    }
    public IDisposable Subscribe(IObserver<IEvent> observer)
    {
        this._observers.Add(observer);

        return new UnSubscriber<IEvent>(_observers, observer);
    }
    protected void SendPrimaryEventToAllListeners(IEvent @event)
    {
        foreach (var observer in _receivers.Where(receiver => receiver.ReceiveEventType == ReceiveEventType.PrimaryEvent).Select(receiver => receiver.Observer).ToList())
            observer.OnNext(@event);
    }
    protected void SendTransformedEventToAllListeners(IEvent @event)
    {
        foreach (var observer in _receivers.Where(receiver => receiver.ReceiveEventType == ReceiveEventType.Transformed).Select(receiver => receiver.Observer).ToList())
            observer.OnNext(@event);
    }
}