using Framework.Config;
using Framework.Messages;
using System;

namespace Framework.MassTransit
{
    public class MassTransitModule : IFrameworkModule
    {
        private readonly Type[] _consumers;
        private readonly MassTransitConfiguration _configuration;

        public MassTransitModule(Action<MassTransitConfiguration> config, params Type[] consumers)
        {
            _consumers = consumers;
            _configuration = new MassTransitConfiguration();

            config?.Invoke(_configuration);
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterMassTransit(Map(_configuration), _consumers);

            dependencyRegister.RegisterScoped<IMessageSender, MassTransitMessageSender>();
        }

        private Config.MassTransitConfiguration Map(MassTransitConfiguration configuration)
        {
            return new()
            {
                ProducersExchangeType = configuration.ProducerExchangeType,
                EndpointExchangeType = configuration.EndpointExchangeType,
                Priority = configuration.Priority,
                ConfigureConsumeTopology = configuration.ConfigureConsumeTopology,
                Connection = configuration.Connection,
                RetryConfiguration = new Config.RetryConfiguration()
                {
                    Interval = configuration.RetryConfiguration?.Interval ?? 0,
                    RetryCount = configuration.RetryConfiguration?.RetryCount ?? 0
                },
                EndpointPrefetchCount = configuration.EndpointPrefetchCount,
                QueueName = configuration.QueueName
            };
        }
    }
}

