using Framework.Core.Events;

namespace Framework.EventProcessor.Events
{
    public interface IEventBus
    {
        Task Start();
    }
}
