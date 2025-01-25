using System.Threading;
using System.Threading.Tasks;
using Framework.Application.Contracts;
using Framework.Core.Logging;

namespace Framework.Config
{
    public class LoggingCommandHandlerDecorator<T>(ICommandHandler<T> commandHandler, ILogger logger)
        : ICommandHandler<T>
        where T : ICommand
    {
        public async Task Handle(T command, CancellationToken cancellationToken = default)
        {
            logger.Write(
                "class: {className} | method: {methodName} " +
                "|Command is: {@command}.",
                LogLevel.Information,
            nameof(LoggingCommandHandlerDecorator<T>), nameof(Handle), command);

            await commandHandler.Handle(command, cancellationToken);
        }
    }
}
