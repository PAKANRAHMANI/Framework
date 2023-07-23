using Microsoft.Extensions.DependencyInjection;

namespace Framework.EventProcessor.Initial
{
    public static class EventProcessorServiceExtensions
    {
        public static void AddEventProcessor(this IServiceCollection services, Action<EventProcessorConfigurator> config)
        {
            var configurator = new EventProcessorConfigurator(services);

            config.Invoke(configurator);
        }
    }
}
