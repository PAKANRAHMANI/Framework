using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.ChainHandlers;

public interface IHandlerAsync<TData, TResult>
{
    Task<TResult> HandleAsync(TData data, CancellationToken cancellationToken = default);
    void SetNext(IHandlerAsync<TData, TResult> handler);
}