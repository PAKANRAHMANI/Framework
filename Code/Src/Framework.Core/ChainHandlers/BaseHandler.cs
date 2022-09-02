using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.ChainHandlers
{
    public abstract class BaseHandler<T> : IHandler<T>
    {
        private IHandler<T> _nextHandler;

        public abstract object Handle(T request);

        public void SetNext(IHandler<T> handler)
        {
            this._nextHandler = handler;
        }

        protected object CallNext(T request)
        {
            return _nextHandler?.Handle(request);
        }
    }
}
