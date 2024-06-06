using Framework.Core.Events;


namespace Framework.EventProcessor.Events
{
    internal interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : IEvent;
    }
}
