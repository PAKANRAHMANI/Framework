using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Framework.Application.Contracts;

namespace Framework.Application;

public class CommandBus(ICommandHandlerResolver resolver) : ICommandBus
{
    public async Task Dispatch<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand
    {
        var handlers = resolver.ResolveHandlers(command).ToList();
        foreach (var handler in handlers)
        {
            await handler.Handle(command, cancellationToken);
        }
    }
}