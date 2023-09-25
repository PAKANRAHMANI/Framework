using Framework.Core.Events;

namespace Framework.EventProcessor.Filtering
{
    public class NoEventFilter : IEventFilter
    {
        public bool ShouldPublish(IEvent @event)
        {
            return true;
        }
    }
}
