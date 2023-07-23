using Framework.Core.Events;
using Framework.EventProcessor.Services;

namespace Framework.EventProcessor.Handlers;

public class SecondaryData
{
    public SecondaryData()
    {

    }
    public SecondaryData(ServiceConfig config, IEvent @event)
    {
        Config = config;
        Event = @event;
    }
    public ServiceConfig Config { get; set; }
    public IEvent Event { get; set; }
}