using Framework.Core.Logging;
using Serilog.Events;

namespace Framework.EventProcessor.Initial
{
    internal sealed class SerilogAdapter(Serilog.ILogger logger) : ILogger
    {
        public void Write(string message, LogLevel level)
        {
            logger.Write((LogEventLevel)level, message);
        }

        public void Write(string template, LogLevel level, params object[] parameters)
        {
            logger.Write((LogEventLevel)level, template, parameters);
        }

        public void WriteException(Exception exception)
        {
            logger.Error(exception, "error");
        }
    }
}
