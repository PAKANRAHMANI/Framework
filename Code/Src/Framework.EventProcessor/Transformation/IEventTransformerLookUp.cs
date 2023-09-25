using Framework.Core.Events;

namespace Framework.EventProcessor.Transformation
{
    public interface IEventTransformerLookUp
    {
        IEventTransformer LookUpTransformer(IEvent @event);
    }
}
