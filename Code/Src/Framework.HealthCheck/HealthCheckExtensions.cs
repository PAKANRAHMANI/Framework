
using Microsoft.Extensions.DependencyInjection;

namespace Framework.HealthCheck
{
    public static class HealthCheckExtensions
    {
        public static void Run(this IHealthChecksBuilder healthChecksBuilder, Action<HealthCheckConfiguration> config)
        {
            var healthCheckConfiguration = new HealthCheckConfiguration();

            config.Invoke(healthCheckConfiguration);

            healthChecksBuilder.Services.AddSingleton(healthCheckConfiguration);

            healthChecksBuilder.Services.AddSingleton<ReadinessTcpHealthCheckService>();

            healthChecksBuilder.Services.AddHostedService<LivenessHealthCheckService>();
        }

    }
}
