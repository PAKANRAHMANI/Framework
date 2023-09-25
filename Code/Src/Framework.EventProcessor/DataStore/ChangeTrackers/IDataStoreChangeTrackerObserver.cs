namespace Framework.EventProcessor.DataStore.ChangeTrackers;

public interface IDataStoreChangeTrackerObserver
{
    Task ChangeDetected(List<EventItem> events);
}