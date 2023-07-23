using Framework.Core.Events;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Services;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace Framework.EventProcessor.Handlers;

public class MassTransitHandler : TemplateHandler
{
    private readonly IPublishEndpoint _publishEndpoint;
    public MassTransitHandler(Bind<ISecondBus, IPublishEndpoint> publishEndpoint)
    {
        _publishEndpoint = publishEndpoint.Value;
    }
    protected override bool UseOfSecondarySending(ServiceConfig config)
    {
        return config.EnableSecondarySending && config.SendWithMassTransit;
    }

    protected override async Task MessageSend(IEvent @event)
    {
        await _publishEndpoint.Publish(@event);
    }
}