namespace Framework.EventProcessor.DataStore.ChangeTrackers;

internal interface IDataStoreChangeTrackerObserver
{
    Task ChangeDetected(List<EventItem> events);
}