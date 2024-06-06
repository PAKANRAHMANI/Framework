using Framework.Core.Events;

namespace Framework.EventProcessor.DataStore.ChangeTrackers;

internal sealed class UnSubscriber<TEvent> : IDisposable where TEvent : IEvent
{
    private readonly List<IObserver<IEvent>> _observers;
    private readonly IObserver<IEvent> _observer;

    internal UnSubscriber(List<IObserver<IEvent>> observers, IObserver<IEvent> observer) => (_observers, _observer) = (observers, observer);

    public void Dispose() => _observers.Remove(_observer);
}