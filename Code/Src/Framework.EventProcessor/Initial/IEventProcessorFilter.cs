using Framework.EventProcessor.Filtering;

namespace Framework.EventProcessor.Initial;

public interface IEventProcessorFilter
{
    IEventTransformer WithFilter(IEventFilter filter);
    IEventTransformer WithNoFilter();
}