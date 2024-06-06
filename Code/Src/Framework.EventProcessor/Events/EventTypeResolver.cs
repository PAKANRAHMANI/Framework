using Framework.Core.Events;
using System.Reflection;

namespace Framework.EventProcessor.Events
{
    public sealed class EventTypeResolver : IEventTypeResolver
    {
        private readonly Dictionary<string, Type> _eventTypeNames = new();
        private readonly Dictionary<string, Type> _eventTypesFullNames = new();
        public void AddTypesFromAssembly(Assembly assembly)
        {
            var assemblyEventTypes = assembly.GetTypes().Where(type => typeof(IEvent).IsAssignableFrom(type)).ToList();

            assemblyEventTypes.ForEach(type =>
            {
                _eventTypeNames.Add(type.Name, type);
                if (type.FullName != null) _eventTypesFullNames.Add(type.FullName, type);
            });
        }

        public Type GetType(string typeName)
        {
            return _eventTypeNames.ContainsKey(typeName) ? _eventTypeNames[typeName] : _eventTypesFullNames.ContainsKey(typeName) ? _eventTypesFullNames[typeName] : null;
        }
    }
}
