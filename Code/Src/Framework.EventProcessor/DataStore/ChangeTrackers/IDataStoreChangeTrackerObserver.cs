using Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;

namespace Framework.EventProcessor.DataStore.ChangeTrackers;

internal interface IDataStoreChangeTrackerObserver
{
    Task ChangeDetected(List<EventItem> events, IUpdateCursorPosition updateCursorPosition);
}