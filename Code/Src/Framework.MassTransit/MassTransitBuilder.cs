using System;
using System.Collections.Generic;
using Framework.Messages;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.MassTransit
{
    public class MassTransitBuilder
    {
        private readonly IServiceCollection _services;
        private readonly MassTransitConfiguration _config;
        private readonly List<Type> _consumers = new();
        private bool _hasConsume = false;
        private bool _addMassTransitBus = false;
        public MassTransitBuilder(IServiceCollection services, Action<MassTransitConfiguration> massConfig)
        {
            _services = services;
            _config = new MassTransitConfiguration();

            massConfig.Invoke(_config);
        }
        public MassTransitBuilder RegisterSender()
        {
            _addMassTransitBus = true;
            _services.AddSingleton<IMessageSender, MassTransitMessageSender>();
            return this;
        }
        public MassTransitBuilder EnableConsume()
        {
            this._hasConsume = true;
            return this;
        }
        public MassTransitBuilder AddConsumer(Type consumerType)
        {
            this._consumers.Add(consumerType);
            return this;
        }

        public void Build()
        {
            _services.AddMassTransit(configurator =>
            {
                if (_addMassTransitBus)
                {
                    configurator.AddBus(context => Bus.Factory.CreateUsingRabbitMq(rabbitMqConfig =>
                    {

                        if (_config.Priority != null)
                            rabbitMqConfig.EnablePriority(_config.Priority.Value);
                        rabbitMqConfig.Host(_config.Connection);
                        rabbitMqConfig.ExchangeType = _config.ExchangeType;
                        rabbitMqConfig.ConfigureEndpoints(context);
                    }));
                }

                if (!_hasConsume) return;
                {
                    foreach (var consumer in _consumers)
                    {
                        configurator.AddConsumer(consumer);

                        configurator.UsingRabbitMq((context, rabbitConfig) =>
                        {
                            rabbitConfig.Host(_config.Connection);
                            rabbitConfig.ReceiveEndpoint(_config.QueueName, configEndpoint =>
                            {
                                configEndpoint.ConfigureConsumeTopology = _config.ConfigureConsumeTopology;
                                configEndpoint.ExchangeType = _config.ExchangeType;

                                if (_config.Priority != null)
                                    configEndpoint.EnablePriority(_config.Priority.Value);

                                configEndpoint.PrefetchCount = _config.PrefetchCount;

                                if (_config.RetryConfiguration != null)
                                    configEndpoint.UseMessageRetry(retryConfig => retryConfig.Interval(_config.RetryConfiguration.RetryCount, _config.RetryConfiguration.Interval));
                                
                                configEndpoint.ConfigureConsumer(context, consumer);
                            });
                            rabbitConfig.ConfigureEndpoints(context);

                        });
                    }
                }
            });

            _services.AddMassTransitHostedService();
        }
    }

}
