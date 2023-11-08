using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.ChainHandlers;

public class ChainBuilderAsync<TData, TResult>
{
    private readonly List<IHandlerAsync<TData, TResult>> _handlers;
    public ChainBuilderAsync()
    {
        this._handlers = new List<IHandlerAsync<TData, TResult>>();
    }

    public ChainBuilderAsync<TData, TResult> WithHandler(IHandlerAsync<TData, TResult> handler)
    {
        this._handlers.Add(handler);
        return this;
    }

    public ChainBuilderAsync<TData, TResult> WithEndOfChainHandler()
    {
        return WithHandler(new EndOfChainHandlerAsync<TData, TResult>());
    }

    public IHandlerAsync<TData, TResult> Build()
    {
        if (!_handlers.Any()) throw new Exception("No handler has been added");

        _handlers.Aggregate((a, b) =>
        {
            a.SetNext(b);
            return b;
        });

        return _handlers.First();
    }
}