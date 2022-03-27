using System;
using Framework.Messages;
using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.MassTransit
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitSender(this IServiceCollection services, RabbitMqHostSettings rabbitMqHostSettings)
        {
            services.AddMassTransit(configurator =>
            {
                configurator.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(rabbitMqHostSettings);

                    cfg.ConfigureEndpoints(context);
                }));
            });

            services.AddSingleton<IMessageSender, MassTransitMessageSender>();

            return services;
        }
        public static IServiceCollection AddMassTransitConsumer<T>(this IServiceCollection services, Action<MassTransitConfiguration> massConfig) where T : class, IConsumer
        {
            var config = new MassTransitConfiguration();

            massConfig.Invoke(config);

            services.AddMassTransit(busConfig =>
            {
                busConfig.AddConsumer<T>();
                busConfig.AddBus(busContext => Bus.Factory.CreateUsingRabbitMq(rabbitMqConfig =>
                {
                    if (config.Priority != null)
                        rabbitMqConfig.EnablePriority(config.Priority.Value);

                    rabbitMqConfig.Host(config.Connection);
                    rabbitMqConfig.ExchangeType = config.ExchangeType;
                    rabbitMqConfig.ReceiveEndpoint(config.QueueName, configEndpoint =>
                    {
                        configEndpoint.ConfigureConsumeTopology = config.ConfigureConsumeTopology;
                        configEndpoint.ExchangeType = config.ExchangeType;

                        if(config.Priority != null)
                         configEndpoint.EnablePriority(config.Priority.Value);

                        configEndpoint.PrefetchCount = config.PrefetchCount;
                        configEndpoint.UseMessageRetry(retryConfig => retryConfig.Interval(config.RetryConfiguration.RetryCount, config.RetryConfiguration.Interval));
                        configEndpoint.ConfigureConsumer<T>(busContext);
                    });
                }));
            });

            return services;
        }
    }
}
