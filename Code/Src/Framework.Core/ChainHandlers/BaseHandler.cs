using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.ChainHandlers
{
    public abstract class BaseHandler<T> : IHandler<T>
    {
        private IHandler<T> _nextHandler;

        public abstract Task<object> Handle(T request);

        public void SetNext(IHandler<T> handler)
        {
            this._nextHandler = handler;
        }

        protected async Task CallNext(T request)
        {
            if (_nextHandler != null) await _nextHandler?.Handle(request);
        }
    }
}
