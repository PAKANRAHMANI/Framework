namespace Framework.EventProcessor.DataStore;

public interface IDataStoreObservable
{
    void SetSubscriber(IDataStoreChangeTrackerObserver dataStoreChangeTracker);
    ISubscription SubscribeForChanges();
}