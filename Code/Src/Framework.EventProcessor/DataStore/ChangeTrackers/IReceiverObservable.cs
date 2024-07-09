using Framework.Core.Events;
using Framework.EventProcessor.Initial;

namespace Framework.EventProcessor.DataStore.ChangeTrackers;

internal interface IReceiverObservable
{
    void SetSubscriber(ReceiveEventType receiveEventType, IReceiverObserver receiver);
    void UnSubscriber(ReceiveEventType receiveEventType, IReceiverObserver receiver);
}