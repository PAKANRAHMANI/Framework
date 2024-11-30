using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.ChainHandlers
{
    public class ChainBuilder<TData, TResult>
    {
        private readonly List<IHandler<TData, TResult>> _handlers;
        public ChainBuilder()
        {
            this._handlers = new List<IHandler<TData, TResult>>();
        }

        public ChainBuilder<TData, TResult> WithHandler(IHandler<TData, TResult> handler)
        {
            this._handlers.Add(handler);
            return this;
        }

        public ChainBuilder<TData, TResult> WithEndOfChainHandler()
        {
            return WithHandler(new EndOfChainHandler<TData, TResult>());
        }

        public IHandler<TData, TResult> Build()
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

    public class ChainBuilder<T>
    {
        private readonly List<IHandler<T>> _handlers;

        public ChainBuilder()
        {
            this._handlers = new List<IHandler<T>>();
        }

        public ChainBuilder<T> WithHandler(IHandler<T> handler)
        {
            this._handlers.Add(handler);
            return this;
        }

        public ChainBuilder<T> WithEndOfChainHandler()
        {
            return WithHandler(new EndOfChainHandler<T>());
        }

        public IHandler<T> Build()
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
}