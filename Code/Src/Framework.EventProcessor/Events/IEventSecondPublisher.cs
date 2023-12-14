using Framework.Core.Events;

namespace Framework.EventProcessor.Events
{
    public interface IEventSecondPublisher
    {
        Task Publish<T>(T @event) where T : IEvent;
    }
}
