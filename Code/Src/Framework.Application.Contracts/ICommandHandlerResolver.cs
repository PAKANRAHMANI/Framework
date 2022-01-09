using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Application.Contracts
{
    public interface ICommandHandlerResolver
    {
        IEnumerable<ICommandHandler<T>> ResolveHandlers<T>(T command) where T : ICommand;
    }
}
