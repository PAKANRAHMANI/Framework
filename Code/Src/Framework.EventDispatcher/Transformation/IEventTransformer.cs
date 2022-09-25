using Framework.Core.Events;

namespace Framework.EventProcessor.Transformation
{
    public interface IEventTransformer
    {
        IEvent Transform(IEvent @event);
    }
}
