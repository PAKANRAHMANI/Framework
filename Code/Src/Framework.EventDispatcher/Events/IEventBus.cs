using Framework.Core.Events;

namespace Framework.EventProcessor.Events
{
    public interface IEventBus
    {
        Task Publish<T>(T @event) where T : IEvent;
        Task Start();
    }
}
