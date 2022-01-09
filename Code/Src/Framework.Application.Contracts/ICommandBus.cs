using System;
using System.Threading.Tasks;

namespace Framework.Application.Contracts
{
    public interface ICommandBus
    {
        Task Dispatch<T>(T command) where T : class, ICommand;
    }
}
