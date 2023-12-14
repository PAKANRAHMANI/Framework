using System.Reflection;

namespace Framework.EventProcessor.Initial;

internal interface IEventLookup
{
    IEventProcessorFilter UseEventsInAssemblies(params Assembly[] assemblies);
}