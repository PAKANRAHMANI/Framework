using Framework.Core.Logging;
using Serilog.Events;

namespace Framework.EventProcessor.Initial
{
    internal class SerilogAdapter : ILogger
    {
        private readonly Serilog.ILogger _logger;

        public SerilogAdapter(Serilog.ILogger logger)
        {
            _logger = logger;
        }
        public void Write(string message, LogLevel level)
        {
            _logger.Write((LogEventLevel)level, message);
        }

        public void Write(string template, LogLevel level, params object[] parameters)
        {
            _logger.Write((LogEventLevel)level, template, parameters);
        }

        public void WriteException(Exception exception)
        {
            _logger.Error(exception, "error");
        }
    }
}
