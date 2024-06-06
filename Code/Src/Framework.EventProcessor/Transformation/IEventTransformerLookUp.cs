using Framework.Core.Events;

namespace Framework.EventProcessor.Transformation
{
    internal interface IEventTransformerLookUp
    {
        IEventTransformer LookUpTransformer(IEvent @event);
    }
}
