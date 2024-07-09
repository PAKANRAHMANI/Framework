using Framework.Core.Events;
using Framework.EventProcessor.Initial;

namespace Framework.EventProcessor.DataStore.ChangeTrackers;

internal class EventObservable : IReceiverObservable
{
    private readonly List<IReceiverObserver> _primaryEventObservers= [];
    private readonly List<IReceiverObserver> _transformedEventObservers= [];


    internal EventObservable(IReadOnlyCollection<Receiver> receivers)
    {
        foreach (var receiver in receivers.Where(receiver => receiver.ReceiveEventType == ReceiveEventType.PrimaryEvent).ToList())
        {
            SetSubscriber(receiver.ReceiveEventType, receiver.Observer);
        }
        foreach (var receiver in receivers.Where(receiver => receiver.ReceiveEventType == ReceiveEventType.Transformed).ToList())
        {
            SetSubscriber(receiver.ReceiveEventType, receiver.Observer);
        }
    }

    protected void SendPrimaryEventToAllListeners(IEvent @event)
    {
        _primaryEventObservers.ForEach(observer => observer.Receive(@event));
    }
    protected void SendTransformedEventToAllListeners(IEvent @event)
    {
        _transformedEventObservers.ForEach(observer => observer.Receive(@event));
    }

    public void SetSubscriber(ReceiveEventType receiveEventType, IReceiverObserver observer)
    {
        if (receiveEventType == ReceiveEventType.PrimaryEvent)
            this._primaryEventObservers.Add(observer);
        else
            this._transformedEventObservers.Add(observer);
    }

    public void UnSubscriber(ReceiveEventType receiveEventType, IReceiverObserver observer)
    {
        if (receiveEventType == ReceiveEventType.PrimaryEvent)
            this._primaryEventObservers.Remove(observer);
        else
            this._transformedEventObservers.Remove(observer);
    }
}