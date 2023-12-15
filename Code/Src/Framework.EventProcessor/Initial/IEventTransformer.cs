using System.Reflection;

namespace Framework.EventProcessor.Initial;

public interface IEventTransformer
{
    IEventSenderBuilder UseEventTransformersInAssemblies(params Assembly[] assemblies);
    IEventSenderBuilder WithNoEventTransformer();
}