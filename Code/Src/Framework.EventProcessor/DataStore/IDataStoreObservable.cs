using Framework.EventProcessor.DataStore.ChangeTrackers;

namespace Framework.EventProcessor.DataStore;

internal interface IDataStoreObservable
{
    void SetSubscriber(IDataStoreChangeTrackerObserver dataStoreChangeTracker);
    ISubscription SubscribeForChanges();
}