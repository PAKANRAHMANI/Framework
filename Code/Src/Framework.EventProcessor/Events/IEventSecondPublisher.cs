using Framework.Core.Events;

namespace Framework.EventProcessor.Events
{
    internal interface IEventSecondPublisher
    {
        Task Publish<T>(T @event) where T : IEvent;
    }
}
