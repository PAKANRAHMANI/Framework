using Framework.Core.Events;

namespace Framework.EventProcessor.Filtering
{
    public interface IEventFilter
    {
        bool ShouldPublish(IEvent @event);
    }
}
