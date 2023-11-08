using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.ChainHandlers;

public class EndOfChainHandlerAsync<TData, TResult> : IHandlerAsync<TData, TResult>
{
    public Task<TResult> HandleAsync(TData data, CancellationToken cancellationToken = default)
    {
        throw new Exception("No other handler has processed the request");
    }

    public void SetNext(IHandlerAsync<TData, TResult> handler)
    {
        throw new NotSupportedException();
    }
}