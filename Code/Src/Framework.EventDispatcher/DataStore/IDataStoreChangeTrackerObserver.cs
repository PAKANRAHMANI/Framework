namespace Framework.EventProcessor.DataStore;

public interface IDataStoreChangeTrackerObserver
{
    void ChangeDetected(List<EventItem> events);
}