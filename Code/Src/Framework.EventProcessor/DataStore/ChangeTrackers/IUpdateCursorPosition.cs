namespace Framework.EventProcessor.DataStore.ChangeTrackers;

public interface IUpdateCursorPosition
{
    void MoveCursorPosition(EventItem eventItem);
}