using Framework.Application.Contracts;
using Framework.Config;
using Serilog;

namespace Framework.Logging.SeriLog
{
    public class SerilogModule : IFrameworkModule
    {
        private readonly ILogger _logger;
        private readonly bool _isUsingRequestHandler;

        public SerilogModule(ILogger logger, bool isUsingRequestHandler = false)
        {
            _logger = logger;
            _isUsingRequestHandler = isUsingRequestHandler;
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            var adapter = new SeriLogAdapter(_logger);
            dependencyRegister.RegisterSingleton<Core.Logging.ILogger, SeriLogAdapter>(adapter);

            if (_isUsingRequestHandler)
                dependencyRegister.RegisterDecorator(typeof(IRequestHandler<,>), typeof(LoggingRequestHandlerDecorator<,>));
            else
                dependencyRegister.RegisterDecorator(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        }
    }
}
