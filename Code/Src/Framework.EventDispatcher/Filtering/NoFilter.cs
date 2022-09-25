using Framework.Core.Events;

namespace Framework.EventProcessor.Filtering
{
    public class NoFilter : IEventFilter
    {
        public bool ShouldPublish(IEvent @event)
        {
            return true;
        }
    }
}
