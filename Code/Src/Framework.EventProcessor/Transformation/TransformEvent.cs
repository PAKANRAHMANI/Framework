using Framework.Core.Events;

namespace Framework.EventProcessor.Transformation
{
    public static class TransformEvent
    {
        public static IEvent Transform(IEventTransformerLookUp transformerLookUp, IEvent @event)
        {
            var transformer = transformerLookUp.LookUpTransformer(@event);

            if (transformer == null) return @event;

            @event = transformer.Transform(@event);

            return @event;
        }
    }
}
