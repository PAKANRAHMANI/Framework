using Framework.Core.Events;


namespace Framework.EventProcessor.Events
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : IEvent;
    }
}
