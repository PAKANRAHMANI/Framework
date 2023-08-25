using Framework.Config;
using System;

namespace Framework.RabbitMQ
{
    public class RabbitMQModule : IFrameworkModule
    {
        private readonly RabbitConfiguration _config;

        public RabbitMQModule(Action<RabbitConfiguration>  configuration)
        {
            _config = new RabbitConfiguration();

            configuration.Invoke(_config);
        }

        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterSingleton(typeof(RabbitConfiguration), _config);

            dependencyRegister.RegisterSingleton<IAcknowledgeManagement, AcknowledgeManagement>();

            dependencyRegister.RegisterSingleton<IRabbitMqMessageSender, RabbitMqMessageSender>();
        }
    }
}
