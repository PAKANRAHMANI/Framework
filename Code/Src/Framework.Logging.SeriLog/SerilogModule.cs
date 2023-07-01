using Framework.Config;
using Serilog;

namespace Framework.Logging.SeriLog
{
    public class SerilogModule : IFrameworkModule
    {
        private readonly ILogger _logger;

        public SerilogModule(ILogger logger)
        {
            _logger = logger;
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            var adapter = new SeriLogAdapter(_logger);
            dependencyRegister.RegisterSingleton<Core.Logging.ILogger, SeriLogAdapter>(adapter);
        }
    }
}
