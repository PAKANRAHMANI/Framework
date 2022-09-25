using Framework.EventProcessor.DataStore.Sql;
using Framework.EventProcessor.Events;
using Framework.EventProcessor.Events.MassTransit;
using Framework.EventProcessor.Filtering;
using Framework.EventProcessor.Transformation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Framework.EventProcessor.DataStore;

namespace Framework.EventProcessor.Initial
{
    public class EventProcessorConfigurator
    {
        private readonly IServiceCollection _services;

        public EventProcessorConfigurator(IServiceCollection serviceCollection)
        {
            _services = serviceCollection;
        }

        public EventProcessorConfigurator ReadFromSqlServer(Action<SqlStoreConfig> config)
        {
            _services.AddSingleton<IDataStoreObservable, SqlDataStore>();

            _services.Configure<SqlStoreConfig>(config);

            return this;
        }
        public EventProcessorConfigurator PublishWithMassTransit(Action<MassTransitConfig> config)
        {
            _services.AddSingleton<IEventBus, MassTransitEventBusAdapter>();

            _services.Configure<MassTransitConfig>(config);

            return this;
        }
        public EventProcessorConfigurator WithFilter(IEventFilter filter)
        {
            _services.AddSingleton(filter);
            return this;
        }

        public EventProcessorConfigurator WithNoFilter()
        {
            return WithFilter(new NoFilter());
        }

        public EventProcessorConfigurator UseEventsInAssemblies(params Assembly[] assemblies)
        {
            var eventTypeResolver = new EventTypeResolver();
            if (assemblies.Length > 0)
            {
                foreach (var assembly in assemblies)
                    eventTypeResolver.AddTypesFromAssembly(assembly);
            }
            _services.AddSingleton<IEventTypeResolver>(eventTypeResolver);
            return this;
        }

        public EventProcessorConfigurator UseEventTransformersInAssemblies(params Assembly[] assemblies)
        {
            var transformerLookUp = new EventTransformerLookUp();
            if (assemblies.Length > 0)
            {
                foreach (var assembly in assemblies)
                    transformerLookUp.AddTypesFromAssembly(assembly);
            }
            _services.AddSingleton<IEventTransformerLookUp>(transformerLookUp);
            return this;
        }

        public EventProcessorConfigurator WithNoEventTransformer()
        {
            return UseEventTransformersInAssemblies();
        }
    }
}
