using Framework.Core.Events;

namespace Framework.EventProcessor.Filtering
{
    public sealed class NoEventFilter : IEventFilter
    {
        public bool ShouldPublish(IEvent @event)
        {
            return true;
        }
    }
}
