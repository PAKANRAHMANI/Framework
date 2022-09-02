using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.ChainHandlers
{
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
