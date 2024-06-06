using Framework.Core.ChainHandlers;

namespace Framework.EventProcessor.Handlers;

internal abstract class TemplateHandler : BaseHandler<KafkaData>
{
    public override object Handle(KafkaData data)
    {
        return CanHandle(data) ? MessageSend(data) : CallNext(data);
    }

    protected abstract bool CanHandle(KafkaData data);
    protected abstract Task MessageSend(KafkaData data);

}