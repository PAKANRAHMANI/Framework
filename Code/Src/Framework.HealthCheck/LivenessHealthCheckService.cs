using Framework.Core.Logging;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Sockets;

namespace Framework.HealthCheck
{
    public class LivenessHealthCheckService : BackgroundService
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly ILogger _logger;
        private readonly HealthCheckConfiguration _healthCheckConfig;
        private readonly TcpListener _livenessListener;
        private readonly ReadinessTcpHealthCheckService _readinessTcpHealthCheckService;

        public LivenessHealthCheckService(
            HealthCheckService healthCheckService,
            ILogger logger,
            HealthCheckConfiguration healthCheckConfig,
            ReadinessTcpHealthCheckService readinessTcpHealthCheckService
            )
        {
            _healthCheckService = healthCheckService;
            _logger = logger;
            _healthCheckConfig = healthCheckConfig;
            _readinessTcpHealthCheckService = readinessTcpHealthCheckService;
            _livenessListener = new TcpListener(IPAddress.Any, healthCheckConfig.LivenessPort);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {

                try
                {

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        _livenessListener.Start();

                        await UpdateHeartbeatAsync(stoppingToken);

                        await Task.Delay(TimeSpan.FromSeconds(_healthCheckConfig.LivenessDelay), stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    if (_healthCheckConfig.LogIsActive)
                        _logger.WriteException(ex);

                    StopTcp();
                }
            }, stoppingToken);
        }

        private async Task UpdateHeartbeatAsync(CancellationToken token)
        {
            try
            {
                var result = await _healthCheckService.CheckHealthAsync(token);

                if (result.Status != HealthStatus.Healthy)
                {
                    foreach (var healthReportEntry in result.Entries)
                    {
                        if (_healthCheckConfig.LogIsActive)
                            _logger.Write($"{healthReportEntry.Key} is {healthReportEntry.Value.Status}", LogLevel.Information);
                    }

                    _livenessListener.Stop();

                    if (_healthCheckConfig.LogIsActive)
                        _logger.Write("Service is unhealthy. Listener has been stopped", LogLevel.Information);

                    return;
                }

                _livenessListener.Start();

                if (_healthCheckConfig.LogIsActive)
                    _logger.Write("LiveNess listener is Listening", LogLevel.Information);

                _readinessTcpHealthCheckService.Start();

            }
            catch (Exception ex)
            {
                if (_healthCheckConfig.LogIsActive)
                    _logger.WriteException(ex);
            }
        }
        public override void Dispose()
        {
            StopTcp();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            StopTcp();

            await Task.CompletedTask;
        }
        private void StopTcp()
        {
            _livenessListener?.Stop();

            _readinessTcpHealthCheckService.Stop();
        }
    }
}
