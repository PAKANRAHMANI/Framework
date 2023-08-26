namespace Framework.EventProcessor.DataStore;

public interface IDataStoreChangeTrackerObserver
{
    Task ChangeDetected(List<EventItem> events);
}