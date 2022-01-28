using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Kafka
{
    public static class KafkaExtensions
    {
        public static IServiceCollection AddKafkaSender(this IServiceCollection services, Action<KafkaSenderConfiguration> config)
        {
            var kafkaConfig = new KafkaSenderConfiguration();

            config?.Invoke(kafkaConfig);

            services.AddScoped(_ => kafkaConfig);

            services.AddScoped<IKafkaSender, KafkaSender>();

            return services;
        }

        public static IServiceCollection AddKafkaReceiver(this IServiceCollection services, Action<KafkaReceiverConfiguration> config)
        {
            var kafkaConfig = new KafkaReceiverConfiguration();

            config?.Invoke(kafkaConfig);

            services.AddScoped(_ => kafkaConfig);

            services.AddScoped<IKafkaReceiver, KafkaReceiver>();

            return services;
        }
    }
}
