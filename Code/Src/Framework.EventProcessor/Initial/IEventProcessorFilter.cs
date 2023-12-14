using Framework.EventProcessor.Filtering;

namespace Framework.EventProcessor.Initial;

internal interface IEventProcessorFilter
{
    IEventTransformer WithFilter(IEventFilter filter);
    IEventTransformer WithNoFilter();
}