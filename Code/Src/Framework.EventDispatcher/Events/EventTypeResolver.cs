using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Events;

namespace Framework.EventProcessor.Events
{
    public class EventTypeResolver : IEventTypeResolver
    {
        private readonly Dictionary<string, Type> _eventTypes = new();
        public void AddTypesFromAssembly(Assembly assembly)
        {
            var assemblyEventTypes = assembly.GetTypes().Where(type => typeof(IEvent).IsAssignableFrom(type)).ToList();

            assemblyEventTypes.ForEach(type =>
            {
                _eventTypes.Add(type.Name, type);
            });
        }

        public Type GetType(string typeName)
        {
            return _eventTypes.ContainsKey(typeName) ? _eventTypes[typeName] : null;
        }
    }
}
