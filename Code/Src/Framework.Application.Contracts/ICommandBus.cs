using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Application.Contracts
{
    public interface ICommandBus
    {
        Task Dispatch<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand;
    }
}
