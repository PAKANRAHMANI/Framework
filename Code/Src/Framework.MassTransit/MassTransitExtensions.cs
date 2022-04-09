using Framework.Messages;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using GreenPipes;

namespace Framework.MassTransit
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection RegisterMassTransit(this IServiceCollection services, Action<MassTransitConfiguration> massConfig, params Type[] consumers)
        {
            services.AddScoped<IMessageSender, MassTransitMessageSender>();

            var config = new MassTransitConfiguration();

            massConfig.Invoke(config);

            services.AddSingleton(config);

            services.AddMassTransit(configurator =>
            {
                configurator.AddBus(context => Bus.Factory.CreateUsingRabbitMq(configure =>
                {
                    if (config.Priority != null)
                        configure.EnablePriority(config.Priority.Value);

                    configure.Host(config.Connection);
                    configure.ExchangeType = config.ExchangeType;
                    foreach (var consumer in consumers)
                    {
                        configurator.AddConsumer(consumer);

                        configure.ReceiveEndpoint(config.QueueName, configureEndpoint =>
                        {
                            if (config.Priority != null)
                                configureEndpoint.EnablePriority(config.Priority.Value);

                            configureEndpoint.ConfigureConsumeTopology = config.ConfigureConsumeTopology;
                            configureEndpoint.Consumer(consumer, _ => Activator.CreateInstance(consumer));
                            configureEndpoint.ExchangeType = config.ExchangeType;

                            if (config.RetryConfiguration != null)
                                configureEndpoint.UseMessageRetry(messageConfig => messageConfig
                                    .Interval(config.RetryConfiguration.RetryCount,
                                        config.RetryConfiguration.Interval));
                        });
                        configure.ConfigureEndpoints(context);
                    }
                }));
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
