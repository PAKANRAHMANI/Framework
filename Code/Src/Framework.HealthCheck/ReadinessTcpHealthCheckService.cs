using Framework.Core.Logging;
using System.Net;
using System.Net.Sockets;

namespace Framework.HealthCheck
{
    public class ReadinessTcpHealthCheckService
    {
        private readonly ILogger _logger;
        private readonly TcpListener _listener;
        private bool _isStart = false;
        public ReadinessTcpHealthCheckService(HealthCheckConfiguration healthCheckConfig, ILogger logger)
        {
            _logger = logger;
            _listener = new TcpListener(IPAddress.Any, healthCheckConfig.ReadinessPort);
        }
        public void Start()
        {
            try
            {
                if (_isStart)
                    return;

                _logger.Write("going to listen to readyNes port", LogLevel.Information);

                _listener.Start();

                _isStart = true;

                _logger.Write("readyNes listener is Listening", LogLevel.Information);
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex);

                _listener.Stop();
            }
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}
