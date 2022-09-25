using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Framework.EventProcessor.Initial
{
    public static class EventProcessorServiceExtensions
    {
        public static void AddEventProcessor(this IServiceCollection services, Action<EventProcessorConfigurator> config)
        {
            services.AddHostedService<EventWorker>();

            var configurator = new EventProcessorConfigurator(services);

            config.Invoke(configurator);
        }
    }
}
