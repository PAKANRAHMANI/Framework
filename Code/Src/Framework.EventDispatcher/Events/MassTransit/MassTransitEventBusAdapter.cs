using Framework.Core.Events;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Framework.EventProcessor.Events.MassTransit
{
    public class MassTransitEventBusAdapter : IEventBus
    {
        private IBusControl _bus;
        private readonly MassTransitConfig _config;

        public MassTransitEventBusAdapter(IOptions<MassTransitConfig> config)
        {
            _config = config.Value;
        }
        public Task Publish<T>(T @event) where T : IEvent
        {
            return _bus.Publish(@event);
        }

        public async Task Start()
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(_config.RabbitMqConnectionString);
            });

            await _bus.StartAsync();
        }
    }
}
