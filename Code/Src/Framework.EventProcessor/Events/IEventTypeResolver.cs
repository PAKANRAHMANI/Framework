using System.Reflection;

namespace Framework.EventProcessor.Events
{
    public interface IEventTypeResolver
    {
        void AddTypesFromAssembly(Assembly assembly);
        Type GetType(string typeName);
    }
}
