using System.Collections.Generic;

namespace Framework.Application.Contracts
{
    public interface ICommandHandlerResolver
    {
        IEnumerable<ICommandHandler<T>> ResolveHandlers<T>(T command) where T : ICommand;
    }
}
