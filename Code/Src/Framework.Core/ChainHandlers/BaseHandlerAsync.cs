using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.ChainHandlers;

public abstract class BaseHandlerAsync<TData, TResult> : IHandlerAsync<TData, TResult>
{
    private IHandlerAsync<TData, TResult> _nextHandler;

    public abstract Task<TResult> HandleAsync(TData data, CancellationToken cancellationToken = default);

    protected async Task<TResult> CallNextAsync(TData data, CancellationToken cancellationToken = default)
    {
        return await _nextHandler.HandleAsync(data, cancellationToken);
    }

    public void SetNext(IHandlerAsync<TData, TResult> handler)
    {
        this._nextHandler = handler;
    }
}