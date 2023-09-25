using Framework.Core.Events;
using System.Reflection;

namespace Framework.EventProcessor.Transformation
{
    public class EventTransformerLookUp : IEventTransformerLookUp
    {
        private readonly Dictionary<string, Type> _transformers = new();
        public void AddTypesFromAssembly(Assembly assembly)
        {
            var eventTypes = assembly.GetTypes()
                .Where(IsImplementationOfEventTransformer)
                .ToList();

            eventTypes.ForEach(transformer =>
            {
                var typeOfEvent = transformer.BaseType?.GetGenericArguments().First();

                _transformers.Add(typeOfEvent!.Name, transformer);
            });
        }

        private static bool IsImplementationOfEventTransformer(Type type)
        {
            return type.BaseType is { IsGenericType: true } && type.BaseType.GetGenericTypeDefinition() == typeof(EventTransformer<>);
        }

        public IEventTransformer LookUpTransformer(IEvent @event)
        {
            var nameOfEvent = @event.GetType().Name;

            if (!_transformers.ContainsKey(nameOfEvent)) return null;

            return Activator.CreateInstance(_transformers[nameOfEvent]) as IEventTransformer;
        }
    }
}
