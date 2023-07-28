using System.Reflection;

namespace Framework.EventProcessor.Initial;

public interface IEventLookup
{
    IEventProcessorFilter UseEventsInAssemblies(params Assembly[] assemblies);
}