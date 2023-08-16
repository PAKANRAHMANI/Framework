using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.RabbitMQ
{
    public static class RabbitMqExtension
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, Action<RabbitConfiguration> config)
        {
            var rabbitConfiguration = new RabbitConfiguration();

            config.Invoke(rabbitConfiguration);

            services.AddSingleton(rabbitConfiguration);

            services.AddSingleton<IAcknowledgeManagement, AcknowledgeManagement>();

            services.AddSingleton<IRabbitMqMessageSender, RabbitMqMessageSender>();

            return services;
        }
    }
}
