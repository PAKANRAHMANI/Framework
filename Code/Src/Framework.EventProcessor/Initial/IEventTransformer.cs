using System.Reflection;

namespace Framework.EventProcessor.Initial;

internal interface IEventTransformer
{
    IEventSenderBuilder UseEventTransformersInAssemblies(params Assembly[] assemblies);
    IEventSenderBuilder WithNoEventTransformer();
}