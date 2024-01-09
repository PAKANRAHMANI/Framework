using Microsoft.Extensions.Configuration;
using Serilog;

namespace Framework.Logging.SeriLog
{
    public static class SerilogConfigurator
    {
        public static ILogger Config(IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            Log.Logger = logger;
            return logger;
        }
    }
}
