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
        public async Task Start()
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(rabbitMqBusFactoryConfigurator =>
            {
                rabbitMqBusFactoryConfigurator.Host(new Uri(_config.RabbitMqConnectionString), rabbitMqHostConfigurator =>
                {
                    rabbitMqHostConfigurator.Username(_config.RabbitMqUserName);
                    rabbitMqHostConfigurator.Password(_config.RabbitMqPassword);
                });
            });

            await _bus.StartAsync();
        }
    }
}
