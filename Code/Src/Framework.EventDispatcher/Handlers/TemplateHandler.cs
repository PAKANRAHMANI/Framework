using Framework.Core.ChainHandlers;
using Framework.Core.Events;
using Framework.EventProcessor.Services;

namespace Framework.EventProcessor.Handlers;

public abstract class TemplateHandler : BaseHandler<SecondaryData>
{
    public override object Handle(SecondaryData data)
    {
        return UseOfSecondarySending(data.Config) ? MessageSend(data.Event) : CallNext(data);
    }

    protected abstract bool UseOfSecondarySending(ServiceConfig config);
    protected abstract Task MessageSend(IEvent @event);

}